// ---------------------------------------------------
// Copyright (c) Pampel Games e.K. All Rights Reserved.
// https://www.pampelgames.com
// ---------------------------------------------------

using System;
using UnityEngine;

namespace PampelGames.Shared.Tools.PGInspector
{
    /// <summary>
    ///     Abstract base class for shared Execute implementations. All virtual methods should be called by the implementing MonoBehaviour.
    /// </summary>
    [Serializable]
    public abstract class PGExecuteClassBase
    {
        /* Editor Virtual******************************************************************************************************************/

#if UNITY_EDITOR
        /// <summary>
        ///     Name of the Execute used for custom inspectors.
        /// </summary>
        public virtual string ExecuteName()
        {
            return "";
        }

        /// <summary>
        ///     Info about the Execute, used as Tooltip.
        /// </summary>
        public virtual string ExecuteInfo()
        {
            return "";
        }
#endif

        protected bool isPaused;
        protected bool isExecuting;
        public virtual void ComponentStart(MonoBehaviour baseComponent, Action ExecuteAction)
        {
        }
        public virtual void ComponentOnEnable(MonoBehaviour baseComponent, Action ExecuteAction)
        {
        }
        public virtual void ComponentOnCollisionEnter(MonoBehaviour baseComponent, Action ExecuteAction, Collision collision)
        {
        }
        public virtual void ComponentOnTriggerEnter(MonoBehaviour baseComponent, Action ExecuteAction, Collider other)
        {
        }
        public virtual void ComponentOnTriggerExit(MonoBehaviour baseComponent, Action ExecuteAction, Collider other)
        {
        }
        public virtual void ComponentOnBecameVisible(MonoBehaviour baseComponent, Action ExecuteAction)
        {
        }
        public virtual void ComponentOnBecameInvisible(MonoBehaviour baseComponent, Action ExecuteAction)
        {
        }
        public virtual void ComponentOnParticleCollision(MonoBehaviour baseComponent, Action ExecuteAction)
        {
        }
        public virtual void ExecutionStart(MonoBehaviour baseComponent, Action ExecuteAction)
        {
            isExecuting = true;
        }
        public virtual void ExecutionStop(MonoBehaviour baseComponent, Action ExecuteAction)
        {
            isExecuting = false;
        }
        public virtual void ExecutionPause(MonoBehaviour baseComponent, Action ExecuteAction)
        {
            isPaused = true;
        }
        public virtual void ExecutionResume(MonoBehaviour baseComponent, Action ExecuteAction)
        {
            isPaused = false;
        }
    }
}