// ----------------------------------------------------
// Copyright (c) Pampel Games e.K. All Rights Reserved.
// https://www.pampelgames.com
// ----------------------------------------------------

using System;
using PampelGames.Shared.Tools;
using UnityEngine;

namespace PampelGames.Shared.Tools.PGInspector
{
    public class PGDelayFrames : PGDelayClassBase
    {
        /* Editor Virtual******************************************************************************************************************/

#if UNITY_EDITOR
        public override string DelayName()
        {
            return "Frames";
        }
        public override string DelayInfo()
        {
            return "Delays execution for the specified amount of frames.";
        }
#endif

        [Tooltip("Delay in frames.")] public int delayFrames = 1;
        
        public override void ExecutionPreStart(MonoBehaviour mono, PGIHeader pgiHeader, Action ExecuteAction)
        {
            base.ExecutionPreStart(mono, pgiHeader, ExecuteAction);
            scheduler = PGScheduler.ScheduleFrames(mono, delayFrames, ExecuteAction);
        }
        

    }
}