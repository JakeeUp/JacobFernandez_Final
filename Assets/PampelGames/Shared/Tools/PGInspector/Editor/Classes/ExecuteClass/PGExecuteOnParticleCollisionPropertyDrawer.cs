﻿// ---------------------------------------------------
// Copyright (c) Pampel Games e.K. All Rights Reserved.
// https://www.pampelgames.com
// ---------------------------------------------------

#if UNITY_EDITOR
using UnityEditor;
using UnityEngine.UIElements;

namespace PampelGames.Shared.Tools.PGInspector.Editor
{
    [CustomPropertyDrawer(typeof(PGExecuteOnParticleCollision))]
    public class PGExecuteOnParticleCollisionPropertyDrawer : PropertyDrawer
    {
        public override VisualElement CreatePropertyGUI(SerializedProperty property)
        {
            var container = new VisualElement();

            CreateAndBindProperties(property);
            DrawExecute(property);

            return container;
        }

        private void CreateAndBindProperties(SerializedProperty property)
        {
        }


        /********************************************************************************************************************************/

        private void DrawExecute(SerializedProperty property)
        {
        }
    }
}
#endif