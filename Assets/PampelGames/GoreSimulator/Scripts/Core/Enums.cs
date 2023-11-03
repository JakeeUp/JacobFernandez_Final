// ----------------------------------------------------
// Gore Simulator
// Copyright (c) Pampel Games e.K. All Rights Reserved.
// https://www.pampelgames.com
// ----------------------------------------------------

using UnityEngine;

namespace PampelGames.GoreSimulator
{
    public static class Enums
    {
        public enum Collider
        {
            None,
            Box,
            Capsule,
            Mesh,
        }

        public enum Children
        {
            None,
            Rendered,
            All
        }

        public enum MultiCutStatus
        {
            Mesh,
            Ragdoll
        }

        public enum ParticlePositionExpl
        {
            Center,
            Method,
            Parts
        }

        public enum ParticleRotationExpl
        {
            Default,
            Method,
            Random
        }
        
        public enum ParticleRotationCut
        {
            CutDirection,
            Default,
            Method,
            Random
        }
        
        public enum ParticleSetParent
        {
            None,
            Both,
            Character,
            DetachedPart
        }
    }
}
