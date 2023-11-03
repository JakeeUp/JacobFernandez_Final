// ----------------------------------------------------
// Copyright (c) Pampel Games e.K. All Rights Reserved.
// https://www.pampelgames.com
// ----------------------------------------------------

using System.Collections;

namespace PampelGames.Shared.Editor.EditorTools
{
    public class PGEditorCoroutine
    {
        private readonly IEnumerator routine;
        private bool IsDone { get; set; }

        internal PGEditorCoroutine(IEnumerator enumerator)
        {
            routine = enumerator;
        }

        internal bool MoveNext()
        {
            IsDone = !routine.MoveNext();
            return !IsDone;
        }
    }
}
