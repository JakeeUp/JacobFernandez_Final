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
    ///     Static helper methods for MonoBehaviour Inspectors that implement <see cref="PGStopClassBase" />.
    /// </summary>
    public static class PGStopClassInspectorUtility
    {
        /* Public *******************************************************************************************************************************/

        /// <summary>
        ///     Adds all <see cref="PGStopClassBase" /> types to the respective Editor lists.
        /// </summary>
        /// <param name="assemblies">Use AppDomain.CurrentDomain.GetAssemblies().</param>
        /// <param name="stopClassBaseList">List the inspector must create.</param>
        /// <param name="stopClassBaseStringList">Respective string list the inspector must create.</param>
        public static void AddStopClassTypes(Assembly[] assemblies, List<PGStopClassBase> stopClassBaseList, List<string> stopClassBaseStringList)
        {
            var instances = PGClassUtility.CreateInstances<PGStopClassBase>(assemblies);
            foreach (var instance in instances) stopClassBaseList.Add(instance);
            foreach (var stopClassBase in stopClassBaseList) stopClassBaseStringList.Add(stopClassBase.StopName());
        }


        /// <summary>
        ///     Draw list of <see cref="PGStopClassBase" /> in the inspector of the implementing class. Must be named "stopClasses".
        /// </summary>
        /// <param name="serializedObject">SerializedObject of the inspector.</param>
        /// <param name="baseComponentStopClassBaseList">List of <see cref="PGStopClassBase" /> of the base component.</param>
        /// <param name="addStopDropdown">Dropdown created by the UXML, showing the string names in the inspector. Usually child of a button.</param>
        /// <param name="stopClassBaseList">
        ///     List with all <see cref="PGStopClassBase" /> in all assemblies the inspector must create. Fill it with
        ///     <see cref="PGStopClassInspectorUtility.AddStopClassTypes" />
        /// </param>
        /// <param name="visualElementParent">Parent element the implementation should be added to.</param>
        /// <param name="parentInsertIndex">Index of the parent element the implementation should be inserted.</param>
        public static void DrawStopClass(SerializedObject serializedObject, List<PGStopClassBase> baseComponentStopClassBaseList,
            DropdownField addStopDropdown, List<PGStopClassBase> stopClassBaseList, VisualElement visualElementParent, int parentInsertIndex)
        {
            addStopDropdown.RegisterCallback<ChangeEvent<string>>(evt =>
            {
                if (addStopDropdown.index >= 0)
                {
                    var canAdd = true;
                    foreach (var stopClassBase in baseComponentStopClassBaseList)
                        if (stopClassBaseList[addStopDropdown.index].StopName() == stopClassBase.StopName())
                        {
                            canAdd = false;
                            EditorUtility.DisplayDialog("Invalid Item", stopClassBaseList[addStopDropdown.index].StopName() +
                                                                        " allready exists for this component!", "Ok");
                            break;
                        }

                    if (canAdd)
                    {
                        baseComponentStopClassBaseList.Add(
                            (PGStopClassBase) Activator.CreateInstance(stopClassBaseList[addStopDropdown.index].GetType()));
                        EditorUtility.SetDirty(serializedObject.targetObject);
                    }
                }

                addStopDropdown.value = string.Empty;

                serializedObject.Update();
                serializedObject.ApplyModifiedProperties();
                DrawStopClassListView(serializedObject, baseComponentStopClassBaseList, addStopDropdown, stopClassBaseList, visualElementParent,
                    parentInsertIndex);
            });
            DrawStopClassListView(serializedObject, baseComponentStopClassBaseList, addStopDropdown, stopClassBaseList, visualElementParent,
                parentInsertIndex);
        }

        public static void RemoveExistingListView(VisualElement visualElementParent)
        {
            var existingList = visualElementParent.Q<ListView>("stopClassListView");
            if (existingList != null) visualElementParent.Remove(existingList);
        }

        /********************************************************************************************************************************/


        private static void DrawStopClassListView(SerializedObject serializedObject, List<PGStopClassBase> baseComponentStopClassBaseList,
            DropdownField addStopDropdown, List<PGStopClassBase> stopClassBaseList, VisualElement visualElementParent, int parentInsertIndex)
        {
            var existingList = visualElementParent.Q<ListView>("stopClassListView");
            if (existingList != null) visualElementParent.Remove(existingList);
            var stopClassListView = new ListView(baseComponentStopClassBaseList);
            stopClassListView.name = "stopClassListView";
            stopClassListView.virtualizationMethod = CollectionVirtualizationMethod.DynamicHeight;
            stopClassListView.PGHideScrollView();

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
                itemLabel.text = "Stop: " + baseComponentStopClassBaseList[i].StopName();
                itemLabel.tooltip = baseComponentStopClassBaseList[i].StopInfo();

                var itemProperty = itemWrapper.Q<PropertyField>("itemProperty");
                var itemRemove = itemWrapper.Q<Button>("itemRemove");

                itemProperty.BindProperty(serializedObject.FindProperty("stopClasses.Array.data[" + i + "]"));
                serializedObject.Update();
                serializedObject.ApplyModifiedProperties();

                itemRemove.RegisterCallback<ClickEvent>(x =>
                {
                    if (baseComponentStopClassBaseList.Count > i) baseComponentStopClassBaseList.RemoveAt(i);
                    serializedObject.Update();
                    serializedObject.ApplyModifiedProperties();
                    EditorUtility.SetDirty(serializedObject.targetObject);
                    DrawStopClass(serializedObject, baseComponentStopClassBaseList, addStopDropdown, stopClassBaseList, visualElementParent,
                        parentInsertIndex);
                });
            }

            stopClassListView.makeItem = MakeItem;
            stopClassListView.bindItem = BindItem;

            if (baseComponentStopClassBaseList.Count <= 0) return;
            if (visualElementParent.childCount >= parentInsertIndex)
                visualElementParent.Insert(parentInsertIndex, stopClassListView);
            else
                visualElementParent.Add(stopClassListView);
        }
    }
}

#endif