// ----------------------------------------------------
// Copyright (c) Pampel Games e.K. All Rights Reserved.
// https://www.pampelgames.com
// ----------------------------------------------------

using System;
using UnityEngine;

namespace PampelGames.Shared.Tools.PGInspector
{
    public class PGExecuteOnParticleCollision : PGExecuteClassBase
    {
        /* Editor Virtual******************************************************************************************************************/

#if UNITY_EDITOR
        public override string ExecuteName()
        {
            return "On Particle Collision";
        }
        
        public override string ExecuteInfo()
        {
            return "Starts when an attached collider gets hit by a particle.";
        }
#endif

        public override void ComponentOnParticleCollision(MonoBehaviour baseComponent, Action ExecuteAction)
        {
            base.ComponentOnParticleCollision(baseComponent, ExecuteAction);
            ExecuteAction();
        }
        
    }
}