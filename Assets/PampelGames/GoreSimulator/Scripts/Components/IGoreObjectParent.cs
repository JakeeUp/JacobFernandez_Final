// ----------------------------------------------------
// Gore Simulator
// Copyright (c) Pampel Games e.K. All Rights Reserved.
// https://www.pampelgames.com
// ----------------------------------------------------

using UnityEngine;

namespace PampelGames.GoreSimulator
{
    public interface IGoreObjectParent
    {
        public void ExecuteCut(string boneName, Vector3 position);
        public void ExecuteCut(string boneName, Vector3 position, Vector3 force);

        public void ExecuteExplosion();

        public void ExecuteExplosion(float radialForce);
        
        public void ExecuteExplosion(Vector3 position, float force);
    }
}
