// ----------------------------------------------------
// Gore Simulator
// Copyright (c) Pampel Games e.K. All Rights Reserved.
// https://www.pampelgames.com
// ----------------------------------------------------

using UnityEngine;

namespace PampelGames.GoreSimulator
{
    public class GoreMesh : MonoBehaviour, IGoreObject
    {
        internal string _boneName;
        
        internal GoreMultiCut _goreMultiCut;


        public void ExecuteCut(Vector3 position)
        {
            _goreMultiCut.ExecuteCut(_boneName, position);
        }

        public void ExecuteCut(Vector3 position, Vector3 force)
        {
            _goreMultiCut.ExecuteCut(_boneName, position, force);
        }

        public void ExecuteCut(string boneName, Vector3 position)
        {
            _goreMultiCut.ExecuteCut(boneName, position);
        }

        public void ExecuteCut(string boneName, Vector3 position, Vector3 force)
        {
            _goreMultiCut.ExecuteCut(boneName, position, force);
        }

        public void ExecuteExplosion()
        {
            _goreMultiCut.ExecuteExplosion();
        }

        public void ExecuteExplosion(float radialForce)
        {
            _goreMultiCut.ExecuteExplosion(radialForce);
        }

        public void ExecuteExplosion(Vector3 position, float force)
        {
            _goreMultiCut.ExecuteExplosion(position, force);
        }
    }
}