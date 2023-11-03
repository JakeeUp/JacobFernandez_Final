// ----------------------------------------------------
// Copyright (c) Pampel Games e.K. All Rights Reserved.
// https://www.pampelgames.com
// ----------------------------------------------------

using System;
using UnityEngine;

namespace PampelGames.Shared.Tools.PGInspector
{
    public class PGStopOnParticleCollision : PGStopClassBase
    {
        /* Editor Virtual******************************************************************************************************************/

#if UNITY_EDITOR
        public override string StopName()
        {
            return "On Particle Collision";
        }
        
        public override string StopInfo()
        {
            return "Stops when an attached collider gets hit by a particle.";
        }
#endif

        public override void ComponentOnParticleCollision(MonoBehaviour baseComponent, Action StopAction)
        {
            base.ComponentOnParticleCollision(baseComponent, StopAction);
            StopAction();
        }
        
    }
}