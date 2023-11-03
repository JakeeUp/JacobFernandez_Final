// ----------------------------------------------------
// Copyright (c) Pampel Games e.K. All Rights Reserved.
// https://www.pampelgames.com
// ----------------------------------------------------

#if UNITY_EDITOR
using System.IO;
using UnityEngine;

namespace PampelGames.Shared.Tools.Editor
{
    public class PGTweenDocumentationEditor : UnityEditor.Editor {
	
		
        public static void OpenEasingTypes()
        {
            string[] files = Directory.GetFiles(Application.dataPath, "PGTweenEasingTypes.html", SearchOption.AllDirectories);
            if (files.Length > 0)
            {
                string filePath = files[0];
                Application.OpenURL("file://" + filePath);
            }
            else
            {
                Debug.LogError("HTML file not found in the project folder.");
            }
        }
		
    }
}
#endif