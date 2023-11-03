// ----------------------------------------------------
// Gore Simulator
// Copyright (c) Pampel Games e.K. All Rights Reserved.
// https://www.pampelgames.com
// ----------------------------------------------------

using System;
using System.Collections.Generic;
using UnityEngine;

namespace PampelGames.GoreSimulator
{
    public class SubModuleDisableComponents : SubModuleBase
    {
        public override string ModuleName()
        {
            return "Disable Components";
        }

        public override string ModuleInfo()
        {
            return "Disables the specified components when executed.";
        }

        public override int imageIndex()
        {
            return 6;
        }

        public override void ModuleAdded(Type type)
        {
            base.ModuleAdded(type);
            components ??= new List<Component>();
#if UNITY_EDITOR
            if (type == typeof(GoreModuleRagdoll))
            {
                var animator = _goreSimulator.ragdollAnimator;
                if (animator == null) return;
                components.Add(animator);    
            }
#endif
        }

        public override bool CompatibleCut()
        {
            return false;
        }

        /********************************************************************************************************************************/

        public List<Component> components = new();
        
        /********************************************************************************************************************************/

        public override void ExecuteModuleCut(SubModuleClass subModuleClass)
        {
            
        }

        public override void ExecuteModuleExplosion(SubModuleClass subModuleClass)
        {
            if (subModuleClass.multiCut || subModuleClass.subRagdoll) return;
            SetComponents(false);
        }

        public override void ExecuteModuleRagdoll(List<GoreBone> goreBones)
        {
            SetComponents(false);
        }
        
        public override void Reset()
        {
            base.Reset();
            SetComponents(true);
        }

        /********************************************************************************************************************************/

        private void SetComponents(bool enabled)
        {
            foreach (var component in components)
            {
                switch (component)
                {
                    case Behaviour behaviour:
                        behaviour.enabled = enabled;
                        break;
                    case Renderer renderer: 
                        renderer.enabled = enabled;
                        break;
                }
            }
        }
    }
}
