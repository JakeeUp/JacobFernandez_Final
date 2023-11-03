// ---------------------------------------------------
// Copyright (c) Pampel Games e.K. All Rights Reserved.
// https://www.pampelgames.com
// ---------------------------------------------------

#if UNITY_EDITOR
using UnityEditor;
using UnityEngine.UIElements;

namespace PampelGames.Shared.Tools.PGInspector.Editor
{
    [CustomPropertyDrawer(typeof(PGStopOnBecameVisible))]
    public class PGStopOnBecameVisiblePropertyDrawer : PropertyDrawer
    {
        public override VisualElement CreatePropertyGUI(SerializedProperty property)
        {
            var container = new VisualElement();

            CreateAndBindProperties(property);
            DrawStop(property);

            return container;
        }

        private void CreateAndBindProperties(SerializedProperty property)
        {
        }


        /********************************************************************************************************************************/

        private void DrawStop(SerializedProperty property)
        {
        }
    }
}
#endif