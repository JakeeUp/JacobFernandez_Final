// ----------------------------------------------------
// Copyright (c) Pampel Games e.K. All Rights Reserved.
// https://www.pampelgames.com
// ----------------------------------------------------

using UnityEngine;

namespace PampelGames.Shared.Tools
{
    /// <summary>
    ///     Creates and sets up a new <see cref="PGTweenDescr" />.
    /// </summary>
    public static class PGTweenSetup 
    {
        internal static PGTweenDescr SetupTween(MonoBehaviour mono, object startValue, object endValue, float duration, bool frameTween)
        {
            var tween = new PGTweenDescr
            {
                isFrameTween = frameTween,
                startValue = startValue,
                currentValue = startValue,
                endValue = endValue,
                duration = duration
            };

            return SetupTweenInternal(mono, tween);
        }

        private static PGTweenDescr SetupTweenInternal(MonoBehaviour mono, PGTweenDescr tween)
        {
            tween.easeMethod = PGTweenEase.GetEaseMethod(PGTweenEase.Ease.Linear);
            tween.active = true;

            if (tween.startValue is float)
            {
                tween.differenceValue = (float) tween.endValue - (float) tween.startValue;
                tween.SetValueAction += PGTweenSetValue.SetFloat;
            }

            else if (tween.startValue is Vector2)
            {
                tween.differenceValue = (Vector2) tween.endValue - (Vector2) tween.startValue;
                tween.SetValueAction += PGTweenSetValue.SetVector2;
            }

            else if (tween.startValue is Vector3)
            {
                tween.differenceValue = (Vector3) tween.endValue - (Vector3) tween.startValue;
                tween.SetValueAction += PGTweenSetValue.SetVector3;
            }

            else if (tween.startValue is Vector4)
            {
                tween.differenceValue = (Vector4) tween.endValue - (Vector4) tween.startValue;
                tween.SetValueAction += PGTweenSetValue.SetVector4;
            }

            else if (tween.startValue is Color)
            {
                tween.differenceValue = (Color) tween.endValue - (Color) tween.startValue;
                tween.SetValueAction += PGTweenSetValue.SetColor;
            }
            
            tween.coroutine = mono.StartCoroutine(PGTweenUpdate._TweenUpdate(tween));
            
            return tween;
        }
    }
}