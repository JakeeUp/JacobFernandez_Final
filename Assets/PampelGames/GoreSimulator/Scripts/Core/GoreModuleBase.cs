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
    ///     Abstract base class for main gore modules.
    /// </summary>
    [Serializable]
    public abstract class GoreModuleBase
    {

        public abstract string ModuleName();

        public abstract string ModuleInfo();
        
        public abstract int imageIndex();

        public abstract void ClearSubmodules();

        public GoreSimulator _goreSimulator;

        /********************************************************************************************************************************/
        
        public virtual void Initialize()
        {
            
        }
        
        public virtual void Reset(List<BonesClass> bonesClasses)
        {
            
        }

        public virtual void Destroyed()
        {
            
        }


    }
}
