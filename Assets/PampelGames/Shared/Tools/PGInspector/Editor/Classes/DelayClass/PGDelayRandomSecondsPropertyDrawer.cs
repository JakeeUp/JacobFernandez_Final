// ---------------------------------------------------
// Copyright (c) Pampel Games e.K. All Rights Reserved.
// https://www.pampelgames.com
// ---------------------------------------------------

#if UNITY_EDITOR
using UnityEditor;
using UnityEditor.UIElements;
using UnityEngine;
using UnityEngine.UIElements;

namespace PampelGames.Shared.Tools.PGInspector.Editor
{
    [CustomPropertyDrawer(typeof(PGDelayRandomSeconds))]
    public class PGDelayRandomSecondsPropertyDrawer : PropertyDrawer
    {
        private SerializedProperty delayRandomSecondsProperty;
        private readonly Vector2Field delayRandomSeconds = new();


        public override VisualElement CreatePropertyGUI(SerializedProperty property)
        {
            var container = new VisualElement();

            FindAndBindProperties(property);
            DrawExecute(property);

            container.Add(delayRandomSeconds);
            delayRandomSeconds.label = "";
            return container;
        }

        private void FindAndBindProperties(SerializedProperty property)
        {
            delayRandomSecondsProperty = property.FindPropertyRelative(nameof(PGDelayRandomSeconds.delayRandomSeconds));
            delayRandomSeconds.BindProperty(delayRandomSecondsProperty);
        }


        /********************************************************************************************************************************/

        private void DrawExecute(SerializedProperty property)
        {
            delayRandomSeconds.PGClampValue(0, Mathf.Infinity, true);
        }
    }
}
#endif