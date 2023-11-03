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
    public class BonesClass
    {
        /// <summary>
        ///     Bone.
        /// </summary>
        public Transform bone;
        
        /// <summary>
        ///     Attached <see cref="GoreBone"/> component.
        /// </summary>
        public GoreBone goreBone;
        
        /// <summary>
        ///     True if this is a central bone (pelvis, spine etc.).
        /// </summary>
        public bool centralBone;

        /// <summary>
        ///     First selected parent in hierarchy.
        /// </summary>
        public Transform firstParent;

        public bool parentExists; // Only for saving null check.

        /// <summary>
        ///     The next selected direct children.
        /// </summary>
        public List<Transform> firstChildren;

        /// <summary>
        ///     All direct children of the bone.
        /// </summary>
        public List<Transform> directBoneChildren;

        /// <summary>
        ///     All selected child bones of the bone.
        /// </summary>
        public List<Transform> boneChildrenSel;
        
        
        /********************************************************************************************************************************/

        /// <summary>
        ///     Indexes influenced by the bone. Empty in the runtime.
        /// </summary>
        public List<int> boneIndexes;

        /********************************************************************************************************************************/

        public IndexesStorageClass indexesStorageClass;


        /* Runtime **********************************************************************************************************************/

        internal bool cutted;

        /// <summary>
        ///     -1 means it has not been cutted in between (so completely cutted).
        /// </summary>
        internal int cuttedIndex = -1;

        internal bool firstChildCutted;
    }
}