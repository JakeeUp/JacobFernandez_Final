// ---------------------------------------------------
// Copyright (c) Pampel Games e.K. All Rights Reserved.
// https://www.pampelgames.com
// ---------------------------------------------------

using System;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

namespace PampelGames.Shared.Utility
{
    /// <summary>
    ///     Math helpers.
    /// </summary>
    public static class PGMathUtility
    {
        
        public static Vector3 GetAxis(PGEnums.AxisEnum axis)
        {
            return m_AlignAxisToVector[(int) axis];
        }
        
        private static readonly Vector3[] m_AlignAxisToVector =
        {
            Vector3.right, Vector3.up, Vector3.forward, Vector3.left, Vector3.down, Vector3.back
        };
        
        
        
        
        /// <summary>
        ///     Generates a unique integer value.
        /// </summary>
        public static int UniqueGUID()
        {
            return Guid.NewGuid().GetHashCode();
        }

        /// <summary>
        ///     Gets a random list or array entry using instances weights.
        /// </summary>
        /// <param name="arrayLenght">Lenght of list or array.</param>
        /// <param name="instancesWeights">Weights of the array in the same order. If entry does not exist, will be set to 1.</param>
        public static int GetRandomArrayEntry(int arrayLenght, List<float> instancesWeights)
        {
            return GetRandomArrayEntry(arrayLenght, instancesWeights.ToArray());
        }

        public static int GetRandomArrayEntry(int arrayLenght, float[] instancesWeights)
        {
            return GetRandomArrayEntryInternal(arrayLenght, instancesWeights);
        }

        public static Quaternion GetRandomRotation()
        {
            var xRotation = Random.Range(0.0f, 360.0f);
            var yRotation = Random.Range(0.0f, 360.0f);
            var zRotation = Random.Range(0.0f, 360.0f);
            return Quaternion.Euler(xRotation, yRotation, zRotation);
        }

        /********************************************************************************************************************************/
        /********************************************************************************************************************************/
        

        private static int GetRandomArrayEntryInternal(int arrayLenght, float[] instancesWeights)
        {
            var currentWeight = 0f;
            var totalWeight = 0f;
            for (var i = 0; i < arrayLenght; i++)
                totalWeight += i < instancesWeights.Length ? instancesWeights[i] : 1;

            var randomWeight = Random.Range(0f, totalWeight);

            for (var i = 0; i < arrayLenght; i++)
            {
                currentWeight += i < instancesWeights.Length ? instancesWeights[i] : 1;
                if (randomWeight <= currentWeight) return i;
            }

            return 0;
        }
    }
}