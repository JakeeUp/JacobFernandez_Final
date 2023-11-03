// ----------------------------------------------------
// Copyright (c) Pampel Games e.K. All Rights Reserved.
// https://www.pampelgames.com
// ----------------------------------------------------

using PampelGames.Shared.Editor.EditorTools;
using PampelGames.Shared.Utility;
using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;

namespace PampelGames.Shared.Editor
{
    public static class PGEditorMessages
    {
        /// <summary>
        ///     Displays a modal dialog with
        ///     <see cref="UnityEditor.EditorUtility.DisplayDialog(string, string, string, string)">EditorUtility.DisplayDialog</see>.
        /// </summary>
        public static void PopUpMessage(string title, string message)
        {
            EditorUtility.DisplayDialog(title, message, "Ok");
        }


        /// <summary>
        ///     Creates a temporary <see cref="EditorWindow" /> on the center screen.
        /// </summary>
        public static void WindowMessage(string message, float duration = 2f)
        {
            // var popup = EditorWindow.GetWindow<Popup>(true);
            // if (popup != null) popup.Close();

            Popup.labelText = message;

            var popup = ScriptableObject.CreateInstance<Popup>();
            popup.PGSetWindowSize(400, 200);
            popup.PGCenterOnMainWindow();
            popup.ShowPopup();

            PGEditorScheduler.ScheduleTime(2f, () => ClosePopup(popup));
        }

        /********************************************************************************************************************************/


        private static void ClosePopup(Popup popup)
        {
            if (popup == null) return;
            popup.Close();
        }
    }

    internal class Popup : EditorWindow
    {
        public static string labelText;
        public VisualElement root;


        private void OnEnable()
        {
            root = rootVisualElement;
            root.style.alignItems = Align.Center;
            root.style.justifyContent = Justify.Center;
            root.style.backgroundColor = PGColors.ReadOnlyBackground();
            root.PGBorderWidth(3);
            root.PGBorderColor(PGColors.CustomBorder());

            var label = new Label(labelText);
            label.style.alignSelf = Align.Center;
            label.PGBoldText();
            label.style.fontSize = 40f;


            root.Add(label);
        }
    }
}