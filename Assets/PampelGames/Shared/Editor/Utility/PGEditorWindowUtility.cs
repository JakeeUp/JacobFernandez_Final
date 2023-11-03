// ----------------------------------------------------
// Copyright (c) Pampel Games e.K. All Rights Reserved.
// https://www.pampelgames.com
// ----------------------------------------------------

using System;
using PampelGames.Shared.Utility;
using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;

namespace PampelGames.Shared.Editor
{
    
    public static class PGEditorWindowUtility
    {
        
        /// <summary>
        ///     Tries to get an open Editor window.
        /// </summary>
        public static T GetWindow<T>() where T : EditorWindow 
        {
            T window = null;
            foreach (var w in Resources.FindObjectsOfTypeAll<T>()) {
                if (w != null) {
                    window = w;
                    break;
                }
            }
            
            return window;
        }


        public static void PGSetWindowSize(this EditorWindow window, int width, int height)
        {
            var windowPosition = window.position;
            windowPosition.width = width;
            windowPosition.height = height;
            window.position = windowPosition; 
        }
        
        /// <summary>
        ///     Centers the editor window to the Unity Editor.
        /// </summary>
        public static void PGCenterOnMainWindow(this EditorWindow window)
        {
            Rect main = EditorGUIUtility.GetMainWindowPosition();
            Rect pos = window.position;
            float centerWidth = (main.width - pos.width) * 0.5f;
            float centerHeight = (main.height - pos.height) * 0.5f;
            pos.x = main.x + centerWidth;
            pos.y = main.y + centerHeight;
            window.position = pos;
        }
    }
}