// ----------------------------------------------------
// Copyright (c) Pampel Games e.K. All Rights Reserved.
// https://www.pampelgames.com
// ----------------------------------------------------

#if UNITY_EDITOR
using System;
using System.Linq;
using UnityEditor;
using UnityEngine;

namespace PampelGames.Shared.Tools.Editor
{
    public static class PGInspectorEditorUtility
    {
        /// <summary>
        ///      Get the target object that is drawing the Property Drawer.
        /// </summary>
        public static T GetTargetObject<T>(SerializedProperty property) where T: Component
        {
            var component = (T) property.serializedObject.targetObject;
            return component;
        }
        
        /// <summary>
        /// If the class is drawn in a list, gets the index.
        /// </summary>
        public static int GetDrawingListIndex(SerializedProperty property)
        {
            var index = Convert.ToInt32(new string(property.propertyPath.Where(char.IsDigit).ToArray()));
            return index;
        }
    }
}
#endif
