// ----------------------------------------------------
// Gore Simulator
// Copyright (c) Pampel Games e.K. All Rights Reserved.
// https://www.pampelgames.com
// ----------------------------------------------------

using System;
using System.Collections.Generic;
using UnityEngine;

namespace PampelGames.GoreSimulator
{
    [Serializable]
    public class ChunkClass
    {
        public string boneName;
        public int cutIndexClassIndex;
        
        public Mesh mesh;

        // OrigToNewMap - used to find the precached vertices at runtime.
        public List<int> keys;
        public List<int> values;

        /// <summary>
        ///     Cut and Sew indexes of the new mesh.
        /// </summary>
        public List<ExplosionIndexClass> indexClasses;

        /********************************************************************************************************************************/
        
        internal List<Vector3> cutCenters = new();

    }
}