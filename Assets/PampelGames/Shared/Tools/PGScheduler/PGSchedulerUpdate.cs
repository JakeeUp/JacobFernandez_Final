// ----------------------------------------------------
// Copyright (c) Pampel Games e.K. All Rights Reserved.
// https://www.pampelgames.com
// ----------------------------------------------------

using System.Collections;
using UnityEngine;

namespace PampelGames.Shared.Tools
{
    /// <summary>
    ///     Updates <see cref="PGSchedulerDescr" />s.
    /// </summary>
    internal static class PGSchedulerUpdate
    {
        
        internal static IEnumerator _SchedulerUpdate(PGSchedulerDescr scheduler)
        {
            for (;;)
            {
                if (scheduler.stopped) yield break;

                if (!scheduler.active)
                {
                    yield return scheduler.yieldInstruction;
                    continue;
                }

                if (scheduler.isFrameScheduler)
                {
                    scheduler.currentTime += 1;
                    if (scheduler.currentTime > scheduler.duration)
                    {
                        scheduler.currentTime = scheduler.duration;
                        scheduler.completed = true;
                    }   
                }
                else
                {
                    scheduler.currentTime += Time.deltaTime;
                    if (scheduler.currentTime >= scheduler.duration)
                    {
                        scheduler.currentTime = scheduler.duration;
                        scheduler.completed = true;
                    }    
                }
                
                if (scheduler.active)
                    scheduler.active = !scheduler.completed;
                
                scheduler.internalEvents.onUpdate?.Invoke();

                if (!scheduler.completed) yield return scheduler.yieldInstruction;
                else
                {
                    scheduler.internalEvents.onComplete?.Invoke();
                    yield break;
                }
            }
        }
        
    }
}