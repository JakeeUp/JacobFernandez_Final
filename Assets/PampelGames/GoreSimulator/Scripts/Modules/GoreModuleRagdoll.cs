// ----------------------------------------------------
// Gore Simulator
// Copyright (c) Pampel Games e.K. All Rights Reserved.
// https://www.pampelgames.com
// ----------------------------------------------------

using System.Collections.Generic;
using UnityEngine;

namespace PampelGames.GoreSimulator
{
    public class GoreModuleRagdoll : GoreModuleBase
    {
        public override string ModuleName()
        {
            return "Ragdoll";
        }

        public override string ModuleInfo()
        {
            return "Ragdoll Module: Enables ragdoll physics on the character and cutted mesh parts.\n" +
                   "\n" + "GoreSimulator.ExecuteRagdoll();";
        }

        public override int imageIndex()
        {
            return 5;
        }

        public override void ClearSubmodules()
        {
            _goreSimulator.ragdollModules.Clear();
        }
        

        /********************************************************************************************************************************/
        public override void Reset(List<BonesClass> bonesClasses)
        {
            if (!_goreSimulator.ragdollInitialized) return;
            base.Reset(bonesClasses);
            RagdollUtility.ToggleRagdoll(_goreSimulator.goreBones, false);
            for (int i = 0; i < _goreSimulator.ragdollModules.Count; i++) _goreSimulator.ragdollModules[i].Reset();
        }
        
        
        /********************************************************************************************************************************/

        public void ExecuteRagdoll(List<GoreBone> goreBones)
        {
            RagdollUtility.ToggleRagdoll(goreBones, true);
            for (int i = 0; i < _goreSimulator.ragdollModules.Count; i++)
            {
                if(!_goreSimulator.ragdollModules[i].moduleActive) continue;
                _goreSimulator.ragdollModules[i].ExecuteModuleRagdoll(goreBones);
            }
        }
        
    }
}
