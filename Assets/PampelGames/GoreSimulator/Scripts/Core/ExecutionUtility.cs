// ----------------------------------------------------
// Gore Simulator
// Copyright (c) Pampel Games e.K. All Rights Reserved.
// https://www.pampelgames.com
// ----------------------------------------------------

using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace PampelGames.GoreSimulator
{
    public static class ExecutionUtility
    {
        
        public static GoreModuleBase FindGoreModule<T>(List<GoreModuleBase> goreModules)
        {
            for (var i = 0; i < goreModules.Count; i++)
            {
                if (goreModules[i].GetType() != typeof(T)) continue;
                return goreModules[i];
            }

            return null;
        }
        
        public static SubModuleBase FindSubModule<T>(List<SubModuleBase> subModules)
        {
            for (var i = 0; i < subModules.Count; i++)
            {
                if (subModules[i].GetType() != typeof(T)) continue;
                return subModules[i];
            }

            return null;
        }
        
        public static void AddCutMaterial(GoreSimulator goreSimulator)
        {
            if (goreSimulator.cutMaterialAdded) return;
            Material[] materials = goreSimulator.smr.materials;
            Array.Resize(ref materials, materials.Length + 1);
            materials[^1] = goreSimulator.cutMaterial;
            goreSimulator.smr.materials = materials;
            goreSimulator.cutMaterialAdded = true;
        }
        public static bool AddDecalMaterial(GoreSimulator goreSimulator)
        {
            if (goreSimulator.decalMaterialAdded) return false;
            Material[] materials = goreSimulator.smr.materials;
            Array.Resize(ref materials, materials.Length + 1);
            materials[^1] = goreSimulator.decalMaterial;
            goreSimulator.smr.materials = materials;
            goreSimulator.decalMaterialAdded = true;
            return true;
        }
        
        
        
        
        
        
    }
}