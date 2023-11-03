// ----------------------------------------------------
// Copyright (c) Pampel Games e.K. All Rights Reserved.
// https://www.pampelgames.com
// ----------------------------------------------------

using UnityEngine;

namespace PampelGames.Shared.Tools
{
    /// <summary>
    ///     Sets the current value of a <see cref="PGTweenDescr" /> for all available types.
    /// </summary>
    public static class PGTweenSetValue
    {
        public static void SetFloat(PGTweenDescr tween, float currentTime, object startValue, object changeValue, float duration)
        {
            var newValue = (float) startValue;
            var changeValueFloat = (float) changeValue;
            var easeValue = tween.easeMethod(currentTime, duration, tween.amplitude, tween.animationCurve);
            tween.currentValue = newValue + changeValueFloat * easeValue;
        }

        public static void SetVector2(PGTweenDescr tween, float currentTime, object startValue, object changeValue, float duration)
        {
            var newValue = (Vector2) startValue;
            var changeValueVector2 = (Vector2) changeValue;
            var easeValue = tween.easeMethod(currentTime, duration, tween.amplitude, tween.animationCurve);
            newValue.x += changeValueVector2.x * easeValue;
            newValue.y += changeValueVector2.y * easeValue;
            tween.currentValue = newValue;
        }

        public static void SetVector3(PGTweenDescr tween, float currentTime, object startValue, object changeValue, float duration)
        {
            var newValue = (Vector3) startValue;
            var changeValueVector3 = (Vector3) changeValue;
            var easeValue = tween.easeMethod(currentTime, duration, tween.amplitude, tween.animationCurve);
            newValue.x += changeValueVector3.x * easeValue;
            newValue.y += changeValueVector3.y * easeValue;
            newValue.z += changeValueVector3.z * easeValue;
            tween.currentValue = newValue;
        }

        public static void SetVector4(PGTweenDescr tween, float currentTime, object startValue, object changeValue, float duration)
        {
            var newValue = (Vector4) startValue;
            var changeValueVector4 = (Vector4) changeValue;
            var easeValue = tween.easeMethod(currentTime, duration, tween.amplitude, tween.animationCurve);
            newValue.x += changeValueVector4.x * easeValue;
            newValue.y += changeValueVector4.y * easeValue;
            newValue.z += changeValueVector4.z * easeValue;
            newValue.w += changeValueVector4.w * easeValue;
            tween.currentValue = newValue;
        }

        public static void SetColor(PGTweenDescr tween, float currentTime, object startValue, object changeValue, float duration)
        {
            var newValue = (Color) startValue;
            var changeValueColor = (Color) changeValue;
            var easeValue = tween.easeMethod(currentTime, duration, tween.amplitude, tween.animationCurve);
            newValue.r += changeValueColor.r * easeValue;
            newValue.g += changeValueColor.g * easeValue;
            newValue.b += changeValueColor.b * easeValue;
            newValue.a += changeValueColor.a * easeValue;
            tween.currentValue = newValue;
        }
    }
}