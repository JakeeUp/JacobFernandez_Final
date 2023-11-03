// ----------------------------------------------------
// Copyright (c) Pampel Games e.K. All Rights Reserved.
// https://www.pampelgames.com
// ----------------------------------------------------

using System;
using UnityEngine;

namespace PampelGames.Shared.Editor.EditorTools
{
    /// <summary>
    ///     Extension methods for <see cref="PGEditorTweenDescr" />.
    /// </summary>
    public static class PGEditorTweenExtensions
    {
        public static void Pause(this PGEditorTweenDescr tween)
        {
            tween.active = false;
            tween.internalEvents.onPause?.Invoke();
        }

        public static void Resume(this PGEditorTweenDescr tween)
        {
            if (!tween.completed)
            {
                tween.active = true;
            }
            tween.internalEvents.onResume?.Invoke();
        }

        public static void Kill(this PGEditorTweenDescr tween)
        {
            tween.internalEvents.onKill?.Invoke();
            tween.completed = true;
        }

        /* Callbacks *******************************************************************************************************************************/
        
        public static void OnUpdate(this PGEditorTweenDescr tween, Action action)
        {
            tween.internalEvents.onUpdate = action;
        }

        public static void OnComplete(this PGEditorTweenDescr tween, Action action)
        {
            tween.internalEvents.onComplete = action;
        }

        public static void OnPause(this PGEditorTweenDescr tween, Action action)
        {
            tween.internalEvents.onPause = action;
        }

        public static void OnResume(this PGEditorTweenDescr tween, Action action)
        {
            tween.internalEvents.onResume = action;
        }

        public static void OnKill(this PGEditorTweenDescr tween, Action action)
        {
            tween.internalEvents.onKill = action;
        }

        /* Ease *******************************************************************************************************************************/
        public static void SetEase(this PGEditorTweenDescr tween, PGEditorTweenEase.Ease ease)
        {
            tween.easeMethod = PGEditorTweenEase.GetEaseMethod(ease);
        }

        public static void SetEase(this PGEditorTweenDescr tween, PGEditorTweenEase.Ease ease, float amplitude)
        {
            tween.easeMethod = PGEditorTweenEase.GetEaseMethod(ease);
            tween.amplitude = amplitude;
        }

        public static void SetEase(this PGEditorTweenDescr tween, AnimationCurve animCurve)
        {
            tween.easeMethod = PGEditorTweenEase.GetEaseMethod(PGEditorTweenEase.Ease.AnimationCurve);
            tween.animationCurve = animCurve;
        }
    }
}