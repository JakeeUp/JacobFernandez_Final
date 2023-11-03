// ---------------------------------------------------
// Gore Simulator
// Copyright (c) Pampel Games e.K. All Rights Reserved.
// https://www.pampelgames.com
// ---------------------------------------------------

using System.Collections.Generic;
using PampelGames.GoreSimulator;
using PampelGames.Shared.Utility;
using UnityEditor;
using UnityEditor.UIElements;
using UnityEngine;
using UnityEngine.UIElements;

namespace PampelGames.Shared.Editor
{
    
    [CustomPropertyDrawer(typeof(SubModuleDecal))]
    public class PGExecuteClassPropertyDrawer : PropertyDrawer
    {
        
        private SubModuleDecal _subModuleDecal;
        
        private SerializedProperty radiusProperty;
        private readonly FloatField radius = new("Radius");
        // private SerializedProperty hardnessProperty;
        // private readonly Slider hardness = new("Hardness");
        private SerializedProperty strengthProperty;
        private readonly Slider strength = new("Strength");
        

        public override VisualElement CreatePropertyGUI(SerializedProperty property)
        {
            var container = new VisualElement();

            var listIndex = PGPropertyDrawerUtility.GetDrawingListIndex(property);
            var obj = fieldInfo.GetValue(property.serializedObject.targetObject);
            var objList = obj as List<SubModuleBase>;
            _subModuleDecal = (SubModuleDecal) objList[listIndex];

            FindAndBindProperties(property);
            VisualizeProperties();
            
            DrawModule();

            
            container.Add(radius);
            // container.Add(hardness);
            container.Add(strength);
            
            return container;
        }
        
        private void FindAndBindProperties(SerializedProperty property)
        {
            radiusProperty = property.FindPropertyRelative(nameof(SubModuleDecal.radius));
            radius.BindProperty(radiusProperty);
            // hardnessProperty = property.FindPropertyRelative(nameof(SubModuleDecal.hardness));
            // hardness.BindProperty(hardnessProperty);
            strengthProperty = property.FindPropertyRelative(nameof(SubModuleDecal.strength));
            strength.BindProperty(strengthProperty);
        }

        private void VisualizeProperties()
        {

            radius.tooltip = "Decal application size, with the center at the impact/cut point. Decal size is set via material tiling.";
            strength.tooltip = "Intensity of the decal effect.";
            // hardness.tooltip = "Sharpness of the edges.";
            
            radius.PGClampValue();
            // hardness.lowValue = 0;
            // hardness.highValue = 1;
            // hardness.showInputField = true;
            strength.lowValue = 0;
            strength.highValue = 1;
            strength.showInputField = true;
        }

        private void DrawModule()
        {
            
        }

    }
}