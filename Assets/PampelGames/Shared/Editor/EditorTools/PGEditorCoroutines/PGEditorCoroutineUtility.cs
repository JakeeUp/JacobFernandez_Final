// ----------------------------------------------------
// Copyright (c) Pampel Games e.K. All Rights Reserved.
// https://www.pampelgames.com
// ----------------------------------------------------

using System.Collections;

namespace PampelGames.Shared.Editor.EditorTools
{
    public static class PGEditorCoroutineUtility
    {
        public static PGEditorCoroutine StartCoroutine(IEnumerator routine)
        {
            var coroutine = new PGEditorCoroutine(routine);
            PGEditorCoroutineManager.AddCoroutine(coroutine);
            return coroutine;
        }
        
        public static void StopCoroutine(PGEditorCoroutine coroutine)
        {
            PGEditorCoroutineManager.StopCoroutine(coroutine);
        }
        
    }
}