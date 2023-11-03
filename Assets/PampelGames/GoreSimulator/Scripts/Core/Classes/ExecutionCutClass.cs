// ----------------------------------------------------
// Gore Simulator
// Copyright (c) Pampel Games e.K. All Rights Reserved.
// https://www.pampelgames.com
// ----------------------------------------------------

using System.Collections.Generic;
using UnityEngine;

namespace PampelGames.GoreSimulator
{
    public class ExecutionCutClass
    {

        /// <summary>
        ///     Indexes to seperate from the mesh.
        /// </summary>
        public readonly HashSet<int> newIndexes = new();

        /********************************************************************************************************************************/
        
        /// <summary>
        ///     Edge loop cut indexes, directly on the hole.
        /// </summary>
        public readonly List<List<int>> cutIndexes = new();

        /// <summary>
        ///     Indexes behind the cutIndexes to sew the hole.
        /// </summary>
        public readonly List<List<int>> sewIndexes = new();

        /// <summary>
        ///     Triangles made from the cutIndexes and sewIndexes. Used for new submesh.
        /// </summary>
        public readonly List<List<int>> sewTriangles = new();
    }
}
