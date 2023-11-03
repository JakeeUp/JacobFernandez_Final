// ----------------------------------------------------
// Copyright (c) Pampel Games e.K. All Rights Reserved.
// https://www.pampelgames.com
// ----------------------------------------------------

using System.IO;
using UnityEngine;

namespace PampelGames.Shared.Utility
{
    public static class PGExportUtility
    {
        
        /// <summary>
        ///     Creates a readable Texture2D from a non-readable Texture2D.
        /// </summary>
        public static Texture2D CreateReadableTexture2D(Texture2D original, TextureFormat format = TextureFormat.ARGB32)
        {
            return CreateReadableTexture2DInternal(original, format);
        }
        
        /// <summary>
        ///     Exports a Texture2D to the desktop or a specified path. Texture must be readable.
        /// </summary>
        public static void ExportTexture2D(Texture2D texture, string fileName, string filePath = null)
        {
            ExportTexture2DInternal(texture, fileName, filePath);
        }
        
        
        /********************************************************************************************************************************/
        /********************************************************************************************************************************/

        
        private static Texture2D CreateReadableTexture2DInternal(Texture2D original, TextureFormat format = TextureFormat.ARGB32)
        {
            RenderTexture tmp = RenderTexture.GetTemporary(original.width, original.height);
            Graphics.Blit(original, tmp);
            RenderTexture previous = RenderTexture.active;
            RenderTexture.active = tmp;
            Texture2D readableText = new Texture2D(original.width, original.height, format, false);
            readableText.ReadPixels(new Rect(0, 0, tmp.width, tmp.height), 0, 0);
            readableText.Apply();
            RenderTexture.active = previous;
            RenderTexture.ReleaseTemporary(tmp);
            return readableText;
        }

        private static void ExportTexture2DInternal(Texture2D texture, string fileName, string filePath = null)
        {
            byte[] bytes = texture.EncodeToPNG();
            string exportPath;
            if (string.IsNullOrEmpty(filePath))
                exportPath = System.Environment.GetFolderPath(System.Environment.SpecialFolder.Desktop);
            else
                exportPath = filePath;
            File.WriteAllBytes(Path.Combine(exportPath, $"{fileName}.png"), bytes);
        }
    }
}
