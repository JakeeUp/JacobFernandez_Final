// ----------------------------------------------------
// Gore Simulator
// Copyright (c) Pampel Games e.K. All Rights Reserved.
// https://www.pampelgames.com
// ----------------------------------------------------

using System.Collections.Generic;
using UnityEngine;

namespace PampelGames.GoreSimulator
{
    /// <summary>
    ///     Only used by the center bone.
    /// </summary>
    internal class UsedBonesClass
    {

        public Transform usedBone;
        
        public int usedCutIndex;

    }

    internal static class UsedBonesClassExtensions
    {
        public static void AddItems(this UsedBonesClass usedBonesClass, Transform usedBone, int usedCutIndex)
        {
            usedBonesClass.usedBone = usedBone;
            usedBonesClass.usedCutIndex = usedCutIndex;
        }
    }
}
