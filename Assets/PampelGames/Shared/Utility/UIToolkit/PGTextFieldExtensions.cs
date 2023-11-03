// ----------------------------------------------------
// Copyright (c) Pampel Games e.K. All Rights Reserved.
// https://www.pampelgames.com
// ----------------------------------------------------

using UnityEngine;
using UnityEngine.UIElements;

namespace PampelGames.Shared.Utility
{
    public static class PGTextFieldExtensions
    {
        /// <summary>
        ///     Sets the TextField to multiline and WhiteSpace.Normal so it will wrap when necessary.
        /// </summary>
        public static void PGWrapText(this TextField textField)
        {
            textField.multiline = true;
            textField.style.whiteSpace = WhiteSpace.Normal;
        }
        
        /// <summary>
        ///     Set the background color of the text input field.
        /// </summary>
        public static void PGTextInputColor(this TextField textField, Color color)
        {
            var textInput = textField.Q<VisualElement>(PGConstantsUSS.TextInputField);
            textInput.style.backgroundColor = new StyleColor(color);
        }
    }
}