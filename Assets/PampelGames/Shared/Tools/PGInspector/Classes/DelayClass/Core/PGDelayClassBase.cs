// ---------------------------------------------------
// Copyright (c) Pampel Games e.K. All Rights Reserved.
// https://www.pampelgames.com
// ---------------------------------------------------

using System;
using UnityEngine;

namespace PampelGames.Shared.Tools.PGInspector
{
    /// <summary>
    ///     Abstract base class for shared Delay implementations. All virtual methods should be called by the implementing MonoBehaviour.
    /// </summary>
    [Serializable]
    public abstract class PGDelayClassBase
    {
        /* Editor Virtual******************************************************************************************************************/

#if UNITY_EDITOR
        /// <summary>
        ///     Name of the Delay used for custom inspectors.
        /// </summary>
        public virtual string DelayName()
        {
            return "";
        }

        /// <summary>
        ///     Info about the Delay, used as Tooltip.
        /// </summary>
        public virtual string DelayInfo()
        {
            return "";
        }
#endif

        protected PGSchedulerDescr scheduler;
        
        public virtual void ExecutionPreStart(MonoBehaviour mono, PGIHeader pgiHeader, Action ExecuteAction)
        {
        }
        public virtual void ExecutionStart(MonoBehaviour mono, PGIHeader pgiHeader)
        {
            PGScheduler.Stop(scheduler);
        }
        public virtual void ExecutionStop(MonoBehaviour baseComponent, PGIHeader pgiHeader)
        {
            PGScheduler.Stop(scheduler);
        }
        public virtual void ExecutionPause(MonoBehaviour mono, PGIHeader pgiHeader)
        {
            PGScheduler.Pause(scheduler);
        }
        public virtual void ExecutionResume(MonoBehaviour mono, PGIHeader pgiHeader)
        {
            PGScheduler.Resume(scheduler);
        }
    }
}