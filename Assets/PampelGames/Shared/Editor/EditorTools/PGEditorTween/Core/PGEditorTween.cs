// ---------------------------------------------------
// Copyright (c) Pampel Games e.K. All Rights Reserved.
// https://www.pampelgames.com
// ---------------------------------------------------


using UnityEngine;

namespace PampelGames.Shared.Editor.EditorTools
{
    /// <summary>
    ///     Creates new <see cref="PGEditorTweenDescr" />s and animates the currentValue using the specified type.
    /// </summary>
    public static class PGEditorTween
    {
        
        public static PGEditorTweenDescr Move(float startValue, float endValue, float duration, float delay = 0)
        {
            return PGEditorTweenSetup.SetupTween(startValue, endValue, duration, delay);
        }
        
        public static PGEditorTweenDescr Move(Vector2 startValue, Vector2 endValue, float duration, float delay = 0)
        {
            return PGEditorTweenSetup.SetupTween(startValue, endValue, duration, delay);
        }

        public static PGEditorTweenDescr Move(Vector3 startValue, Vector3 endValue, float duration, float delay = 0)
        {
            return PGEditorTweenSetup.SetupTween(startValue, endValue, duration, delay);
        }

        public static PGEditorTweenDescr Move(Vector4 startValue, Vector4 endValue, float duration, float delay = 0)
        {
            return PGEditorTweenSetup.SetupTween(startValue, endValue, duration, delay);
        }

        public static PGEditorTweenDescr Move(Color startValue, Color endValue, float duration, float delay = 0)
        {
            return PGEditorTweenSetup.SetupTween(startValue, endValue, duration, delay);
        }
        
    }
}