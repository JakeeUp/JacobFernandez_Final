// ----------------------------------------------------
// Gore Simulator
// Copyright (c) Pampel Games e.K. All Rights Reserved.
// https://www.pampelgames.com
// ----------------------------------------------------

using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace PampelGames.GoreSimulator
{
    public static class ShaderConstants
    {
        public static List<string> ComponentColorKeywords()
        {
            var boneNames = new List<string>
            {
                "_Color",
                "_BaseColor",
                "_MainColor",
                "_TintColor"
            };
            return boneNames;
        }
        
        public static List<int> ComponentColorKeywordIDs()
        {
            var boneNames = ComponentColorKeywords();
            return boneNames.Select(Shader.PropertyToID).ToList();
        }
        
        public static readonly int centerID = Shader.PropertyToID("_Center");
        public static readonly int hardnessID = Shader.PropertyToID("_Hardness");
        public static readonly int strengthID = Shader.PropertyToID("_Strength");
        public static readonly int radiusID = Shader.PropertyToID("_Radius");
        public static readonly int blendOpID = Shader.PropertyToID("_BlendOp");
        
        public const string MaskTexture = "_MaskTexture";
    }
}
