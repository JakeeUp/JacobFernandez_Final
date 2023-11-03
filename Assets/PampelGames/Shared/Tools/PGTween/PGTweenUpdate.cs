// ----------------------------------------------------
// Copyright (c) Pampel Games e.K. All Rights Reserved.
// https://www.pampelgames.com
// ----------------------------------------------------

using System.Collections;
using UnityEngine;

namespace PampelGames.Shared.Tools
{
    /// <summary>
    ///     Updates <see cref="PGTweenDescr" />s.
    /// </summary>
    internal static class PGTweenUpdate
    {
        
        internal static IEnumerator _TweenUpdate(PGTweenDescr tween)
        {
            for (;;)
            {

                if (tween.stopped) yield break;

                if (!tween.active)
                {
                    yield return null;
                    continue;
                }
                if (tween.isFrameTween)
                {
                    tween.currentTime += 1;
                    if (tween.currentTime > tween.duration)
                    {
                        tween.currentTime = tween.duration;
                        tween.completed = true;
                    }   
                }
                else
                {
                    tween.currentTime += Time.deltaTime;
                    if (tween.currentTime >= tween.duration)
                    {
                        tween.currentTime = tween.duration;
                        tween.completed = true;
                    }    
                }

                if (tween.active)
                    tween.active = !tween.completed;

                tween.SetValue();
                tween.internalEvents.onUpdate?.Invoke();

                if (!tween.completed) yield return null;
                else
                {
                    tween.internalEvents.onComplete?.Invoke();
                    yield break;
                }
            }
        }
    }
}