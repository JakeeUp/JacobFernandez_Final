// ----------------------------------------------------
// Copyright (c) Pampel Games e.K. All Rights Reserved.
// https://www.pampelgames.com
// ----------------------------------------------------

using UnityEngine;

namespace PampelGames.Shared.Utility
{
    public static class PGColors
    {

        /// <summary>
        /// Background color in the inspector.
        /// </summary>
        public static Color InspectorBackground()
        {
            return new Color32(56, 56, 56, 255);
        }
        
        /// <summary>
        /// Inspector Button. E.g. the question mark next to the script name.
        /// </summary>
        public static Color InspectorButton()
        {
            return new Color32(196, 196, 196, 255);
        }

        /// <summary>
        /// Standard color of properties in the inspector.
        /// </summary>
        public static Color InspectorVariableText()
        {
            return new Color32(190, 190, 190, 255);
        }
        
        /// <summary>
        /// Standard color of script header in the inspector.
        /// </summary>
        public static Color InspectorHeaderText()
        {
            return new Color32(210, 210, 210, 255);
        }
        
        
        /// <summary>
        /// Border of e.g. Unity Events (1px).
        /// </summary>
        public static Color InspectorBorder()
        {
            return new Color32(36, 36, 36, 255);
        }
        
        public static Color TextColor()
        {
            return new Color32(27, 27, 27, 255);
        }
        public static Color CustomBorder()
        {
            return new Color32(26, 26, 26, 255);
        }
        
        /// <summary>
        /// A bit darker than the background, used by Unity Events.
        /// </summary>
        public static Color UnityEventHeader()
        {
            return new Color32(53, 53, 53, 255);
        }
        
        /// <summary>
        /// A bit lighter than the background, used by Unity Events.
        /// </summary>
        public static Color UnityEventBackground()
        {
            return new Color32(65, 65, 65, 255);
        }
        
        public static Color ListViewBackground()
        {
            return new Color32(70, 70, 70, 255);
        }
        
        public static Color ListViewHover()
        {
            return new Color32(47, 47, 47, 255);
        }
        
        /// <summary>
        /// The blue color in Unity 2021 LTS on the top scene view toolbar. Used as background button hover color.
        /// </summary>
        public static Color UnitySceneHeaderBlue()
        {
            return new Color32(70, 96, 124, 255);
        }
        
        public static Color ButtonBackground()
        {
            return new Color32(88, 88, 88, 255);
        }
        
        public static Color ButtonText()
        {
            return new Color32(238, 238, 238, 255);
        }
        
        public static Color HoverButtonGreen()
        {
            return new Color32(38, 252, 88, 125);
        }
        public static Color HoverButtonRed()
        {
            return new Color32(255, 0, 0, 125);
        }
        public static Color WarningYellow()
        {
            return new Color32(255, 193, 7, 255);
        }
        
        public static Color ReadOnlyText()
        {
            return new Color32(120, 120, 120, 255);
        }
        
        public static Color InputFieldBackground()
        {
            return new Color32(42, 42, 42, 255);
        }
        public static Color ReadOnlyBackground()
        {
            return new Color32(48, 48, 48, 255);
        }

        /// <summary>
        /// The blue background color that appears behind the Play and Pause buttons on top of the scene view.
        /// </summary>
        public static Color ScenePlayPauseBlue()
        {
            return new Color32(32, 67, 99, 255);
        }
        
        public static Color FantasyTreeCreatorGreen()
        {
            return new Color32(0, 50, 0, 255);
        }
        
    }
}
