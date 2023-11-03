// ----------------------------------------------------
// Gore Simulator
// Copyright (c) Pampel Games e.K. All Rights Reserved.
// https://www.pampelgames.com
// ----------------------------------------------------

using System;
using System.Collections.Generic;

namespace PampelGames.GoreSimulator
{
    public static class Constants
    {
        
        public const string GlobalSettings = "GlobalSettings";
        public const string DefaultReferences = "DefaultReferences";

        
#if UNITY_EDITOR
        
        public const string DocumentationURL = "https://docs.google.com/document/d/1NqL4Zc172D0frM8DHTNtB8lufxdg9uqegMT6KVhmMHo/edit?usp=sharing";
        
        
        public static List<string> BonesSetupCenter()
        {
            var boneNames = new List<string>
            {
                "pelvis",
                "center",
            };
            return boneNames;
        }
        public static List<string> BonesSetupDuplicated()
        {
            var boneNames = new List<string>
            {
                "spine",
                "center",
            };
            return boneNames;
        }
        public static List<string> BonesSetupHumanoid()
        {
            var boneNames = new List<string>
            {
                "pelvis",
                "spine",
                "arm",
                "head",
                "leg",
                "thigh",
                "calf",
            };
            return boneNames;
        }

        /********************************************************************************************************************************/
        // Character Joint Setup
        public static List<string> HeadBones()
        {
            var boneNames = new List<string>
            {
                "head",
                "neck"
            };
            return boneNames;
        }
        public static List<string> CenterBones()
        {
            var boneNames = new List<string>
            {
                "pelvis",
                "spine",
                "tail",
                "center"
            };
            return boneNames;
        }
        
        public static List<string> ArmBones()
        {
            var boneNames = new List<string>
            {
                "arm",
                "branch",
                "limp",
                "clavicle",
                "hand"
            };
            return boneNames;
        }
        
        public static List<string> UpperLegBones()
        {
            var boneNames = new List<string>
            {
                "upper",
                "thigh"
            };
            return boneNames;
        }
        
        public static List<string> LowerLegBones()
        {
            var boneNames = new List<string>
            {
                "lower",
                "calf"
            };
            return boneNames;
        }

        public static bool ContainsName(string nameToCheck, List<string> names)
        {
            for (int i = 0; i < names.Count; i++)
            {
                if (nameToCheck.Contains(names[i])) return true;
            }
            
            return false;
        }
#endif
        
    }
}
