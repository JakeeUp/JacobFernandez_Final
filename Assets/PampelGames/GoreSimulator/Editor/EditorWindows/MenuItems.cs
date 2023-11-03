// ----------------------------------------------------
// Gore Simulator
// Copyright (c) Pampel Games e.K. All Rights Reserved.
// https://www.pampelgames.com
// ----------------------------------------------------

using PampelGames.Shared.Editor;
using UnityEditor;
using UnityEngine;

namespace PampelGames.GoreSimulator.Editor
{
    internal static class MenuItems
    {
        [MenuItem("Tools/Pampel Games/Gore Simulator/Global Settings")]
        internal static void OpenGlobalSettings()
        {
            var window = EditorWindow.GetWindow<GlobalSettings>();
            
            window.PGSetWindowSize(400, 300);
            window.PGCenterOnMainWindow();

            window.titleContent = new GUIContent("Gore Simulator");
            window.Show();
        }
        
        [MenuItem("Tools/Pampel Games/Gore Simulator/Combine Skinned Meshes")]
        internal static void OpenCombineSkinnedMeshes()
        {
            var window = EditorWindow.GetWindow<CombineSkinnedMeshes>();
            
            window.PGSetWindowSize(400, 300);
            window.PGCenterOnMainWindow();

            window.titleContent = new GUIContent("Gore Simulator");
            window.Show();
        }
        
    }
}