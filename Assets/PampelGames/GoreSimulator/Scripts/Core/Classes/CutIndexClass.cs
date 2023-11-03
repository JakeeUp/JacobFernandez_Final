// ----------------------------------------------------
// Gore Simulator
// Copyright (c) Pampel Games e.K. All Rights Reserved.
// https://www.pampelgames.com
// ----------------------------------------------------

using System;
using System.Collections.Generic;

namespace PampelGames.GoreSimulator
{
    [Serializable]
    public class CutIndexClass
    {
        public int cutIndex;
        
        /// <summary>
        ///     All indexes on the cut side.
        /// </summary>
        public List<int> indexesCutSide;

        /// <summary>
        ///     All indexes on the parent side.
        /// </summary>
        public List<int> indexesParentSide;

        /// <summary>
        ///     Exactly the opposite indexes from the indexesParentSide. May deviate from the indexesCutSide.
        /// </summary>
        public List<int> oppositeParentIndexes;

        /// <summary>
        ///     Indexes of the seperated bone part. Without sewIndexes. Used in explosion.
        /// </summary>
        public List<int> chunkIndexes;

        /// <summary>
        ///     Used to find the center positions of the chunks at runtime.
        /// </summary>
        public List<int> chunkAverageIndexes;


        /// <summary>
        ///     Index of the bone in the smr.bones.
        /// </summary>
        public int boneIndex;
        
        /// <summary>
        ///     Most upper influencing bone indexes (from the bones which bone weights are influencing the bone).
        /// </summary>
        public int detachedParent;
        
        /// <summary>
        ///     Selected, detached bones that have visible mesh.
        /// </summary>
        public List<int> activeBones;
        
        /// <summary>
        ///     Selected, detached bones that have no visible mesh.
        /// </summary>
        public List<int> nonActiveBones;
        
        /// <summary>
        ///     Children indexes of the <see cref="detachedParent"/> that can be removed.
        /// </summary>
        public List<int> RemovableChildren;
        
        /********************************************************************************************************************************/

        /// <summary>
        ///     One list item for each child hole.
        /// </summary>
        public List<CutIndexChildClass> cutIndexChildClasses;

        /// <summary>
        ///     Edge loop cut indexes on the side of the parent bone.
        /// </summary>
        public List<int> cutIndexesParentSide;

        /// <summary>
        ///     Indexes used to sew the cut on the parent side.
        /// </summary>
        public List<int> sewIndexesParentSide;

        /// <summary>
        ///     Triangles for the <see cref="sewIndexesParentSide" />.
        /// </summary>
        public List<int> sewTrianglesParentSide;

        /// <summary>
        ///     Sew indexes used by the parent when this chunk is cut off.
        /// </summary>
        public List<int> sewIndexesForParent;

        /// <summary>
        ///     Triangles for the <see cref="sewIndexesForParent" />.
        /// </summary>
        public List<int> sewTrianglesForParent;
    }
}