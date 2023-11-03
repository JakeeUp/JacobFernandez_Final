// ---------------------------------------------------
// Copyright (c) Pampel Games e.K. All Rights Reserved.
// https://www.pampelgames.com
// ---------------------------------------------------

#if UNITY_EDITOR
using System.Collections.Generic;
using PampelGames.Shared.Tools.PGInspector;
using PampelGames.Shared.Utility;
using UnityEditor;
using UnityEditor.UIElements;
using UnityEngine.UIElements;
using TagField = UnityEditor.UIElements.TagField;

namespace PampelGames.Shared.Tools.Editor
{
    [CustomPropertyDrawer(typeof(PGStopOnCollisionEnter))]
    public class PGStopOnCollisionEnterPropertyDrawer : PropertyDrawer
    {
        private PGStopOnCollisionEnter baseClass;

        private readonly VisualElement layerWrapper = new();
        private SerializedProperty useLayerFilterProperty;
        private readonly Toggle useLayerFilter = new();
        private SerializedProperty matchingLayersProperty;
        private readonly PropertyField matchingLayers = new();
        private readonly VisualElement tagWrapper = new();
        private SerializedProperty useTagFilterProperty;
        private readonly Toggle useTagFilter = new();
        private SerializedProperty matchingTagsProperty;
        private readonly ListView matchingTags = new();


        public override VisualElement CreatePropertyGUI(SerializedProperty property)
        {
            var container = new VisualElement();

            FindAndBindProperties(property);
            DrawStop(property);

            matchingLayers.label = "";
            layerWrapper.style.flexDirection = FlexDirection.Row;
            matchingLayers.style.flexGrow = 1f;
            layerWrapper.Add(useLayerFilter);
            layerWrapper.Add(matchingLayers);
            container.Add(layerWrapper);

            matchingTags.fixedItemHeight = 21f;
            matchingTags.showAddRemoveFooter = true;
            matchingTags.PGHideScrollView();

            tagWrapper.style.flexDirection = FlexDirection.Row;
            matchingTags.style.flexGrow = 1f;
            tagWrapper.Add(useTagFilter);
            tagWrapper.Add(matchingTags);
            container.Add(tagWrapper);


            return container;
        }

        private void FindAndBindProperties(SerializedProperty property)
        {
            useLayerFilterProperty = property.FindPropertyRelative(nameof(PGStopOnCollisionEnter.useLayerFilter));
            useLayerFilter.BindProperty(useLayerFilterProperty);
            matchingLayersProperty = property.FindPropertyRelative(nameof(PGStopOnCollisionEnter.matchingLayers));
            matchingLayers.BindProperty(matchingLayersProperty);
            useTagFilterProperty = property.FindPropertyRelative(nameof(PGStopOnCollisionEnter.useTagFilter));
            useTagFilter.BindProperty(useTagFilterProperty);
            matchingTagsProperty = property.FindPropertyRelative(nameof(PGStopOnCollisionEnter.matchingTags));

            var obj = fieldInfo.GetValue(property.serializedObject.targetObject);
            var objList = obj as List<PGStopClassBase>;
            var index = PGInspectorEditorUtility.GetDrawingListIndex(property);
            if (objList != null) baseClass = objList[index] as PGStopOnCollisionEnter;
            if (baseClass != null) matchingTags.itemsSource = baseClass.matchingTags;
        }


        /********************************************************************************************************************************/

        private void DrawStop(SerializedProperty property)
        {
            useLayerFilter.RegisterValueChangedCallback(evt => CheckVisibility());
            useTagFilter.RegisterValueChangedCallback(evt => CheckVisibility());

            void CheckVisibility()
            {
                matchingLayers.style.display = DisplayStyle.None;
                matchingTags.style.display = DisplayStyle.None;

                if (useLayerFilterProperty.boolValue)
                    matchingLayers.style.display = DisplayStyle.Flex;
                if (useTagFilterProperty.boolValue)
                    matchingTags.style.display = DisplayStyle.Flex;
            }

            VisualElement MakeItem()
            {
                var tagField = new TagField();
                return tagField;
            }

            void BindItem(VisualElement itemWrapper, int i)
            {
                if (i < 0) return;
                var tagField = itemWrapper as TagField;
                property.serializedObject.Update();
                property.serializedObject.ApplyModifiedProperties();
                tagField.BindProperty(matchingTagsProperty.GetArrayElementAtIndex(i));
                EditorUtility.SetDirty(property.serializedObject.targetObject);
            }

            matchingTags.makeItem = MakeItem;
            matchingTags.bindItem = BindItem;
        }
    }
}
#endif