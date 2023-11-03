// ---------------------------------------------------
// Copyright (c) Pampel Games e.K. All Rights Reserved.
// https://www.pampelgames.com
// ---------------------------------------------------

using System;
using UnityEngine;

namespace PampelGames.Shared.Tools.PGInspector
{
    /// <summary>
    ///     Abstract base class for shared Stop implementations. All virtual methods should be called by the implementing MonoBehaviour.
    /// </summary>
    [Serializable]
    public abstract class PGStopClassBase
    {
        /* Editor Virtual******************************************************************************************************************/

#if UNITY_EDITOR
        /// <summary>
        ///     Name of the Stop used for custom inspectors.
        /// </summary>
        public virtual string StopName()
        {
            return "";
        }

        /// <summary>
        ///     Info about the Stop, used as Tooltip.
        /// </summary>
        public virtual string StopInfo()
        {
            return "";
        }
#endif

        protected bool isPaused;
        protected bool isExecuting;
        public virtual void ComponentStart(MonoBehaviour baseComponent, Action StopAction)
        {
        }
        public virtual void ComponentOnEnable(MonoBehaviour baseComponent, Action StopAction)
        {
        }
        public virtual void ComponentOnCollisionEnter(MonoBehaviour baseComponent, Action StopAction, Collision collision)
        {
        }
        public virtual void ComponentOnTriggerEnter(MonoBehaviour baseComponent, Action StopAction, Collider other)
        {
        }
        public virtual void ComponentOnTriggerExit(MonoBehaviour baseComponent, Action StopAction, Collider other)
        {
        }
        public virtual void ComponentOnBecameVisible(MonoBehaviour baseComponent, Action StopAction)
        {
        }
        public virtual void ComponentOnBecameInvisible(MonoBehaviour baseComponent, Action StopAction)
        {
        }
        public virtual void ComponentOnParticleCollision(MonoBehaviour baseComponent, Action StopAction)
        {
        }
        public virtual void ExecutionStart(MonoBehaviour baseComponent, Action StopAction)
        {
            isExecuting = true;
        }
        public virtual void ExecutionStop(MonoBehaviour baseComponent, Action StopAction)
        {
            isExecuting = false;
        }
        public virtual void ExecutionPause(MonoBehaviour baseComponent, Action StopAction)
        {
            isPaused = true;
        }
        public virtual void ExecutionResume(MonoBehaviour baseComponent, Action StopAction)
        {
            isPaused = false;
        }
    }
}