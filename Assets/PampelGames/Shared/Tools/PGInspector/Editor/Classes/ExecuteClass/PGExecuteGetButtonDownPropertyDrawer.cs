// ---------------------------------------------------
// Copyright (c) Pampel Games e.K. All Rights Reserved.
// https://www.pampelgames.com
// ---------------------------------------------------

#if UNITY_EDITOR
using UnityEditor;
using UnityEditor.UIElements;
using UnityEngine.UIElements;

namespace PampelGames.Shared.Tools.PGInspector.Editor
{
    [CustomPropertyDrawer(typeof(PGExecuteGetButtonDown))]
    public class PGExecuteGetButtonDownPropertyDrawer : PropertyDrawer
    {
        private SerializedProperty buttonNameProperty;
        private readonly TextField buttonName = new();


        public override VisualElement CreatePropertyGUI(SerializedProperty property)
        {
            var container = new VisualElement();

            FindAndBindProperties(property);
            DrawExecute(property);

            container.Add(buttonName);
            buttonName.label = "";
            return container;
        }

        private void FindAndBindProperties(SerializedProperty property)
        {
            buttonNameProperty = property.FindPropertyRelative(nameof(PGExecuteGetButtonDown.buttonName));
            buttonName.BindProperty(buttonNameProperty);
        }


        /********************************************************************************************************************************/

        private void DrawExecute(SerializedProperty property)
        {
        }
    }
}
#endif