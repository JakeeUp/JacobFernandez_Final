// ----------------------------------------------------
// Copyright (c) Pampel Games e.K. All Rights Reserved.
// https://www.pampelgames.com
// ----------------------------------------------------

using Unity.Collections;
using Unity.Mathematics;

namespace PampelGames.Shared.Utility.PGMathematics
{
    /// <summary>
    ///     Utility class for Unity.Mathematics math functions.
    /// </summary>
    public static class PGMathematicsUtility
    {

        public static float3 GetAxis(PGEnums.AxisEnum axis)
        {
            return m_AlignAxisToVector[(int) axis];
        }
        
        private static readonly float3[] m_AlignAxisToVector =
        {
            math.right(), math.up(), math.forward(), math.left(), math.down(), math.back()
        };
        
        
        /// <summary>
        ///     Check for error *Quaternion To Matrix conversion failed because input Quaternion is invalid {0,0,0,0}*
        /// </summary>
        public static bool ValidQuaternionRotation(quaternion rotation)
        {
            return rotation.value.x != 0 || rotation.value.y != 0 || rotation.value.z != 0 || rotation.value.w != 0;
        }
        
        /// <summary>
        ///     Fills an IndexArray with indexes from the InstancesWeightsArray.
        /// </summary>
        /// <param name="indexArray">Existing array with length of how many objects are needed. Filled with the object indexes.</param>
        /// <param name="instancesWeightsArray">Existing array with the length of the total object count. Weights are usually from 0 to 1.</param>
        /// <param name="randomConst">Existing Mathematics.Random constant. Can be created like this: var randomConst = new Random((uint) seed);</param>
        public static void FillRandomArray(NativeArray<int> indexArray, NativeArray<float> instancesWeightsArray, Random randomConst)
        {
            for (var i = 0; i < indexArray.Length; i++)
            {
                var currentWeight = 0f;
                var totalWeight = 0f;
                for (var j = 0; j < instancesWeightsArray.Length; j++) totalWeight += instancesWeightsArray[j];
                var randomWeight = randomConst.NextFloat(0f, totalWeight);
                for (var j = 0; j < instancesWeightsArray.Length; j++)
                {
                    currentWeight += instancesWeightsArray[j];
                    if (!(randomWeight <= currentWeight)) continue;
                    indexArray[i] = j;
                    break;
                }
            }
        }
        

    }
}