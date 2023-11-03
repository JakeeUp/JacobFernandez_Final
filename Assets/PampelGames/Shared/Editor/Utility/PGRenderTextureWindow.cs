// ----------------------------------------------------
// Copyright (c) Pampel Games e.K. All Rights Reserved.
// https://www.pampelgames.com
// ----------------------------------------------------

using PampelGames.Shared.Utility;
using UnityEditor;
using UnityEngine;


/*

♥ Create Render Texture

♥ Copy Camera

♥ Change the Clear Flags property to Solid Color. Set Color to Transparent.
(In HDRP, clear flags is "Background Type")

♥ Uncomment the MenuItem below to open the window.

 */


namespace PampelGames.Shared.Editor
{
    public class PGRenderTextureWindow : EditorWindow
    {
        public Camera captureCamera; // camera reference
        public int resolution = 256;

        [MenuItem("Window/Capture Texture")]
        public static void ShowWindow()
        {
            // Show existing window instance. If one doesn't exist, make one.
            PGRenderTextureWindow window = (PGRenderTextureWindow)GetWindow(typeof(PGRenderTextureWindow));
            window.Show();
        }

        void OnGUI()
        {
            // allow selection of camera and rendertexture in editor window
            captureCamera = (Camera)EditorGUILayout.ObjectField("Camera", captureCamera, typeof(Camera), true);
            resolution = EditorGUILayout.IntField("Resolution", resolution);
            
            if(GUILayout.Button("Capture Texture"))
            {
                CaptureToTexture();
            }
        }

        void CaptureToTexture()
        {
            // create new 2D texture with dimensions of RenderTexture
            Texture2D captureTexture = PGRenderTextureUtility.CaptureToTexture(captureCamera, resolution);
   
            // convert Texture2D to PNG
            byte[] bytes = captureTexture.EncodeToPNG();
   
            // save PNG to a file
            string path = EditorUtility.SaveFilePanel("Save Texture As PNG", "", "screenshot.png", "png");
            if (path.Length != 0)
            {
                System.IO.File.WriteAllBytes(path, bytes);
            }

        }
    }
}