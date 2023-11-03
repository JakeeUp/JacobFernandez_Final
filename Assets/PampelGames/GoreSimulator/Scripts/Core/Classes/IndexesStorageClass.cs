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
    public class IndexesStorageClass
    {
        
        /// <summary>
        ///     Empty mesh precached in Editor. One mesh for each <see cref="CutIndexClass"/>. Center uses only first index for its class.
        /// </summary>
        public List<Mesh> cutMeshes;

        /// <summary>
        ///     Full meshes precached in Editor. One mesh for each <see cref="CutIndexClass"/>.
        /// </summary>
        public List<ChunkClass> chunkClasses;
        
    }
}
