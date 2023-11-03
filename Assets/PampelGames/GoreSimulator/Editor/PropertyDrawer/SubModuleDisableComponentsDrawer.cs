// ----------------------------------------------------
// Gore Simulator
// Copyright (c) Pampel Games e.K. All Rights Reserved.
// https://www.pampelgames.com
// ----------------------------------------------------

using System;
using System.Collections.Generic;
using PampelGames.Shared.Editor;
using PampelGames.Shared.Utility;
using UnityEditor;
using UnityEditor.UIElements;
using UnityEngine;
using UnityEngine.UIElements;

namespace PampelGames.GoreSimulator.Editor
{
    [CustomPropertyDrawer(typeof(SubModuleDisableComponents))]
    public class SubModuleDisableComponentsDrawer : PropertyDrawer
    {
        private SubModuleDisableComponents _subModuleDisableComponents;

        private SerializedProperty componentsProperty;
        private readonly ListView components = new();
        
        
        public override VisualElement CreatePropertyGUI(SerializedProperty property)
        {
            var container = new VisualElement();

            var listIndex = PGPropertyDrawerUtility.GetDrawingListIndex(property);
            var obj = fieldInfo.GetValue(property.serializedObject.targetObject);
            var objList = obj as List<SubModuleBase>;
            _subModuleDisableComponents = (SubModuleDisableComponents) objList[listIndex];

            FindAndBindProperties(property);
            VisualizeProperties();
            
            DrawModule();
            
            container.Add(components);
            

            return container;
        }
        
        private void FindAndBindProperties(SerializedProperty property)
        {
            componentsProperty = property.FindPropertyRelative(nameof(SubModuleDisableComponents.components));
            components.BindProperty(componentsProperty);
        }

        private void VisualizeProperties()
        {
            components.tooltip = "Components to disable/enable when the ragdoll activates/deactivates.";
        }

        private void DrawModule()
        {
            components.PGSetupObjectListView(componentsProperty, _subModuleDisableComponents.components);   
            components.PGObjectListViewStyle();
        }



    }
}