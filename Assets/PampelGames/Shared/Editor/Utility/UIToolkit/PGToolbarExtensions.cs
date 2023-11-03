// ----------------------------------------------------
// Copyright (c) Pampel Games e.K. All Rights Reserved.
// https://www.pampelgames.com
// ----------------------------------------------------

using System.Linq;
using UnityEditor.UIElements;
using UnityEngine;
using UnityEngine.UIElements;

namespace PampelGames.Shared.Editor
{
    public static class PGToolbarExtensions
    {
        /// <summary>
        ///     Hides the cavet (arrow) from the ToolbarMenu.
        /// </summary>
        /// <param name="centerText">Center the text of the TextElement.</param>
        /// <param name="hideText">Hide the text of the TextElement. Used when no text is set.</param>
        public static void PGRemoveMenuArrow(this ToolbarMenu toolbarMenu, bool centerText, bool hideText)
        {
            var toolbarText = toolbarMenu.Children().ToList()[0];
            if (centerText) toolbarText.style.unityTextAlign = TextAnchor.MiddleCenter;
            if (hideText) toolbarText.style.display = DisplayStyle.None;
            var toolbarArrow = toolbarMenu.Children().ToList()[1];
            toolbarArrow.style.display = DisplayStyle.None;
        }
    }
}