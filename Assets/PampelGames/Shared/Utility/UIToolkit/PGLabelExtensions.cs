// ---------------------------------------------------
// Copyright (c) Pampel Games e.K. All Rights Reserved.
// https://www.pampelgames.com
// ---------------------------------------------------

using UnityEngine;
using UnityEngine.UIElements;

namespace PampelGames.Shared.Utility
{
    /// <summary>
    ///     See also <see cref="PGVisualElementExtensions"/> for header-wrapper styles.
    /// </summary>
    public static class PGLabelExtensions
    {

        /// <summary>
        ///     Small header for a section.
        /// </summary>
        public static void PGHeaderSmall(this Label label)
        {
            label.PGPadding(0,0,4,4);
            label.style.unityFontStyleAndWeight = new StyleEnum<FontStyle>(FontStyle.Bold);
        }
        
        

        
    }
}