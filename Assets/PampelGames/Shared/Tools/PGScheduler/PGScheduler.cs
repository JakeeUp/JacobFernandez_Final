// ---------------------------------------------------
// Copyright (c) Pampel Games e.K. All Rights Reserved.
// https://www.pampelgames.com
// ---------------------------------------------------

using System;
using PampelGames.Shared.Utility;
using UnityEngine;

namespace PampelGames.Shared.Tools
{
    public static class PGScheduler
    {
        /* Public *******************************************************************************************************************************/

        public static void Pause(PGSchedulerDescr scheduler)
        {
            if (scheduler == null) return;
            scheduler.active = false;
            scheduler.internalEvents.onPause?.Invoke();
        }

        public static void Resume(PGSchedulerDescr scheduler)
        {
            if (scheduler == null) return;
            if (!scheduler.completed) scheduler.active = true;
            scheduler.internalEvents.onResume?.Invoke();
        }

        public static void Stop(PGSchedulerDescr scheduler)
        {
            if (scheduler == null) return;
            scheduler.stopped = true;
            scheduler.internalEvents.onStop?.Invoke();
        }

        /********************************************************************************************************************************/

        private static PGSchedulerDescr SetupScheduler(PGEnums.UpdateMode updateMode, float duration, bool isFrameScheduler)
        {
            var scheduler = new PGSchedulerDescr
            {
                duration = duration,
                active = true,
                isFrameScheduler = isFrameScheduler,
                yieldInstruction = PGEnums.GetYieldInstruction(updateMode)
            };

            return scheduler;
        }

        /********************************************************************************************************************************/

        /// <summary>
        ///     Updates an Action every frame.
        /// </summary>
        /// <param name="mono">MonoBehaviour as base for the coroutine.</param>
        /// <param name="action">Action to update.</param>
        /// <param name="updateMode">Update loop to use.</param>
        /// <param name="duration">Optional. If != 0, stops after the duration.</param>
        /// <param name="frames">Optional. If != 0, stops after the amount of frames.</param>
        /// <returns></returns>
        public static PGSchedulerDescr UpdateLoop(MonoBehaviour mono, Action action, PGEnums.UpdateMode updateMode = PGEnums.UpdateMode.Update,
            float duration = 0, int frames = 0)
        {
            bool isFrameScheduler = frames != 0;
            if (duration == 0 && !isFrameScheduler) duration = Mathf.Infinity;
            if (frames == 0 && isFrameScheduler) frames = int.MaxValue;
            var scheduler = SetupScheduler(updateMode, isFrameScheduler ? frames : duration, isFrameScheduler);
            scheduler.internalEvents.onUpdate = action;
            var newCoroutine = mono.StartCoroutine(PGSchedulerUpdate._SchedulerUpdate(scheduler));
            scheduler._coroutine = newCoroutine;
            return scheduler;
        }

        /********************************************************************************************************************************/

        public static PGSchedulerDescr ScheduleTime(MonoBehaviour mono, float delayDuration, Action action)
        {
            var scheduler = SetupScheduler(PGEnums.UpdateMode.Update, delayDuration, false);
            scheduler.internalEvents.onComplete = action;
            var newCoroutine = mono.StartCoroutine(PGSchedulerUpdate._SchedulerUpdate(scheduler));
            scheduler._coroutine = newCoroutine;
            return scheduler;
        }


        /********************************************************************************************************************************/

        public static PGSchedulerDescr ScheduleFrames(MonoBehaviour mono, int delayFrames, Action action)
        {
            var scheduler = SetupScheduler(PGEnums.UpdateMode.Update, delayFrames, true);
            scheduler.internalEvents.onComplete = action;
            var newCoroutine = mono.StartCoroutine(PGSchedulerUpdate._SchedulerUpdate(scheduler));
            scheduler._coroutine = newCoroutine;
            return scheduler;
        }
    }
}