// ----------------------------------------------------
// Copyright (c) Pampel Games e.K. All Rights Reserved.
// https://www.pampelgames.com
// ----------------------------------------------------

using System.Collections.Generic;
using UnityEngine;

namespace PampelGames.Shared.Utility
{
    public static class PGAnimationCurvesUtility
    {
        public static readonly AnimationCurve punch = new(new Keyframe(0.0f, 0.0f), new Keyframe(0.112586f, 0.9976035f),
            new Keyframe(0.3120486f, -0.1720615f), new Keyframe(0.4316337f, 0.07030682f), new Keyframe(0.5524869f, -0.03141804f),
            new Keyframe(0.6549395f, 0.003909959f), new Keyframe(0.770987f, -0.009817753f), new Keyframe(0.8838775f, 0.001939224f),
            new Keyframe(1.0f, 0.0f));

        public static readonly AnimationCurve shake = new(new Keyframe(0f, 0f), new Keyframe(0.25f, 1f), new Keyframe(0.75f, -1f),
            new Keyframe(1f, 0f));

        /********************************************************************************************************************************/

        /// <summary>
        ///     Create a new Animation curve with a shaking wave.
        /// </summary>
        /// <param name="frequency">Number of waves.</param>
        /// <param name="maxPositive">Max amount of positive waves (max 1).</param>
        /// <param name="maxNegative">Max amount of negative waves (max -1).</param>
        /// <param name="multiplier">Multiplier applied after each wave.</param>
        /// <param name="flip">Flip the curve horizontally after creating.</param>
        public static AnimationCurve CreateShakeCurve(float maxPositive, float maxNegative, int frequency, float multiplier, bool flip)
        {
            return CreateShakeCurveInternal(maxPositive, maxNegative, frequency, multiplier, flip);
        }
        
        
        /********************************************************************************************************************************/
        /********************************************************************************************************************************/
        
        
        private static AnimationCurve CreateShakeCurveInternal(float maxPositive, float maxNegative, int frequency, float multiplier, bool flip)
        {
            if (frequency < 1) frequency = 1;

            var waveDistance = 1f / frequency;

            var lastPositive = false;
            List<float> newKeyframesTime = new();
            List<float> newKeyframesValue = new();

            for (var i = 0; i < frequency + 1; i++)
            {
                var time = waveDistance * i;

                var value = 0f;

                if (lastPositive)
                    value = Mathf.Min(1, maxPositive);
                else
                    value = Mathf.Max(-1, maxNegative);

                if (i > 1)
                    for (var j = 1; j < i; j++)
                        value *= multiplier;

                if (i == 0 || i == frequency)
                    value = 0;

                newKeyframesTime.Add(time);
                newKeyframesValue.Add(value);

                lastPositive = !lastPositive;
            }

            if (flip) newKeyframesValue.Reverse();

            var newAnimationCurve = new AnimationCurve();
            for (var i = 0; i < newKeyframesTime.Count; i++) newAnimationCurve.AddKey(new Keyframe(newKeyframesTime[i], newKeyframesValue[i]));
            return newAnimationCurve;
        }
    }
}