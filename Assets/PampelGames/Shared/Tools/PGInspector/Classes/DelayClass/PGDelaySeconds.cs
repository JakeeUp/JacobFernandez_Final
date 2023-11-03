// ----------------------------------------------------
// Copyright (c) Pampel Games e.K. All Rights Reserved.
// https://www.pampelgames.com
// ----------------------------------------------------

using System;
using UnityEngine;

namespace PampelGames.Shared.Tools.PGInspector
{
    public class PGDelaySeconds : PGDelayClassBase
    {
        /* Editor Virtual******************************************************************************************************************/

#if UNITY_EDITOR
        public override string DelayName()
        {
            return "Seconds";
        }
        public override string DelayInfo()
        {
            return "Delays execution for the specified amount of seconds.";
        }
#endif

        [Tooltip("Delay in seconds.")] public float delaySeconds = 1f;


        public override void ExecutionPreStart(MonoBehaviour mono, PGIHeader pgiHeader, Action ExecuteAction)
        {
            base.ExecutionPreStart(mono, pgiHeader, ExecuteAction);
            scheduler = PGScheduler.ScheduleTime(mono, delaySeconds, ExecuteAction);
        }

    }
}