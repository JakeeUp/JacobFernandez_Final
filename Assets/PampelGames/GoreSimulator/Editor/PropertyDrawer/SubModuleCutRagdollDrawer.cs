// ----------------------------------------------------
// Gore Simulator
// Copyright (c) Pampel Games e.K. All Rights Reserved.
// https://www.pampelgames.com
// ----------------------------------------------------

using System.Collections.Generic;
using PampelGames.Shared;
using PampelGames.Shared.Editor;
using UnityEditor;
using UnityEditor.UIElements;
using UnityEngine.UIElements;

namespace PampelGames.GoreSimulator.Editor
{
    [CustomPropertyDrawer(typeof(SubModuleCutRagdoll))]
    public class SubModuleCutRagdollDrawer : PropertyDrawer
    {
        private GoreSimulator _goreSimulator;
        private SubModuleCutRagdoll _subModuleRagdoll;


        private SerializedProperty minimumBoneAmountProperty;
        private readonly IntegerField minimumBoneAmount = new("Min. Bones");
        private SerializedProperty dragProperty;
        private readonly FloatField drag = new("Drag");
        private SerializedProperty angularDragProperty;
        private readonly FloatField angularDrag = new("Angular Drag");


        public override VisualElement CreatePropertyGUI(SerializedProperty property)
        {
            var container = new VisualElement();

            _goreSimulator = property.serializedObject.targetObject as GoreSimulator;
            var listIndex = PGPropertyDrawerUtility.GetDrawingListIndex(property);
            var obj = fieldInfo.GetValue(_goreSimulator);
            var objList = obj as List<SubModuleBase>;
            _subModuleRagdoll = (SubModuleCutRagdoll) objList[listIndex];

            FindAndBindProperties(property);
            VisualizeProperties();

            DrawModule();

            container.Add(minimumBoneAmount);
            container.Add(drag);
            container.Add(angularDrag);

            if (ExecutionUtility.FindGoreModule<GoreModuleRagdoll>(_goreSimulator.goreModules) is not GoreModuleRagdoll moduleRagdoll)
            {
                var helpBox = new HelpBox("Ragdoll Module is not initialized!", HelpBoxMessageType.Warning);
                container.Add(helpBox);
            }


            return container;
        }

        private void FindAndBindProperties(SerializedProperty property)
        {
            minimumBoneAmountProperty = property.FindPropertyRelative(nameof(SubModuleCutRagdoll.minimumBoneAmount));
            minimumBoneAmount.BindProperty(minimumBoneAmountProperty);
            dragProperty = property.FindPropertyRelative(nameof(SubModuleCutRagdoll.drag));
            drag.BindProperty(dragProperty);
            angularDragProperty = property.FindPropertyRelative(nameof(SubModuleCutRagdoll.angularDrag));
            angularDrag.BindProperty(angularDragProperty);
        }

        private void VisualizeProperties()
        {
            minimumBoneAmount.tooltip = "Defines the minimum number of cut bones needed for a ragdoll to be generated on the cut mesh.";
            minimumBoneAmount.PGClampValue(1);
            drag.tooltip = "Drag can be used to slow down an object. The higher the drag the more the object slows down.";
            drag.PGClampValue();
            angularDrag.tooltip =
                "Angular drag can be used to slow down the rotation of an object. The higher the drag the more the rotation slows down.";
            angularDrag.PGClampValue();
        }

        private void DrawModule()
        {
        }
    }
}