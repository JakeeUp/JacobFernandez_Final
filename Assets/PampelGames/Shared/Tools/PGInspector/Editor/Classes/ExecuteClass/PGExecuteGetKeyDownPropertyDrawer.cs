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
    [CustomPropertyDrawer(typeof(PGExecuteGetKeyDown))]
    public class PGExecuteGetKeyDownPropertyDrawer : PropertyDrawer
    {
        private SerializedProperty keyCodeProperty;
        private readonly PropertyField keyCode = new();


        public override VisualElement CreatePropertyGUI(SerializedProperty property)
        {
            var container = new VisualElement();

            FindAndBindProperties(property);
            DrawExecute(property);

            container.Add(keyCode);
            keyCode.label = "";
            return container;
        }

        private void FindAndBindProperties(SerializedProperty property)
        {
            keyCodeProperty = property.FindPropertyRelative(nameof(PGExecuteGetKeyDown.keyCode));
            keyCode.BindProperty(keyCodeProperty);
        }


        /********************************************************************************************************************************/

        private void DrawExecute(SerializedProperty property)
        {
        }
    }
}
#endif