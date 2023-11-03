// ----------------------------------------------------
// Gore Simulator
// Copyright (c) Pampel Games e.K. All Rights Reserved.
// https://www.pampelgames.com
// ----------------------------------------------------

using System.Collections.Generic;
using UnityEngine;

namespace PampelGames.GoreSimulator
{
    [CreateAssetMenu(fileName = "Storage", menuName = "Pampel Games/Gore Simulator/Storage", order = 1)]
    public class SO_Storage : ScriptableObject
    {
        public MeshDataClass meshDataClass;

        public BonesStorageClass centerBonesStorageClass;
        public List<BonesStorageClass> bonesStorageClasses;
    }
}
