// ----------------------------------------------------
// Copyright (c) Pampel Games e.K. All Rights Reserved.
// https://www.pampelgames.com
// ----------------------------------------------------

using UnityEngine;
using UnityEngine.UIElements;

namespace PampelGames.Shared.Editor.EditorTools
{
    public static class PGEditorTweenVisualElementExtensions
    {
        
        public static void PGEditorTweenColor(this VisualElement element, Color color, float duration)
        {
            if (!PGEditorTweenManager.InitializeTweenedObject(element)) return;
            
            var originalColor = element.style.backgroundColor.value;

            var tween = PGEditorTween.Move(originalColor, color, duration);
            tween.OnUpdate(() =>
            {
                element.style.backgroundColor = new StyleColor((Color) tween.currentValue);
            });
            tween.OnComplete(() =>
            {
                var tweenBack = PGEditorTween.Move(element.style.backgroundColor.value, originalColor, 1f);
                tweenBack.OnUpdate(() =>
                {
                    element.style.backgroundColor = new StyleColor((Color) tweenBack.currentValue);
                    tweenBack.OnComplete(() => PGEditorTweenManager.RemoveTweenedObject(element));
                });
            });
        }
    }
}