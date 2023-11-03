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
    /// <summary>
    ///     Abstract base class for sub-modules.
    /// </summary>
    [Serializable]
    public abstract class SubModuleBase
    {

        /// <summary>
        ///     Name of the module (editor only).
        /// </summary>
        public abstract string ModuleName();

        /// <summary>
        ///     Basic information of the module (editor only).
        /// </summary>
        /// <returns></returns>
        public virtual string ModuleInfo()
        {
            return "";
        }

        /// <summary>
        ///     Image to use from the "ModuleIconList" scriptable object (editor only).
        /// </summary>
        public virtual int imageIndex()
        {
            return 0;
        }

        /// <summary>
        ///     Can this module be added to <see cref="GoreModuleCut"/>.;
        /// </summary>
        public virtual bool CompatibleCut()
        {
            return true;
        }

        /// <summary>
        ///     Can this module be added to <see cref="GoreModuleExplosion"/>.;
        /// </summary>
        public virtual bool CompatibleExplosion()
        {
            return true;
        }

        /// <summary>
        ///     Can this module be added to <see cref="GoreModuleRagdoll"/>.;
        /// </summary>
        public virtual bool CompatibleRagdoll()
        {
            return true;
        }
        
        [HideInInspector] public GoreSimulator _goreSimulator;
        internal bool moduleActive = true;

        /********************************************************************************************************************************/
        
        /// <summary>
        ///     The module has been added in the inspector.
        /// </summary>
        /// <param name="type">Type of the main-module it has been added to, for example <see cref="GoreModuleCut"/>.</param>
        public virtual void ModuleAdded(Type type)
        {
            
        }
        
        
        /********************************************************************************************************************************/
        
        /// <summary>
        ///     Called by <see cref="GoreSimulator"/> in Awake().
        /// </summary>
        public virtual void Initialize()
        {
            
        }
        
        /// <summary>
        ///     Module being executed by the <see cref="GoreModuleCut"/> main-module.
        /// </summary>
        public abstract void ExecuteModuleCut(SubModuleClass subModuleClass);
        
        /// <summary>
        ///     Module being executed by the <see cref="GoreModuleExplosion"/> main-module.
        /// </summary>
        public abstract void ExecuteModuleExplosion(SubModuleClass subModuleClass);
        
        /// <summary>
        ///     Module being executed by the <see cref="GoreModuleRagdoll"/> main-module.
        /// </summary>
        public abstract void ExecuteModuleRagdoll(List<GoreBone> goreBones);

        /// <summary>
        ///     Called by <see cref="GoreSimulator.ResetCharacter()"/>.
        /// </summary>
        public virtual void Reset()
        {
            
        }
        
        /// <summary>
        ///     Called when the <see cref="GoreSimulator"/> GameObject has been destroyed.
        /// </summary>
        public virtual void Destroyed()
        {
            
        }

    }
}