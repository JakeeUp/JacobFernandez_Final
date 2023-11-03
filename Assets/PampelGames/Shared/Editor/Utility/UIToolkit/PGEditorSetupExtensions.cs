// ----------------------------------------------------
// Copyright (c) Pampel Games e.K. All Rights Reserved.
// https://www.pampelgames.com
// ----------------------------------------------------

using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEditor.UIElements;
using UnityEngine;
using UnityEngine.UIElements;

namespace PampelGames.Shared.Editor
{
    public static class PGEditorSetupExtensions
    {
        /// <summary>
        ///     Finds the serialized property and binds it to the element.
        /// </summary>
        public static void PGSetupBindProperty(this IBindable element, SerializedObject serializedObject, string propertyName)
        {
            var property = serializedObject.FindProperty(propertyName);
            if (property != null) element.BindProperty(property);
            else Debug.LogError($"Could not find property: {propertyName}");
        }

        /// <summary>
        ///     Finds the serialized property and binds it to the element. Used for Property Drawer.
        /// </summary>
        public static void PGSetupBindPropertyRelative(this IBindable element, SerializedProperty serializedProperty, string propertyName)
        {
            var property = serializedProperty.FindPropertyRelative(propertyName);
            if (property != null) element.BindProperty(property);
            else Debug.LogError($"Could not find property: {propertyName}");
        }
        
        /// <summary>
        ///     Sets up a ListView with one ObjectField for the type of the collection incl. MakeItem and BindItem.
        /// </summary>
        /// <param name="listView">ListView that will be used to display the collection.</param>
        /// <param name="collectionProperty">SerializedProperty of the underlying collection.</param>
        /// <param name="collection">The underlying collection of items to be displayed in the ObjectFields.</param>
        public static void PGSetupObjectListView(this ListView listView, SerializedProperty collectionProperty, IEnumerable collection)
        {
            listView.showBoundCollectionSize = false; // Important to avoid OutOfRangeException in BindItem.
            listView.itemsSource = collection.Cast<object>().ToList();
            var itemType = collection.GetType().GetGenericArguments()[0];

            listView.makeItem = () =>
            {
                var item = new VisualElement();

                var objectField = new ObjectField
                {
                    objectType = itemType
                };

                item.Add(objectField);
                return item;
            };

            listView.bindItem = (element, index) =>
            {
                if (index >= collectionProperty.arraySize) return;
                var field = element.Q<ObjectField>();
                field.BindProperty(collectionProperty.GetArrayElementAtIndex(index));
            };
        }
        
        /// <summary>
        /// Sets up a ListView with ObjectFields bound to a given list of Unity Objects.
        /// </summary>
        /// <param name="listView">ListView to populate</param>
        /// <param name="objectList">List of Unity Objects</param>
        public static void PGSetupObjectListViewEditor<T>(this ListView listView, List<T> objectList) where T : Object
        {
            listView.showBoundCollectionSize = false;
            listView.itemsSource = objectList;

            listView.makeItem = () =>
            {
                var item = new VisualElement();
                var objectField = new ObjectField
                {
                    objectType = typeof(T)
                };
        
                item.Add(objectField);
                return item;
            };

            listView.bindItem = (element, i) =>
            {
                var field = element.Q<ObjectField>();
                field.value = objectList[i];

                // move the registration to bindItem
                field.RegisterValueChangedCallback(evt =>
                {
                    objectList[i] = evt.newValue as T;
                });
            };
        }
        
    }
}