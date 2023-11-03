// ---------------------------------------------------
// Copyright (c) Pampel Games e.K. All Rights Reserved.
// https://www.pampelgames.com
// ---------------------------------------------------

using System.Collections.Generic;
#if UNITY_EDITOR
using UnityEditor;
#endif
using UnityEngine;
using UnityEngine.Pool;
using Object = UnityEngine.Object;

namespace PampelGames.Shared.Tools
{
    /// <summary>
    ///     Pool class that can preload objects.
    /// </summary>
    public static class PGPool
    {
        private static readonly Dictionary<GameObject, PoolsStruct> poolsStructDictionary = new();

        private struct PoolsStruct
        {
            public ObjectPool<GameObject> pool;
            public LinkedList<GameObject> sceneObjects;
            public bool limited;
        }
        
#if UNITY_EDITOR

        private static PlayModeStateChange currentPlayMode;
        [InitializeOnLoadMethod]
        private static void EditorInit()
        {
            EditorApplication.playModeStateChanged -= Change; 
            EditorApplication.playModeStateChanged += Change;  
        }
    
        private static void Change(PlayModeStateChange state)
        {
            currentPlayMode = state;
            if (state != PlayModeStateChange.EnteredEditMode) return;
            poolsStructDictionary.Clear();
        }
#endif

        /********************************************************************************************************************************/
        /* Public ***********************************************************************************************************************/
        
        /// <summary>
        ///     Checks for an existing pool of a prefab.
        /// </summary>
        /// <returns>Returns existing pool or null.</returns>
        public static ObjectPool<GameObject> TryGetExistingPool(GameObject prefab)
        {
            return poolsStructDictionary.TryGetValue(prefab, out var poolsStruct) ? poolsStruct.pool : null;
        }
        

        /// <summary>
        ///     Preloads objects into the scene using Pool.Get() and Pool.Release().
        /// </summary>
        /// <param name="prefab">Prefab used for dictionary look up.</param>
        /// <param name="_pool">Existing pool.</param>
        /// <param name="loadAmount">If pool exists, only loads until pooled count is equal to the loadAmount.</param>
        /// <param name="limited">
        ///     Objects in the scene are limited to the preload amount. If false and the pool reaches the max size, any further instances
        ///     returned to the pool will be ignored and can be garbage collected.
        /// </param>
        /// <returns>Newly created objects (Use GetPooledObjects for all objects).</returns>
        public static GameObject[] Preload(GameObject prefab, ObjectPool<GameObject> _pool, int loadAmount, bool limited)
        {
            if (!poolsStructDictionary.TryGetValue(prefab, out var poolsStruct))
            {
                var newPoolsStruct = new PoolsStruct
                {
                    pool = _pool,
                    sceneObjects = new LinkedList<GameObject>(),
                    limited = limited
                };
                poolsStructDictionary.Add(prefab, newPoolsStruct);
                poolsStruct = newPoolsStruct;
            }

            if (loadAmount <= poolsStruct.pool.CountAll) return null;

            var newLoadedObjects = new GameObject[loadAmount - poolsStruct.pool.CountAll];
            for (var i = 0; i < newLoadedObjects.Length; i++)
            {
                newLoadedObjects[i] = poolsStruct.pool.Get();

                if (!newLoadedObjects[i].TryGetComponent<PGPoolable>(out var pgPoolable))
                {
                    pgPoolable = newLoadedObjects[i].AddComponent<PGPoolable>();
                }

                var pgPoolables = newLoadedObjects[i].GetComponentsInChildren<PGPoolable>();
                foreach (var poolable in pgPoolables) poolable.prefab = prefab;

                poolsStruct.sceneObjects.AddLast(newLoadedObjects[i]);
            }

            for (var i = newLoadedObjects.Length - 1; i >= 0; i--) poolsStruct.pool.Release(newLoadedObjects[i]);

            return newLoadedObjects;
        }

        /// <summary>
        ///     Spawns an <see cref="UnityEngine.GameObject" /> from an initialized pool.
        /// </summary>
        /// <param name="prefab">Prefab from the project folder. Used for dictionary look up.</param>
        /// <returns>The Object spawned into the scene.</returns>
        public static GameObject Get(GameObject prefab)
        {
            if (!poolsStructDictionary.TryGetValue(prefab, out var foundPoolStruct))
            {
                return null;
            }

            var queued = false;
            if (foundPoolStruct.pool.CountInactive == 0)
            {
                if (foundPoolStruct.limited)
                {
                    if(foundPoolStruct.sceneObjects.First != null) foundPoolStruct.pool.Release(foundPoolStruct.sceneObjects.First.Value);
                }
                    
                queued = true;
            }

            var obj = foundPoolStruct.pool.Get();

            var pgPoolables = obj.GetComponentsInChildren<PGPoolable>();
            if (pgPoolables == null || pgPoolables.Length == 0)
            {
                var pgPoolable = obj.AddComponent<PGPoolable>();
                pgPoolable.prefab = prefab;
                pgPoolable.pooled = false;
                pgPoolable.OnPoolSpawn();
            }
            else
            {
                foreach (var pgPoolable in pgPoolables)
                {
                    pgPoolable.pooled = false;
                    pgPoolable.prefab = prefab;
                    pgPoolable.OnPoolSpawn();
                }
            }
            
            if (queued)
            {
                if (foundPoolStruct.limited)
                {
                    if(foundPoolStruct.sceneObjects.First != null) foundPoolStruct.sceneObjects.RemoveFirst();
                }
                foundPoolStruct.sceneObjects.AddLast(obj);
            }

            return obj;
        }

        /// <summary>
        ///     Releases an <see cref="UnityEngine.GameObject" /> to an existing pool.
        /// </summary>
        /// <param name="obj">Object that was spawned into the scene. Must be stored by the using class.</param>
        public static void Release(GameObject obj)
        {
#if UNITY_EDITOR
            if (currentPlayMode == PlayModeStateChange.ExitingPlayMode)
            {
                return;
            }
#endif
            if (obj == null)
            {
                return;
            }

            var pgPoolables = obj.GetComponentsInChildren<PGPoolable>();
            if (pgPoolables == null || pgPoolables.Length == 0)
            {
                Debug.LogWarning(obj.name + " has no PG_Poolable component and will be destroyed!");
                Object.Destroy(obj);
                return;
            }

            if (pgPoolables[0].prefab == null)
            {
                Debug.LogWarning("Original prefab not specified for " + obj.name);
                return;
            }

            if (!poolsStructDictionary.TryGetValue(pgPoolables[0].prefab, out var foundPoolStruct))
            {
                Debug.LogWarning("No pool found for " + obj.name);
                return;
            }
            
            foreach (var pgPoolable in pgPoolables)
            {
                if (pgPoolable.pooled) return;
                pgPoolable.pooled = true;
                pgPoolable.OnPoolUnSpawn();
            }

            foundPoolStruct.pool.Release(obj);
        }
        
        
        /********************************************************************************************************************************/
        
        public static int GetCountActive(GameObject prefab)
        {
            return poolsStructDictionary.TryGetValue(prefab, out var foundPoolStruct) ? foundPoolStruct.pool.CountActive : 0;
        }

        public static int GetCountInactive(GameObject prefab)
        {
            return poolsStructDictionary.TryGetValue(prefab, out var foundPoolStruct) ? foundPoolStruct.pool.CountInactive : 0;
        }
        
        
        /// <summary>
        ///     Gets all pooled objects from the specified prefab that are active or inactive in the scene.
        /// </summary>
        /// <param name="prefab">Prefab from the project folder. Used for dictionary look up.</param>
        /// <returns>LinkedList referencing all cloned objects of the prefab in the scene.</returns>
        public static LinkedList<GameObject> GetPooledGameObjects(GameObject prefab)
        {
            if (!poolsStructDictionary.TryGetValue(prefab, out var foundPoolStruct))
            {
                var pgPoolable = prefab.GetComponent<PGPoolable>();
                if (pgPoolable != null) return GetPooledGameObjects(pgPoolable);
                else
                {
                    Debug.LogWarning("No pool found for " + prefab.name);
                    return null;    
                }
            }
            
            return foundPoolStruct.sceneObjects;
        }
        
        public static LinkedList<GameObject> GetPooledGameObjects(PGPoolable pgPoolable)
        {
            if (!poolsStructDictionary.TryGetValue(pgPoolable.prefab, out var foundPoolStruct))
            {
                Debug.LogWarning("No pool found for " + pgPoolable.prefab.name);
                return null;
            }
            
            return foundPoolStruct.sceneObjects;
        }
        

        /// <summary>
        ///     Destroys all pooled objects from the specified prefab that are active or inactive in the scene.
        /// </summary>
        /// <returns>True when pool existed and was destroyed.</returns>
        public static bool DestroyPooledGameObjects(GameObject prefab)
        {
            if (!poolsStructDictionary.TryGetValue(prefab, out var foundPoolStruct))
            {
                var pgPoolable = prefab.GetComponent<PGPoolable>();
                if (pgPoolable != null) return DestroyPooledGameObjects(pgPoolable);
                return false;
            }
            foreach (var sceneObject in foundPoolStruct.sceneObjects) Release(sceneObject);
            foundPoolStruct.pool.Clear();
            foundPoolStruct.sceneObjects.Clear();
            return true;
        }
        
        public static bool DestroyPooledGameObjects(PGPoolable pgPoolable)
        {
            if (!poolsStructDictionary.TryGetValue(pgPoolable.prefab, out var foundPoolStruct))
                return false;
            
            foreach (var sceneObject in foundPoolStruct.sceneObjects) Release(sceneObject);
            foundPoolStruct.pool.Clear();
            foundPoolStruct.sceneObjects.Clear();
            return true;
        }
        
        

        

    }
}