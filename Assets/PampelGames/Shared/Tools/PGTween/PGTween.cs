// ---------------------------------------------------
// Copyright (c) Pampel Games e.K. All Rights Reserved.
// https://www.pampelgames.com
// ---------------------------------------------------


using UnityEngine;

namespace PampelGames.Shared.Tools
{
    /// <summary>
    ///     Creates new <see cref="PGTweenDescr" />s and animates the currentValue using the specified type.
    /// </summary>
    public static class PGTween
    {
        
        public static PGTweenDescr Move(MonoBehaviour mono, float startValue, float endValue, float duration)
        {
            return PGTweenSetup.SetupTween(mono, startValue, endValue, duration, false);
        }
        
        public static PGTweenDescr MoveFrames(MonoBehaviour mono, float startValue, float endValue, int frames)
        {
            return PGTweenSetup.SetupTween(mono, startValue, endValue, frames, true);
        }

        public static PGTweenDescr Move(MonoBehaviour mono, Vector2 startValue, Vector2 endValue, float duration)
        {
            return PGTweenSetup.SetupTween(mono, startValue, endValue, duration, false);
        }
        public static PGTweenDescr MoveFrames(MonoBehaviour mono, Vector2 startValue, Vector2 endValue, int frames)
        {
            return PGTweenSetup.SetupTween(mono, startValue, endValue, frames, true);
        }

        public static PGTweenDescr Move(MonoBehaviour mono, Vector3 startValue, Vector3 endValue, float duration)
        {
            return PGTweenSetup.SetupTween(mono, startValue, endValue, duration, false);
        }
        public static PGTweenDescr MoveFrames(MonoBehaviour mono, Vector3 startValue, Vector3 endValue, int frames)
        {
            return PGTweenSetup.SetupTween(mono, startValue, endValue, frames, true);
        }

        public static PGTweenDescr Move(MonoBehaviour mono, Vector4 startValue, Vector4 endValue, float duration)
        {
            return PGTweenSetup.SetupTween(mono, startValue, endValue, duration, false);
        }
        public static PGTweenDescr MoveFrames(MonoBehaviour mono, Vector4 startValue, Vector4 endValue, int frames)
        {
            return PGTweenSetup.SetupTween(mono, startValue, endValue, frames, true);
        }

        public static PGTweenDescr Move(MonoBehaviour mono, Color startValue, Color endValue, float duration)
        {
            return PGTweenSetup.SetupTween(mono, startValue, endValue, duration, false);
        }
        public static PGTweenDescr MoveFrames(MonoBehaviour mono, Color startValue, Color endValue, int frames)
        {
            return PGTweenSetup.SetupTween(mono, startValue, endValue, frames, true);
        }
    }
}