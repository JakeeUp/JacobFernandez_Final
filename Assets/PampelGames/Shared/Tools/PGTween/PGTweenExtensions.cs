// ----------------------------------------------------
// Copyright (c) Pampel Games e.K. All Rights Reserved.
// https://www.pampelgames.com
// ----------------------------------------------------

using System;
using UnityEngine;

namespace PampelGames.Shared.Tools
{
    /// <summary>
    ///     Extension methods for <see cref="PGTweenDescr" />.
    /// </summary>
    public static class PGTweenExtensions
    {
        public static void Pause(this PGTweenDescr tween)
        {
            if (tween == null) return;
            tween.active = false;
            tween.internalEvents.onPause?.Invoke();
        }

        public static void Resume(this PGTweenDescr tween)
        {
            if (tween == null) return;
            if (!tween.completed) tween.active = true;
            tween.internalEvents.onResume?.Invoke();
        }

        public static void Kill(this PGTweenDescr tween)
        {
            if (tween == null) return;
            tween.stopped = true;
            tween.internalEvents.onKill?.Invoke();
        }

        /* Callbacks *******************************************************************************************************************************/
        
        public static void OnUpdate(this PGTweenDescr tween, Action action)
        {
            tween.internalEvents.onUpdate = action;
        }

        public static void OnComplete(this PGTweenDescr tween, Action action)
        {
            tween.internalEvents.onComplete = action;
        }

        public static void OnPause(this PGTweenDescr tween, Action action)
        {
            tween.internalEvents.onPause = action;
        }

        public static void OnResume(this PGTweenDescr tween, Action action)
        {
            tween.internalEvents.onResume = action;
        }

        public static void OnKill(this PGTweenDescr tween, Action action)
        {
            tween.internalEvents.onKill = action;
        }

        /* Ease *******************************************************************************************************************************/
        public static void SetEase(this PGTweenDescr tween, PGTweenEase.Ease ease)
        {
            tween.easeMethod = PGTweenEase.GetEaseMethod(ease);
        }

        public static void SetEase(this PGTweenDescr tween, PGTweenEase.Ease ease, float amplitude)
        {
            tween.easeMethod = PGTweenEase.GetEaseMethod(ease);
            tween.amplitude = amplitude;
        }

        public static void SetEase(this PGTweenDescr tween, AnimationCurve animCurve)
        {
            tween.easeMethod = PGTweenEase.GetEaseMethod(PGTweenEase.Ease.AnimationCurve);
            tween.animationCurve = animCurve;
        }
    }
}