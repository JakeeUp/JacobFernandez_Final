// ----------------------------------------------------
// Copyright (c) Pampel Games e.K. All Rights Reserved.
// https://www.pampelgames.com
// ----------------------------------------------------

using UnityEditor;
using UnityEngine;


namespace PampelGames.Shared.Editor
{
    // [InitializeOnLoad]
    public class PGHierarchyIconDisplay
    {
        static PGHierarchyIconDisplay()
        {
            EditorApplication.hierarchyWindowItemOnGUI += HandleHierarchyWindowItemOnGUI;
        }
    
        private static void HandleHierarchyWindowItemOnGUI(int instanceID, Rect selectionRect)
        {
            var gameObject = EditorUtility.InstanceIDToObject(instanceID) as GameObject;

            if (gameObject == null) return;  
        
            if (gameObject.GetComponent<Transform>())  
            {
                Rect r = new Rect(selectionRect); 
                r.x = r.width + 10;  
            
                GUI.Label(r, IconContent);  
            }
        
        }
    
        static GUIContent IconContent
        {
            get
            {
                var texture = EditorGUIUtility.ObjectContent(null, typeof(ParticleSystem)).image as Texture2D;
                return new GUIContent(texture);
            }
        }
    }
}