// ----------------------------------------------------
// Gore Simulator
// Copyright (c) Pampel Games e.K. All Rights Reserved.
// https://www.pampelgames.com
// ----------------------------------------------------

using UnityEngine;

namespace PampelGames.GoreSimulator
{
    /// <summary>
    ///     Execution should preferably be done over the IGoreObject.
    /// </summary>
    public interface IGoreObject : IGoreObjectParent
    {
        public void ExecuteCut(Vector3 position);
        public void ExecuteCut(Vector3 position, Vector3 force);

        
    }
}
