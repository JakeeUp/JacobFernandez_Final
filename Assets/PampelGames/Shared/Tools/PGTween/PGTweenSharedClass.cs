// ----------------------------------------------------
// Copyright (c) Pampel Games e.K. All Rights Reserved.
// https://www.pampelgames.com
// ----------------------------------------------------

using System;
using UnityEngine;

namespace PampelGames.Shared.Tools
{
    [Serializable]
    public class PGTweenSharedClass
    {

        public PGTweenEase.Ease ease = PGTweenEase.Ease.AnimationCurve;

        public AnimationCurve animationCurve = AnimationCurve.Linear(0,0,1,1);
        
        public float amplitude = 1.70158f;
        
        
        /* Curve Creator *******************************************************************************************************************************/
        
        [Tooltip("Number of waves.")]
        public int shakeFrequency = 6;

        [Tooltip("Maximum amount the waves can move up.")]
        public float maxPositive = 1f;
        [Tooltip("Maximum amount the waves can move down.")]
        public float maxNegative = -1f;

        [Tooltip("Multiplier applied after each wave.")]
        public float multiplier = 0.5f;

        [Tooltip("Flip the curve horizontally.")]
        public bool flip;
    }
    
}
