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
    [CustomPropertyDrawer(typeof(PGDelaySeconds))]
    public class PGDelaySecondsPropertyDrawer : PropertyDrawer
    {
        private SerializedProperty delaySecondsProperty;
        private readonly FloatField delaySeconds = new();


        public override VisualElement CreatePropertyGUI(SerializedProperty property)
        {
            var container = new VisualElement();

            FindAndBindProperties(property);
            DrawExecute(property);

            container.Add(delaySeconds);
            delaySeconds.label = "";
            return container;
        }

        private void FindAndBindProperties(SerializedProperty property)
        {
            delaySecondsProperty = property.FindPropertyRelative(nameof(PGDelaySeconds.delaySeconds));
            delaySeconds.BindProperty(delaySecondsProperty);
        }


        /********************************************************************************************************************************/

        private void DrawExecute(SerializedProperty property)
        {
            delaySeconds.PGClampValue();
        }
    }
}
#endif