// ----------------------------------------------------
// Gore Simulator
// Copyright (c) Pampel Games e.K. All Rights Reserved.
// https://www.pampelgames.com
// ----------------------------------------------------

using System.Collections.Generic;
using PampelGames.Shared.Editor;
using PampelGames.Shared.Utility;
using UnityEditor;
using UnityEditor.UIElements;
using UnityEngine;
using UnityEngine.UIElements;

namespace PampelGames.GoreSimulator.Editor
{
    [CustomPropertyDrawer(typeof(SubModulePhysics))]
    public class SubModulePhysicsDrawer : PropertyDrawer
    {
        private SubModulePhysics _subModulePhysics;

        private VisualElement ColliderWrapper = new();
        private SerializedProperty layerMaskProperty;
        private readonly LayerField layerMask = new("Layer");
        
        private SerializedProperty colliderProperty;
        private readonly EnumField collider = new("Collider");
        private SerializedProperty rigidbodyProperty;
        private readonly Toggle rigidbody = new("Rigidbody");
        
        
        public override VisualElement CreatePropertyGUI(SerializedProperty property)
        {
            var container = new VisualElement();

            var listIndex = PGPropertyDrawerUtility.GetDrawingListIndex(property);
            var obj = fieldInfo.GetValue(property.serializedObject.targetObject);
            var objList = obj as List<SubModuleBase>;
            _subModulePhysics = (SubModulePhysics) objList[listIndex];

            FindAndBindProperties(property);
            VisualizeProperties();
            
            MaterialVisualization();
            DrawModule();
            
            ColliderWrapper.Add(layerMask);
            
            container.Add(collider);
            container.Add(ColliderWrapper);
            
            rigidbody.PGDrawTopLine(true);
            container.Add(rigidbody);
            

            return container;
        }
        
        private void FindAndBindProperties(SerializedProperty property)
        {
            layerMaskProperty = property.FindPropertyRelative(nameof(SubModulePhysics.layer));
            layerMask.BindProperty(layerMaskProperty);
            
            
            colliderProperty = property.FindPropertyRelative(nameof(SubModulePhysics.collider));
            collider.BindProperty(colliderProperty);
            rigidbodyProperty = property.FindPropertyRelative(nameof(SubModulePhysics.rigidbody));
            rigidbody.BindProperty(rigidbodyProperty);
            
        }

        private void VisualizeProperties()
        {
            string meshColliderTooltip = "Mesh colliders are the most realistic but by far the most expensive to add.";
            collider.tooltip = "Adds colliders to detached objects. " + meshColliderTooltip;
            
            layerMask.tooltip = "Layer being used for attached colliders.";

            rigidbody.tooltip = "Adds rigidbodies to detached objects.";
        }

        private void DrawModule()
        {
            collider.RegisterValueChangedCallback(evt =>
            {
                MaterialVisualization();
            });
        }

        private void MaterialVisualization()
        {
            if (_subModulePhysics.collider == Enums.Collider.None)
            {
                ColliderWrapper.style.display = DisplayStyle.None;
            }
            else
            {
                ColliderWrapper.style.display = DisplayStyle.Flex;
            }
        }
    }
}