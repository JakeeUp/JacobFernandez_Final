// ----------------------------------------------------
// Gore Simulator
// Copyright (c) Pampel Games e.K. All Rights Reserved.
// https://www.pampelgames.com
// ----------------------------------------------------

using System;
using UnityEngine;

namespace PampelGames.GoreSimulator
{
    public class GoreBone : MonoBehaviour, IGoreObject
    {
        public GoreSimulator goreSimulator;
        
        public Collider _collider;
        public Rigidbody _rigidbody;
        public CharacterJoint _characterJoint;
        
        [Tooltip("Whether to invoke the OnDeath event when this bone is being cut.")]
        public bool onDeath;

        internal bool multiCut;
        internal GoreMultiCut _goreMultiCut;

        
        /********************************************************************************************************************************/

        /// <summary>
        /// Executes a ragdoll cut with the given position and force.
        /// The force is applied in the direction from the calculated world center of mass of the object towards the provided position.
        /// </summary>
        /// <param name="position">The position at which the ragdoll cut is executed.</param>
        /// <param name="force">The amount of force to apply to the ragdoll cut.</param>
        public void ExecuteRagdollCut(Vector3 position, float force)
        {
            Vector3 toCenterDirection = (_rigidbody.worldCenterOfMass - position).normalized;
            goreSimulator.ExecuteRagdollCut(gameObject.name, position, toCenterDirection * force);
        }
        
        /// <summary>
        ///     Executes a Ragdoll Cut with force applied in world space.
        /// </summary>
        public void ExecuteRagdollCut(Vector3 position, Vector3 force)
        {
            goreSimulator.ExecuteRagdollCut(gameObject.name, position, force);
        }
        
        /* IGoreObject *****************************************************************************************************************/
        public void ExecuteCut(Vector3 position)
        {
            if(!multiCut) goreSimulator.ExecuteCut(gameObject.name, position);
            else _goreMultiCut.ExecuteCut(gameObject.name, position);
        }

        public void ExecuteCut(Vector3 position, Vector3 force)
        {
            if(!multiCut) goreSimulator.ExecuteCut(gameObject.name, position, force);
            else _goreMultiCut.ExecuteCut(gameObject.name, position, force);
        }

        public void ExecuteCut(string boneName, Vector3 position)
        {
            if(!multiCut) goreSimulator.ExecuteCut(boneName, position);
            else _goreMultiCut.ExecuteCut(boneName, position);
        }

        public void ExecuteCut(string boneName, Vector3 position, Vector3 force)
        {
            if(!multiCut) goreSimulator.ExecuteCut(boneName, position, force);
            else _goreMultiCut.ExecuteCut(boneName, position, force);
        }

        public void ExecuteExplosion()
        {
            if (!multiCut) goreSimulator.ExecuteExplosion();
            else _goreMultiCut.ExecuteExplosion();
        }

        public void ExecuteExplosion(float radialForce)
        {
            if (!multiCut) goreSimulator.ExecuteExplosion(radialForce);
            else _goreMultiCut.ExecuteExplosion(radialForce);
        }

        public void ExecuteExplosion(Vector3 position, float force)
        {
            if (!multiCut) goreSimulator.ExecuteExplosion(position, force);
            else _goreMultiCut.ExecuteExplosion(position, force);
        }
        
    }
}
