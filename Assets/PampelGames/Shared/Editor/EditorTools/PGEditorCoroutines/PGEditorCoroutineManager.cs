// ----------------------------------------------------
// Copyright (c) Pampel Games e.K. All Rights Reserved.
// https://www.pampelgames.com
// ----------------------------------------------------

using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace PampelGames.Shared.Editor.EditorTools
{
    internal static class PGEditorCoroutineManager
    {
        private static readonly List<PGEditorCoroutine> editorCoroutines = new();
        
        static PGEditorCoroutineManager()
        {
            EditorApplication.update += Update;
        }

        internal static void AddCoroutine(PGEditorCoroutine editorCoroutine)
        {
            editorCoroutines.Add(editorCoroutine);
        }
        
        internal static void StopCoroutine(PGEditorCoroutine editorCoroutine)
        {
            editorCoroutines.Remove(editorCoroutine);
        }
        
        private static void Update()
        {
            for(int i = editorCoroutines.Count - 1; i >= 0; --i)
            {
                var coroutine = editorCoroutines[i];
                if(!coroutine.MoveNext())
                {
                    editorCoroutines.RemoveAt(i);
                }
            }
        }

 
      
    }
}
