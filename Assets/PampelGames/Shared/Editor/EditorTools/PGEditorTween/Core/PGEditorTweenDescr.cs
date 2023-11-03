// ----------------------------------------------------
// Copyright (c) Pampel Games e.K. All Rights Reserved.
// https://www.pampelgames.com
// ----------------------------------------------------

using System;
using UnityEngine;

namespace PampelGames.Shared.Editor.EditorTools
{
    /// <summary>
    ///     Tween object that is being animated.
    /// </summary>
    public class PGEditorTweenDescr
    {
        internal event Action<PGEditorTweenDescr, float, object, object, float> SetValueAction;

        internal readonly InternalEvents internalEvents = new();

        internal class InternalEvents
        {
            public Action onUpdate;
            public Action onComplete;
            public Action onPause;
            public Action onResume;
            public Action onKill;
        }

        internal object obj;
        internal float duration;
        internal float amplitude = 1.70158f;
        internal AnimationCurve animationCurve;
        internal float delay;
        internal float currentTime;
        internal bool active;
        internal bool completed;
        public object startValue;
        public object endValue;
        public object differenceValue;
        public object currentValue;

        internal PGEditorTweenEase.EaseMethod easeMethod;

        public void SetValue()
        {
            SetValueAction(this, currentTime, startValue, differenceValue, duration);
        }

        public void Reset()
        {
            SetValueAction = delegate { };
            internalEvents.onUpdate = null;
            internalEvents.onComplete = null;
            internalEvents.onPause = null;
            internalEvents.onResume = null;
            internalEvents.onKill = null;
            duration = 0;
            amplitude = 1.70158f;
            animationCurve = null;
            currentTime = 0;
            active = false;
            completed = false;
            easeMethod = null;
            startValue = null;
            endValue = null;
            differenceValue = null;
            currentValue = null;
        }
    }
}