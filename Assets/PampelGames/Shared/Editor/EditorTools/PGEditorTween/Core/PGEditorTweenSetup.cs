// ----------------------------------------------------
// Copyright (c) Pampel Games e.K. All Rights Reserved.
// https://www.pampelgames.com
// ----------------------------------------------------

using UnityEngine;

namespace PampelGames.Shared.Editor.EditorTools
{
    /// <summary>
    ///     Creates and sets up a new <see cref="PGEditorTweenDescr" />.
    /// </summary>
    public static class PGEditorTweenSetup 
    {
        internal static PGEditorTweenDescr SetupTween(object startValue, object endValue, float duration, float delay)
        {
            var tween = new PGEditorTweenDescr
            {
                startValue = startValue,
                currentValue = startValue,
                endValue = endValue,
                duration = duration,
                delay = delay
            };
            
            return SetupTweenInternal(tween);
        }

        private static PGEditorTweenDescr SetupTweenInternal(PGEditorTweenDescr tween)
        {
            tween.easeMethod = PGEditorTweenEase.GetEaseMethod(PGEditorTweenEase.Ease.Linear);
            tween.active = true;

            if (tween.startValue is float)
            {
                tween.differenceValue = (float) tween.endValue - (float) tween.startValue;
                tween.SetValueAction += PGEditorTweenSetValue.SetFloat;
            }

            else if (tween.startValue is Vector2)
            {
                tween.differenceValue = (Vector2) tween.endValue - (Vector2) tween.startValue;
                tween.SetValueAction += PGEditorTweenSetValue.SetVector2;
            }

            else if (tween.startValue is Vector3)
            {
                tween.differenceValue = (Vector3) tween.endValue - (Vector3) tween.startValue;
                tween.SetValueAction += PGEditorTweenSetValue.SetVector3;
            }

            else if (tween.startValue is Vector4)
            {
                tween.differenceValue = (Vector4) tween.endValue - (Vector4) tween.startValue;
                tween.SetValueAction += PGEditorTweenSetValue.SetVector4;
            }

            else if (tween.startValue is Color)
            {
                tween.differenceValue = (Color) tween.endValue - (Color) tween.startValue;
                tween.SetValueAction += PGEditorTweenSetValue.SetColor;
            }

            PGEditorCoroutineUtility.StartCoroutine(PGEditorTweenUpdate._TweenUpdate(tween));
            
            return tween;
        }
    }
}