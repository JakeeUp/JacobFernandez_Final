// ---------------------------------------------------
// Copyright (c) Pampel Games e.K. All Rights Reserved.
// https://www.pampelgames.com
// ---------------------------------------------------

using System;
using System.Collections;
using System.Collections.Generic;
using PampelGames.Shared.Utility;
using UnityEngine;
using UnityEngine.Events;

namespace PampelGames.Shared.Tools
{
    
    /// <summary>
    ///  Can be attached to a <see cref="PGPool"/>ed object. Invokes Unity Events when being spawned or unspawned and has unspawn methods.
    /// </summary>

    public class PGOnPoolable : PGPoolable
    {

        #region Variables
        

        [Tooltip("Destroy gameobject after execution.")]
        public bool destroyAfterExecute;
        
        [Tooltip("Only invokes one method from each list.")] [SerializeField]
        private bool randomInvoke;
        
        [SerializeField] [Min(0)]
        [Tooltip("Invoke probability. Match with set events from top to bottom. \n" +
                 "If array number is missing, (only) that number is automatically set to 1.")]
        private float[] eventInstancesWeights;
        
        /********************************************************************************************************************************/
        
        [Header("Events")]

        [SerializeField]
        private List<OnPoolSpawnClass> m_OnPoolSpawn = new();
        
        [Serializable]
        public class OnPoolSpawnClass
        {
            public DelayEnum setDelay = DelayEnum.None;
            
            [Min(0)] public float delaySeconds;
            
            [Min(0)] public int delayFrames;     
            
            public UnityEvent onPoolSpawn;
        }
        
        [SerializeField]
        private List<OnPoolOnPoolUnSpawnClass> m_OnPoolUnSpawn = new();
        
        [Serializable]
        public class OnPoolOnPoolUnSpawnClass
        {
            public DelayEnum setDelay = DelayEnum.None;
            
            [Min(0)] public float delaySeconds;
            
            [Min(0)] public int delayFrames;     
            
            public UnityEvent onPoolUnSpawn;
        }
        public enum DelayEnum
        {
            None,
            Seconds,
            Frames
        }
        
        
        /********************************************************************************************************************************/
        
        public List<OnPoolSpawnClass> onPoolSpawn { get => m_OnPoolSpawn; set => m_OnPoolSpawn = value; }
        public List<OnPoolOnPoolUnSpawnClass> onPoolUnSpawn { get => m_OnPoolUnSpawn; set => m_OnPoolUnSpawn = value; }

        /********************************************************************************************************************************/
        
        #endregion
        

        internal override void OnPoolSpawn()
        {
            if (m_OnPoolSpawn.Count == 0) return;

            var coroutineExecuted = false;

            if (randomInvoke)
            {
                var randomEntry = PGMathUtility.GetRandomArrayEntry(m_OnPoolSpawn.Count, eventInstancesWeights);
                if (m_OnPoolSpawn[randomEntry].setDelay != DelayEnum.None)
                {
                    StartCoroutine(_OnPoolSpawn(m_OnPoolSpawn[randomEntry]));
                    coroutineExecuted = true;
                }

                m_OnPoolSpawn[randomEntry].onPoolSpawn.Invoke();
            }
            else
                foreach (var onPoolSpawnClass in m_OnPoolSpawn)
                {
                    if (onPoolSpawnClass.setDelay != DelayEnum.None)
                    {
                        StartCoroutine(_OnPoolSpawn(onPoolSpawnClass));
                        coroutineExecuted = true;
                        continue;
                    }

                    onPoolSpawnClass.onPoolSpawn.Invoke();
                }

            if (!coroutineExecuted && destroyAfterExecute)
                Destroy(gameObject);
        }

        internal override void OnPoolUnSpawn()
        {
            if (m_OnPoolUnSpawn.Count == 0) return;

            var coroutineExecuted = false;

            if (randomInvoke)
            {
                var randomEntry = PGMathUtility.GetRandomArrayEntry(m_OnPoolUnSpawn.Count, eventInstancesWeights);
                if (m_OnPoolUnSpawn[randomEntry].setDelay != DelayEnum.None)
                {
                    StartCoroutine(_OnPoolUnSpawn(m_OnPoolUnSpawn[randomEntry]));
                    coroutineExecuted = true;
                }

                m_OnPoolUnSpawn[randomEntry].onPoolUnSpawn.Invoke();
            }
            else
                foreach (var onPoolUnSpawnClass in m_OnPoolUnSpawn)
                {
                    if (onPoolUnSpawnClass.setDelay != DelayEnum.None)
                    {
                        StartCoroutine(_OnPoolUnSpawn(onPoolUnSpawnClass));
                        coroutineExecuted = true;
                        continue;
                    }

                    onPoolUnSpawnClass.onPoolUnSpawn.Invoke();
                }

            if (!coroutineExecuted && destroyAfterExecute)
                Destroy(gameObject);
        }

        /********************************************************************************************************************************/

        private IEnumerator _OnPoolSpawn(OnPoolSpawnClass onPoolSpawnClass)
        {
            yield return new WaitForSeconds(onPoolSpawnClass.delaySeconds);

            for (int i = 0; i < onPoolSpawnClass.delayFrames; i++)
                yield return null;

            onPoolSpawnClass.onPoolSpawn.Invoke();
            
            if(destroyAfterExecute) Destroy(gameObject);
        }
        
        private IEnumerator _OnPoolUnSpawn(OnPoolOnPoolUnSpawnClass onPoolUnSpawnClass)
        {
            yield return new WaitForSeconds(onPoolUnSpawnClass.delaySeconds);
            
            for (int i = 0; i < onPoolUnSpawnClass.delayFrames; i++)
                yield return null;
            
            onPoolUnSpawnClass.onPoolUnSpawn.Invoke();
            
            if(destroyAfterExecute) Destroy(gameObject);
        }
        
        
        
        /* Public *******************************************************************************************************************************/

        public void DespawnGameobject()
        {
            ReleaseObjectToPool(gameObject);
        }
        
        public void DespawnGameobject(GameObject go)
        {
            ReleaseObjectToPool(go);
        }

        public void DespawnGameobject(float delay)
        {
            ReleaseObjectToPool(gameObject, delay);
        }
        
        public void DespawnGameobject(GameObject go, float delay)
        {
            ReleaseObjectToPool(go, delay);
        }
        
        
        /* Private *******************************************************************************************************************************/

        private void ReleaseObjectToPool(GameObject go, float delay = 0f)
        {
            if (delay == 0f) PGPool.Release(go);

            else
                StartCoroutine(_ReleaseObjectToPool(go, delay));
            
        }
        
        private IEnumerator _ReleaseObjectToPool(GameObject go, float delay)
        {
            yield return new WaitForSeconds(delay);
            PGPool.Release(go);
        }
        
    }
}