// ----------------------------------------------------
// Gore Simulator
// Copyright (c) Pampel Games e.K. All Rights Reserved.
// https://www.pampelgames.com
// ----------------------------------------------------

using System;
using System.Collections.Generic;
using PampelGames.Shared.Tools;
using UnityEngine;
using UnityEngine.Pool;
using Object = UnityEngine.Object;

namespace PampelGames.GoreSimulator
{
    public static class Pool
    {
        
        internal static GameObject Get(GameObject prefab)
        {
            var newObject = PGPool.Get(prefab);
            return newObject;
        }
        
        internal static void Release(GameObject detachedObject)
        {
            PGPool.Release(detachedObject);    
        }
        
        internal static GameObject Get(GameObject prefab, int instanceID)
        {
            var newObject = PGPool.Get(prefab);
            if (newObject.TryGetComponent<GorePoolable>(out var poolable)) poolable.m_InstanceID = instanceID;

            return newObject;
        }

        internal static void Release(GameObject detachedObject, int instanceID)
        {
            if (detachedObject == null) return;
            if (detachedObject.TryGetComponent<GorePoolable>(out var poolable))
            {
                if (poolable.m_InstanceID == instanceID) PGPool.Release(detachedObject);    
            }
        }
        

        /********************************************************************************************************************************/
        internal static GameObject[] InitializePool(GameObject prefab, int preloadAmount, bool limited)
        {
            var pool = PGPool.TryGetExistingPool(prefab) ?? new ObjectPool<GameObject>(
                () => CreateSetup(prefab),
                GetSetup,
                ReleaseSetup,
                DestroySetup,
                true,
                preloadAmount);
            return PGPool.Preload(prefab, pool, preloadAmount, limited);
        }

        /********************************************************************************************************************************/

        private static GameObject CreateSetup(GameObject prefab)
        {
            var obj = Object.Instantiate(prefab);
            return obj;
        }

        private static void GetSetup(GameObject obj)
        {
#if UNITY_EDITOR
            if(SO_GlobalSettings.Instance.hidePooledObjects) obj.hideFlags = HideFlags.None;
#endif
            obj.SetActive(true);
        }

        static readonly List<Type> excludedCompTypes = new() {typeof(Transform), typeof(GorePoolable)};
        private static void ReleaseSetup(GameObject obj)
        {
            Component[] components = obj.GetComponents<Component>();
            foreach (var component in components)
            {
                if (component is MeshFilter filter)
                {
                    filter.mesh = null;
                    continue;
                }
                if (component is MeshRenderer renderer)
                {
                    renderer.materials = Array.Empty<Material>();
                    continue;
                }
                if (excludedCompTypes.Contains(component.GetType())) continue;
                Object.Destroy(component);
            }
#if UNITY_EDITOR
            if(SO_GlobalSettings.Instance.hidePooledObjects) obj.hideFlags = HideFlags.HideInHierarchy;
#endif
            obj.transform.SetParent(null);
            obj.SetActive(false);
        }

        private static void DestroySetup(GameObject obj)
        {
            Object.Destroy(obj);
        }
    }
}