// ----------------------------------------------------
// Copyright (c) Pampel Games e.K. All Rights Reserved.
// https://www.pampelgames.com
// ----------------------------------------------------

using System;
using UnityEngine;

namespace PampelGames.Shared.Editor.EditorTools
{
    public static class PGEditorScheduler
    {
        public static PGEditorSchedulerDescr ScheduleTime(float delayDuration, Action action)
        {
            var scheduler = new PGEditorSchedulerDescr
            {
                duration = delayDuration,
                onComplete = action
            };

            PGEditorCoroutineUtility.StartCoroutine(PGEditorSchedulerUpdate._SchedulerUpdate(scheduler));
            return scheduler;
        }
    }
}
