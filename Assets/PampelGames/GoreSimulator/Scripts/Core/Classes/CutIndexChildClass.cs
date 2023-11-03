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
    
    /// <summary>
    ///     Indexes on the child side split per hole.
    /// </summary>
    [Serializable]
    public class CutIndexChildClass
    {

        public string childBoneName;
        
        /// <summary>
        ///     Edge loop cut indexes in the direction of the child bone.
        /// </summary>
        public List<int> cutIndexes = new();

        /// <summary>
        ///     Indexes used to sew the <see cref="cutIndexes"/>.
        /// </summary>
        public List<int> sewIndexes = new();
        
        /// <summary>
        /// Triangles for the <see cref="sewIndexes"/>.
        /// </summary>
        public List<int> sewTriangles;
        
    }
}