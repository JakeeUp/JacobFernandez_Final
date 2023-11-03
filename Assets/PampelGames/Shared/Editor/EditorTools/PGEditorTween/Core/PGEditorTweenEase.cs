// ----------------------------------------------------
// Copyright (c) Pampel Games e.K. All Rights Reserved.
// https://www.pampelgames.com
// ----------------------------------------------------

using System;
using UnityEngine;

namespace PampelGames.Shared.Editor.EditorTools
{
    /// <summary>
    ///     Handles Tween ease methods.
    /// </summary>
    public static class PGEditorTweenEase
    {
        public enum Ease
        {
            Linear,
            AnimationCurve,
            InSine,
            OutSine,
            InOutSine,
            InQuad,
            OutQuad,
            InOutQuad,
            InCubic,
            OutCubic,
            InOutCubic,
            InQuart,
            OutQuart,
            InOutQuart,
            InQuint,
            OutQuint,
            InOutQuint,
            InExpo,
            OutExpo,
            InOutExpo,
            InCirc,
            OutCirc,
            InOutCirc,
            InBack,
            OutBack,
            InOutBack,
            InElastic,
            OutElastic,
            InOutElastic,
            InBounce,
            OutBounce,
            InOutBounce
        }

        /// <summary>
        ///     Methods from https://easings.net/.
        /// </summary>
        public delegate float EaseMethod(float currentTime, float duration, float amplitude, AnimationCurve animationCurve);

        public static EaseMethod GetEaseMethod(Ease easeType)
        {
            switch (easeType)
            {
                case Ease.Linear:
                    return EaseLinear;
                case Ease.AnimationCurve:
                    return EaseAnimationCurve;
                case Ease.InSine:
                    return EaseInSine;
                case Ease.OutSine:
                    return EaseOutSine;
                case Ease.InOutSine:
                    return EaseInOutSine;
                case Ease.InQuad:
                    return EaseInQuad;
                case Ease.OutQuad:
                    return EaseOutQuad;
                case Ease.InOutQuad:
                    return EaseInOutQuad;
                case Ease.InCubic:
                    return EaseInCubic;
                case Ease.OutCubic:
                    return EaseOutCubic;
                case Ease.InOutCubic:
                    return EaseInOutCubic;
                case Ease.InQuart:
                    return EaseInQuart;
                case Ease.OutQuart:
                    return EaseOutQuart;
                case Ease.InOutQuart:
                    return EaseInOutQuart;
                case Ease.InQuint:
                    return EaseInQuint;
                case Ease.OutQuint:
                    return EaseOutQuint;
                case Ease.InOutQuint:
                    return EaseInOutQuint;
                case Ease.InExpo:
                    return EaseInExpo;
                case Ease.OutExpo:
                    return EaseOutExpo;
                case Ease.InOutExpo:
                    return EaseInOutExpo;
                case Ease.InCirc:
                    return EaseInCirc;
                case Ease.OutCirc:
                    return EaseOutCirc;
                case Ease.InOutCirc:
                    return EaseInOutCirc;
                case Ease.InBack:
                    return EaseInBack;
                case Ease.OutBack:
                    return EaseOutBack;
                case Ease.InOutBack:
                    return EaseInOutBack;
                case Ease.InElastic:
                    return EaseInElastic;
                case Ease.OutElastic:
                    return EaseOutElastic;
                case Ease.InOutElastic:
                    return EaseInOutElastic;
                case Ease.InBounce:
                    return EaseInBounce;
                case Ease.OutBounce:
                    return EaseOutBounce;
                case Ease.InOutBounce:
                    return EaseInOutBounce;

                default:
                    return EaseLinear;
            }
        }

        private static float EaseLinear(float currentTime, float duration, float amplitude, AnimationCurve animationCurve)
        {
            return currentTime / duration;
        }

        private static float EaseAnimationCurve(float currentTime, float duration, float amplitude, AnimationCurve animationCurve)
        {
            var x = currentTime / duration;
            var normalizedLength = animationCurve[animationCurve.length - 1].time;
            return animationCurve.Evaluate(x * normalizedLength);
        }

        private static float EaseInSine(float currentTime, float duration, float amplitude, AnimationCurve animationCurve)
        {
            var x = currentTime / duration;
            return 1 - Mathf.Cos(x * Mathf.PI * 0.5f);
        }

        private static float EaseOutSine(float currentTime, float duration, float amplitude, AnimationCurve animationCurve)
        {
            var x = currentTime / duration;
            return Mathf.Sin(x * Mathf.PI * 0.5f);
        }

        private static float EaseInOutSine(float currentTime, float duration, float amplitude, AnimationCurve animationCurve)
        {
            var x = currentTime / duration;

            return -(Mathf.Cos(Mathf.PI * x) - 1) * 0.5f;
        }

        private static float EaseInQuad(float currentTime, float duration, float amplitude, AnimationCurve animationCurve)
        {
            var x = currentTime / duration;
            return x * x;
        }

        private static float EaseOutQuad(float currentTime, float duration, float amplitude, AnimationCurve animationCurve)
        {
            var x = currentTime / duration;
            return 1 - (1 - x) * (1 - x);
        }

        private static float EaseInOutQuad(float currentTime, float duration, float amplitude, AnimationCurve animationCurve)
        {
            var x = currentTime / duration;
            return x < 0.5f ? 2 * x * x : 1 - Mathf.Pow(-2 * x + 2, 2) * 0.5f;
        }

        private static float EaseInCubic(float currentTime, float duration, float amplitude, AnimationCurve animationCurve)
        {
            var x = currentTime / duration;
            return x * x * x;
        }

        private static float EaseOutCubic(float currentTime, float duration, float amplitude, AnimationCurve animationCurve)
        {
            var x = currentTime / duration;
            return 1 - Mathf.Pow(1 - x, 3);
        }

        private static float EaseInOutCubic(float currentTime, float duration, float amplitude, AnimationCurve animationCurve)
        {
            var x = currentTime / duration;
            return x < 0.5 ? 4 * x * x * x : 1 - Mathf.Pow(-2 * x + 2, 3) * 0.5f;
        }

        private static float EaseInQuart(float currentTime, float duration, float amplitude, AnimationCurve animationCurve)
        {
            var x = currentTime / duration;
            return x * x * x * x;
        }

        private static float EaseOutQuart(float currentTime, float duration, float amplitude, AnimationCurve animationCurve)
        {
            var x = currentTime / duration;
            return 1 - Mathf.Pow(1 - x, 4);
        }

        private static float EaseInOutQuart(float currentTime, float duration, float amplitude, AnimationCurve animationCurve)
        {
            var x = currentTime / duration;
            return x < 0.5 ? 8 * x * x * x * x : 1 - Mathf.Pow(-2 * x + 2, 4) * 0.5f;
        }

        private static float EaseInQuint(float currentTime, float duration, float amplitude, AnimationCurve animationCurve)
        {
            var x = currentTime / duration;
            return x * x * x * x * x;
        }

        private static float EaseOutQuint(float currentTime, float duration, float amplitude, AnimationCurve animationCurve)
        {
            var x = currentTime / duration;
            return 1 - Mathf.Pow(1 - x, 5);
        }

        private static float EaseInOutQuint(float currentTime, float duration, float amplitude, AnimationCurve animationCurve)
        {
            var x = currentTime / duration;
            return x < 0.5 ? 16 * x * x * x * x * x : 1 - Mathf.Pow(-2 * x + 2, 5) * 0.5f;
        }

        private static float EaseInExpo(float currentTime, float duration, float amplitude, AnimationCurve animationCurve)
        {
            var x = currentTime / duration;
            return Mathf.Pow(2, 10 * x - 10);
        }

        private static float EaseOutExpo(float currentTime, float duration, float amplitude, AnimationCurve animationCurve)
        {
            var x = currentTime / duration;
            return 1 - Mathf.Pow(2, -10 * x);
        }

        private static float EaseInOutExpo(float currentTime, float duration, float amplitude, AnimationCurve animationCurve)
        {
            var x = currentTime / duration;
            return x < 0.5
                ? Mathf.Pow(2, 20 * x - 10) * 0.5f
                : (2 - Mathf.Pow(2, -20 * x + 10)) * 0.5f;
        }

        private static float EaseInCirc(float currentTime, float duration, float amplitude, AnimationCurve animationCurve)
        {
            var x = currentTime / duration;
            return 1 - Mathf.Sqrt(1 - Mathf.Pow(x, 2));
        }

        private static float EaseOutCirc(float currentTime, float duration, float amplitude, AnimationCurve animationCurve)
        {
            var x = currentTime / duration;
            return Mathf.Sqrt(1 - Mathf.Pow(x - 1, 2));
        }

        private static float EaseInOutCirc(float currentTime, float duration, float amplitude, AnimationCurve animationCurve)
        {
            var x = currentTime / duration;
            return x < 0.5
                ? (1 - Mathf.Sqrt(1 - Mathf.Pow(2 * x, 2))) * 0.5f
                : (Mathf.Sqrt(1 - Mathf.Pow(-2 * x + 2, 2)) + 1) * 0.5f;
        }

        private static float EaseInBack(float currentTime, float duration, float amplitude, AnimationCurve animationCurve)
        {
            var x = currentTime / duration;
            var c1 = amplitude;
            var c3 = c1 + 1;
            return c3 * x * x * x - c1 * x * x;
        }

        private static float EaseOutBack(float currentTime, float duration, float amplitude, AnimationCurve animationCurve)
        {
            var x = currentTime / duration;
            var c1 = amplitude;
            var c3 = c1 + 1;
            return 1 + c3 * Mathf.Pow(x - 1, 3) + c1 * Mathf.Pow(x - 1, 2);
        }

        private static float EaseInOutBack(float currentTime, float duration, float amplitude, AnimationCurve animationCurve)
        {
            var x = currentTime / duration;
            var c1 = amplitude;
            var c2 = c1 * 1.525f;

            return x < 0.5
                ? Mathf.Pow(2 * x, 2) * ((c2 + 1) * 2 * x - c2) * 0.5f
                : (Mathf.Pow(2 * x - 2, 2) * ((c2 + 1) * (x * 2 - 2) + c2) + 2) * 0.5f;
        }

        private static float EaseInElastic(float currentTime, float duration, float amplitude, AnimationCurve animationCurve)
        {
            var x = currentTime / duration;
            var c4 = (float) (amplitude * Math.PI / 3f);
            return -Mathf.Pow(2, 10 * x - 10) * Mathf.Sin((float) ((x * 10 - 10.75) * c4));
        }

        private static float EaseOutElastic(float currentTime, float duration, float amplitude, AnimationCurve animationCurve)
        {
            var x = currentTime / duration;
            var c4 = (float) (amplitude * Math.PI / 3);
            return Mathf.Pow(2, -10 * x) * Mathf.Sin((float) ((x * 10 - 0.75) * c4)) + 1;
        }

        private static float EaseInOutElastic(float currentTime, float duration, float amplitude, AnimationCurve animationCurve)
        {
            var x = currentTime / duration;
            var c5 = (float) (amplitude * Math.PI / 4.5f);
            return x < 0.5
                ? -(Mathf.Pow(2, 20 * x - 10) * Mathf.Sin((float) ((20 * x - 11.125) * c5))) * 0.5f
                : Mathf.Pow(2, -20 * x + 10) * Mathf.Sin((float) ((20 * x - 11.125) * c5)) * 0.5f + 1;
        }

        private static float EaseInBounce(float currentTime, float duration, float amplitude, AnimationCurve animationCurve)
        {
            var x = currentTime / duration;
            return 1f - EaseOutBounceOriginal(1f - x);
        }

        private static float EaseOutBounce(float currentTime, float duration, float amplitude, AnimationCurve animationCurve)
        {
            var x = currentTime / duration;
            return EaseOutBounceOriginal(x);
        }

        private static float EaseInOutBounce(float currentTime, float duration, float amplitude, AnimationCurve animationCurve)
        {
            var x = currentTime / duration;
            return x < 0.5
                ? (1 - EaseOutBounceOriginal(1 - 2 * x)) * 0.5f
                : (1 + EaseOutBounceOriginal(2 * x - 1)) * 0.5f;
        }

        private static float EaseOutBounceOriginal(float x)
        {
            var n1 = 7.5625f;
            var d1 = 2.75f;
            if (x < 1 / d1)
                return n1 * x * x;
            if (x < 2 / d1)
                return (float) (n1 * (x -= 1.5f / d1) * x + 0.75);
            if (x < 2.5 / d1)
                return (float) (n1 * (x -= 2.25f / d1) * x + 0.9375);
            return (float) (n1 * (x -= 2.625f / d1) * x + 0.984375);
        }
    }
}