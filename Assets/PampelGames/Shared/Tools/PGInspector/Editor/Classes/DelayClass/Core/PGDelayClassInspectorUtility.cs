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
    ///     Static helper methods for MonoBehaviour Inspectors that implement <see cref="PGDelayClass" />.
    /// </summary>
    public static class PGDelayClassInspectorUtility
    {
        /* Public *******************************************************************************************************************************/

        /// <summary>
        ///     Adds all <see cref="PGDelayClassBase" /> types to the respective Editor lists.
        /// </summary>
        /// <param name="assemblies">Use AppDomain.CurrentDomain.GetAssemblies().</param>
        /// <param name="delayClassBaseList">List the inspector must create.</param>
        /// <param name="delayClassBaseStringList">Respective string list the inspector must create.</param>
        public static void AddDelayClassTypes(Assembly[] assemblies, List<PGDelayClassBase> delayClassBaseList, List<string> delayClassBaseStringList)
        {
            var instances = PGClassUtility.CreateInstances<PGDelayClassBase>(assemblies);
            foreach (var instance in instances) delayClassBaseList.Add(instance);
            foreach (var delayClassBase in delayClassBaseList) delayClassBaseStringList.Add(delayClassBase.DelayName());
        }


        /// <summary>
        ///     Draw list of <see cref="PGDelayClassBase" /> in the inspector of the implementing class. Must be named "delayClasses".
        /// </summary>
        /// <param name="serializedObject">SerializedObject of the inspector.</param>
        /// <param name="baseComponentDelayClassBaseList">List of <see cref="PGDelayClassBase" /> of the base component.</param>
        /// <param name="addDelayDropdown">Dropdown created by the UXML, showing the string names in the inspector. Usually child of a button.</param>
        /// <param name="delayClassBaseList">
        ///     List with all <see cref="PGDelayClassBase" /> in all assemblies the inspector must create. Fill it with
        ///     <see cref="PGDelayClassInspectorUtility.AddDelayClassTypes" />
        /// </param>
        /// <param name="visualElementParent">Parent element the implementation should be added to.</param>
        /// <param name="parentInsertIndex">Index of the parent element the implementation should be inserted.</param>
        public static void DrawDelayClass(SerializedObject serializedObject, List<PGDelayClassBase> baseComponentDelayClassBaseList,
            DropdownField addDelayDropdown, List<PGDelayClassBase> delayClassBaseList, VisualElement visualElementParent, int parentInsertIndex)
        {
            addDelayDropdown.RegisterCallback<ChangeEvent<string>>(evt =>
            {
                if (addDelayDropdown.index >= 0)
                {
                    var canAdd = true;
                    if (baseComponentDelayClassBaseList.Count > 0)
                    {
                        canAdd = false;
                        EditorUtility.DisplayDialog("Invalid Item", "Delay has allready been set!", "Ok");
                    }

                    if (canAdd)
                    {
                        baseComponentDelayClassBaseList.Add(
                            (PGDelayClassBase) Activator.CreateInstance(delayClassBaseList[addDelayDropdown.index].GetType()));
                        EditorUtility.SetDirty(serializedObject.targetObject);
                    }
                }

                addDelayDropdown.value = string.Empty;

                serializedObject.Update();
                serializedObject.ApplyModifiedProperties();
                DrawDelayClassListView(serializedObject, baseComponentDelayClassBaseList, addDelayDropdown, delayClassBaseList, visualElementParent,
                    parentInsertIndex);
            });
            DrawDelayClassListView(serializedObject, baseComponentDelayClassBaseList, addDelayDropdown, delayClassBaseList, visualElementParent,
                parentInsertIndex);
        }

        public static void RemoveExistingListView(VisualElement visualElementParent)
        {
            var existingList = visualElementParent.Q<ListView>("delayClassListView");
            if (existingList != null) visualElementParent.Remove(existingList);
        }


        /********************************************************************************************************************************/


        private static void DrawDelayClassListView(SerializedObject serializedObject, List<PGDelayClassBase> baseComponentDelayClassBaseList,
            DropdownField addDelayDropdown, List<PGDelayClassBase> delayClassBaseList, VisualElement visualElementParent, int parentInsertIndex)
        {
            var existingList = visualElementParent.Q<ListView>("delayClassListView");
            if (existingList != null) visualElementParent.Remove(existingList);
            var delayClassListView = new ListView(baseComponentDelayClassBaseList);
            delayClassListView.name = "delayClassListView";
            delayClassListView.virtualizationMethod = CollectionVirtualizationMethod.DynamicHeight;

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
                itemLabel.text = "Delay: " + baseComponentDelayClassBaseList[i].DelayName();
                itemLabel.tooltip = baseComponentDelayClassBaseList[i].DelayInfo();

                var itemProperty = itemWrapper.Q<PropertyField>("itemProperty");
                var itemRemove = itemWrapper.Q<Button>("itemRemove");

                itemProperty.BindProperty(serializedObject.FindProperty("delayClasses.Array.data[" + i + "]"));
                serializedObject.Update();
                serializedObject.ApplyModifiedProperties();

                itemRemove.RegisterCallback<ClickEvent>(x =>
                {
                    if (baseComponentDelayClassBaseList.Count > i) baseComponentDelayClassBaseList.RemoveAt(i);
                    serializedObject.Update();
                    serializedObject.ApplyModifiedProperties();
                    EditorUtility.SetDirty(serializedObject.targetObject);
                    DrawDelayClass(serializedObject, baseComponentDelayClassBaseList, addDelayDropdown, delayClassBaseList, visualElementParent,
                        parentInsertIndex);
                });
            }

            delayClassListView.makeItem = MakeItem;
            delayClassListView.bindItem = BindItem;

            if (baseComponentDelayClassBaseList.Count <= 0) return;
            if (visualElementParent.childCount >= parentInsertIndex)
                visualElementParent.Insert(parentInsertIndex, delayClassListView);
            else
                visualElementParent.Add(delayClassListView);
        }
    }
}

#endif