// ----------------------------------------------------
// Gore Simulator
// Copyright (c) Pampel Games e.K. All Rights Reserved.
// https://www.pampelgames.com
// ----------------------------------------------------

using UnityEngine;

namespace PampelGames.GoreSimulator.Editor
{
    internal static class RagdollEditorUtility
    {
        public static void AssignRagdollAnimator(GoreSimulator _goreSimulator)
        {
            if (_goreSimulator.ragdollAnimator == null && _goreSimulator.setupRagdollAnimator)
            {
                var animator = _goreSimulator.smr.gameObject.GetComponent<Animator>();
                if(animator == null) animator = _goreSimulator.smr.transform.parent.gameObject.GetComponent<Animator>();
                if (animator != null) _goreSimulator.ragdollAnimator = animator;
            }
        }

        
    }
}