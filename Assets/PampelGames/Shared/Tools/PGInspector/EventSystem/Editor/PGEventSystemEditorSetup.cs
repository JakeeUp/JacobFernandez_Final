// ----------------------------------------------------
// Copyright (c) Pampel Games e.K. All Rights Reserved.
// https://www.pampelgames.com
// ----------------------------------------------------

#if UNITY_EDITOR
using PampelGames.Shared.Utility;
using UnityEditor;
using UnityEditor.UIElements;
using UnityEngine.UIElements;

namespace PampelGames.Shared.Tools.PGInspector.Editor
{
    public static class PGEventSystemEditorSetup
    {
        public static void DrawEventClass(string eventClassName, PGEventClass eventClass, SerializedObject serializedObject,
            ToolbarMenu toolbarMenu, VisualElement ParentElement)
        {
            var labelEventName = char.ToUpper(eventClassName[0]) + eventClassName.Substring(1);
            labelEventName = labelEventName.Replace("Class", "");

            toolbarMenu.menu.AppendAction(labelEventName, action =>
            {
                eventClass.activated = true;
                EditorUtility.SetDirty(serializedObject.targetObject);
                SetUnityEventDisplay(eventClassName, eventClass.activated, ParentElement);
            });

            CreateUnityEventWrapper(eventClassName, eventClass, serializedObject, ParentElement);
            SetUnityEventDisplay(eventClassName, eventClass.activated, ParentElement);
        }

        private static void SetUnityEventDisplay(string eventClassName, bool display, VisualElement ParentElement)
        {
            var UnityEventWrapper = ParentElement.Q<VisualElement>("UnityEvent_" + eventClassName);
            UnityEventWrapper?.PGDisplayStyleFlex(display);
        }

        private static void CreateUnityEventWrapper(string eventClassName, PGEventClass eventClass, SerializedObject serializedObject,
            VisualElement ParentElement)
        {
            var UnityEventWrapper = ParentElement.Q<VisualElement>("UnityEvent_" + eventClassName);
            if (UnityEventWrapper != null) return;

            UnityEventWrapper = new VisualElement();
            UnityEventWrapper.name = "UnityEvent_" + eventClassName;
            PropertyField eventProperty = new();
            var labelEventName = char.ToUpper(eventClassName[0]) + eventClassName.Substring(1);
            labelEventName = labelEventName.Replace("Class", "");
            eventProperty.label = labelEventName;

            eventProperty.style.flexGrow = 1f;
            VisualElement RemoveButtonWrapper = new();
            Button removeEventButton = new();
            UnityEventWrapper.style.flexDirection = FlexDirection.Row;

            var eventClassProperty = serializedObject.FindProperty(eventClassName);
            var eventSerializedProperty = eventClassProperty.FindPropertyRelative(nameof(PGEventClass.unityEvent));
            eventProperty.BindProperty(eventSerializedProperty);

            removeEventButton.PGSetupRemoveButton();
            removeEventButton.clicked += () =>
            {
                UnityEventWrapper.style.display = DisplayStyle.None;
                eventClass.activated = false;
                EditorUtility.SetDirty(serializedObject.targetObject);
                SetUnityEventDisplay(eventClassName, eventClass.activated, ParentElement);
            };

            UnityEventWrapper.Add(eventProperty);
            RemoveButtonWrapper.Add(removeEventButton);
            UnityEventWrapper.Add(RemoveButtonWrapper);

            ParentElement.Add(UnityEventWrapper);
        }
    }
}
#endif