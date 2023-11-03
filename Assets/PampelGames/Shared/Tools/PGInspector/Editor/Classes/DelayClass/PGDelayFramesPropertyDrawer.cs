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
    [CustomPropertyDrawer(typeof(PGDelayFrames))]
    public class PGDelayFramesPropertyDrawer : PropertyDrawer
    {
        private SerializedProperty delayFramesProperty;
        private readonly IntegerField delayFrames = new();


        public override VisualElement CreatePropertyGUI(SerializedProperty property)
        {
            var container = new VisualElement();

            FindAndBindProperties(property);
            DrawExecute(property);

            container.Add(delayFrames);
            delayFrames.label = "";
            return container;
        }

        private void FindAndBindProperties(SerializedProperty property)
        {
            delayFramesProperty = property.FindPropertyRelative(nameof(PGDelayFrames.delayFrames));
            delayFrames.BindProperty(delayFramesProperty);
        }


        /********************************************************************************************************************************/

        private void DrawExecute(SerializedProperty property)
        {
            delayFrames.PGClampValue(1);
        }
    }
}
#endif