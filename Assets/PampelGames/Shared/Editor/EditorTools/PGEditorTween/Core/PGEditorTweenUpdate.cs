// ----------------------------------------------------
// Copyright (c) Pampel Games e.K. All Rights Reserved.
// https://www.pampelgames.com
// ----------------------------------------------------

using System.Collections;
using UnityEngine;

namespace PampelGames.Shared.Editor.EditorTools
{
    /// <summary>
    ///     Updates <see cref="PGEditorTweenDescr" />s.
    /// </summary>
    internal static class PGEditorTweenUpdate
    {
        
        internal static IEnumerator _TweenUpdate(PGEditorTweenDescr tween)
        {
            float timeStarted = Time.realtimeSinceStartup;
            
            for (;;)
            {
                if (!tween.active)
                {
                    yield return null;
                    continue;
                }

                tween.currentTime = Time.realtimeSinceStartup - timeStarted;
                
                if (tween.delay > 0)
                {
                    if (tween.currentTime >= tween.delay)
                    {
                        tween.delay = -1;
                        timeStarted = Time.realtimeSinceStartup;
                    }

                    yield return null;  
                    continue;
                }

                if (tween.currentTime >= tween.duration)
                {
                    tween.currentTime = tween.duration;
                    tween.completed = true;
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