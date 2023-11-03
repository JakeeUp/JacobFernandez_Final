// ----------------------------------------------------
// Copyright (c) Pampel Games e.K. All Rights Reserved.
// https://www.pampelgames.com
// ----------------------------------------------------

using System.Collections.Generic;
using UnityEngine;

namespace PampelGames.Shared.Editor.EditorTools
{
    public static class PGEditorTweenManager
    {

        private static HashSet<object> activeObjects = new();
        

        public static bool InitializeTweenedObject(object obj)
        {
            if (activeObjects.Contains(obj)) return false;
            activeObjects.Add(obj);
            return true;
        }

        public static void RemoveTweenedObject(object obj)
        {
            if (!activeObjects.Contains(obj)) return;
            activeObjects.Remove(obj);
        }
    }
}
