// ----------------------------------------------------
// Gore Simulator
// Copyright (c) Pampel Games e.K. All Rights Reserved.
// https://www.pampelgames.com
// ----------------------------------------------------

using UnityEngine;

namespace PampelGames.GoreSimulator
{
    public class SO_DefaultReferences : ScriptableObject
    {
        public Material cutMaterial;
        public PhysicMaterial physicMaterial;
        
        public GameObject pooledMesh;
        
        public Material skinnedDecal;
        public Material uvTransformation;

        public ParticleSystem cutParticle;
        public ParticleSystem explosionParticle;
    }
}
