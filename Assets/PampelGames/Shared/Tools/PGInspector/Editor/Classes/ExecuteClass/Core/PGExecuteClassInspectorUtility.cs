// ----------------------------------------------------
// Copyright (c) Pampel Games e.K. All Rights Reserved.
// https://www.pampelgames.com
// ----------------------------------------------------

#if UNITY_EDITOR
using System;
using System.Collections.Generic;
using System.Reflection;
using PampelGames.Shared.Utility;
using UnityEditor;
using UnityEditor.UIElements;
using UnityEngine;
using UnityEngine.UIElements;

namespace PampelGames.Shared.Tools.PGInspector.Editor
{
    /// <summary>
    ///     Static helper methods for MonoBehaviour Inspectors that implement <see cref="PGExecuteClass" />.
    /// </summary>
    public static class PGExecuteClassInspectorUtility
    {
        /* Public *******************************************************************************************************************************/

        /// <summary>
        ///     Adds all <see cref="PGExecuteClassBase" /> types to the respective Editor lists.
        /// </summary>
        /// <param name="assemblies">Use AppDomain.CurrentDomain.GetAssemblies().</param>
        /// <param name="executeClassBaseList">List the inspector must create.</param>
        /// <param name="executeClassBaseStringList">Respective string list the inspector must create.</param>
        public static void AddExecuteClassTypes(Assembly[] assemblies, List<PGExecuteClassBase> executeClassBaseList,
            List<string> executeClassBaseStringList)
        {
            var instances = PGClassUtility.CreateInstances<PGExecuteClassBase>(assemblies);
            foreach (var instance in instances) executeClassBaseList.Add(instance);
            foreach (var executeClassBase in executeClassBaseList) executeClassBaseStringList.Add(executeClassBase.ExecuteName());
        }


        /// <summary>
        ///     Draw list of <see cref="PGExecuteClassBase" /> in the inspector of the implementing class. Must be named "executeClasses".
        /// </summary>
        /// <param name="serializedObject">SerializedObject of the inspector.</param>
        /// <param name="baseComponentExecuteClassBaseList">List of <see cref="PGExecuteClassBase" /> of the base component.</param>
        /// <param name="addExecuteDropdown">Dropdown created by the UXML, showing the string names in the inspector. Usually child of a button.</param>
        /// <param name="executeClassBaseList">
        ///     List with all <see cref="PGExecuteClassBase" /> in all assemblies the inspector must create. Fill it with
        ///     <see cref="PGExecuteClassInspectorUtility.AddExecuteClassTypes" />
        /// </param>
        /// <param name="visualElementParent">Parent element the implementation should be added to.</param>
        /// <param name="parentInsertIndex">Index of the parent element the implementation should be inserted.</param>
        public static void DrawExecuteClass(SerializedObject serializedObject, List<PGExecuteClassBase> baseComponentExecuteClassBaseList,
            DropdownField addExecuteDropdown, List<PGExecuteClassBase> executeClassBaseList, VisualElement visualElementParent, int parentInsertIndex)
        {
            addExecuteDropdown.RegisterCallback<ChangeEvent<string>>(evt =>
            {
                if (addExecuteDropdown.index >= 0)
                {
                    var canAdd = true;
                    foreach (var executeClassBase in baseComponentExecuteClassBaseList)
                        if (executeClassBaseList[addExecuteDropdown.index].ExecuteName() == executeClassBase.ExecuteName())
                        {
                            canAdd = false;
                            EditorUtility.DisplayDialog("Invalid Item", executeClassBaseList[addExecuteDropdown.index].ExecuteName() +
                                                                        " allready exists for this component!", "Ok");
                            break;
                        }

                    if (canAdd)
                    {
                        baseComponentExecuteClassBaseList.Add(
                            (PGExecuteClassBase) Activator.CreateInstance(executeClassBaseList[addExecuteDropdown.index].GetType()));
                        EditorUtility.SetDirty(serializedObject.targetObject);
                    }
                }

                addExecuteDropdown.value = string.Empty;

                serializedObject.Update();
                serializedObject.ApplyModifiedProperties();
                DrawExecuteClassListView(serializedObject, baseComponentExecuteClassBaseList, addExecuteDropdown, executeClassBaseList,
                    visualElementParent, parentInsertIndex);
            });
            DrawExecuteClassListView(serializedObject, baseComponentExecuteClassBaseList, addExecuteDropdown, executeClassBaseList,
                visualElementParent, parentInsertIndex);
        }


        public static void RemoveExistingListView(VisualElement visualElementParent)
        {
            var existingList = visualElementParent.Q<ListView>("executeClassListView");
            if (existingList != null) visualElementParent.Remove(existingList);
        }


        /********************************************************************************************************************************/


        private static void DrawExecuteClassListView(SerializedObject serializedObject, List<PGExecuteClassBase> baseComponentExecuteClassBaseList,
            DropdownField addExecuteDropdown, List<PGExecuteClassBase> executeClassBaseList, VisualElement visualElementParent, int parentInsertIndex)
        {
            var existingList = visualElementParent.Q<ListView>("executeClassListView");
            if (existingList != null) visualElementParent.Remove(existingList);
            var executeClassListView = new ListView(baseComponentExecuteClassBaseList);
            executeClassListView.name = "executeClassListView";
            executeClassListView.virtualizationMethod = CollectionVirtualizationMethod.DynamicHeight;
            executeClassListView.PGHideScrollView();

            VisualElement MakeItem()
            {
                var itemWrapper = new VisualElement();
                itemWrapper.style.flexDirection = FlexDirection.Row;

                var itemLabel = new Label();
                itemLabel.name = "itemLabel";
                itemLabel.style.width = 180;
                itemLabel.style.unityTextAlign = TextAnchor.MiddleLeft;
                itemLabel.style.color = PGColors.InspectorVariableText();

                var itemProperty = new PropertyField();
                itemProperty.name = "itemProperty";
                itemProperty.style.flexGrow = 2f;

                var itemRemove = new Button();
                itemRemove.name = "itemRemove";
                itemRemove.text = "-";
                itemRemove.tooltip = "Remove";
                itemRemove.PGBackgroundColorHover(PGColors.HoverButtonRed());

                itemWrapper.PGBackgroundColorHover(PGColors.ListViewHover(), PGColors.InspectorBackground());
                itemWrapper.RegisterCallback<ClickEvent>(evt => { itemWrapper.style.backgroundColor = PGColors.InspectorBackground(); });

                itemWrapper.Add(itemLabel);
                itemWrapper.Add(itemProperty);
                itemWrapper.Add(itemRemove);

                return itemWrapper;
            }

            void BindItem(VisualElement itemWrapper, int i)
            {
                var itemLabel = itemWrapper.Q<Label>("itemLabel");
                itemLabel.text = "Execute: " + baseComponentExecuteClassBaseList[i].ExecuteName();
                itemLabel.tooltip = baseComponentExecuteClassBaseList[i].ExecuteInfo();

                var itemProperty = itemWrapper.Q<PropertyField>("itemProperty");
                var itemRemove = itemWrapper.Q<Button>("itemRemove");

                itemProperty.BindProperty(serializedObject.FindProperty("executeClasses.Array.data[" + i + "]"));
                serializedObject.Update();
                serializedObject.ApplyModifiedProperties();

                itemRemove.RegisterCallback<ClickEvent>(x =>
                {
                    if (baseComponentExecuteClassBaseList.Count > i) baseComponentExecuteClassBaseList.RemoveAt(i);
                    serializedObject.Update();
                    serializedObject.ApplyModifiedProperties();
                    EditorUtility.SetDirty(serializedObject.targetObject);
                    DrawExecuteClass(serializedObject, baseComponentExecuteClassBaseList, addExecuteDropdown, executeClassBaseList,
                        visualElementParent, parentInsertIndex);
                });
            }

            executeClassListView.makeItem = MakeItem;
            executeClassListView.bindItem = BindItem;

            if (baseComponentExecuteClassBaseList.Count <= 0) return;
            if (visualElementParent.childCount >= parentInsertIndex)
                visualElementParent.Insert(parentInsertIndex, executeClassListView);
            else
                visualElementParent.Add(executeClassListView);
        }
    }
}

#endif