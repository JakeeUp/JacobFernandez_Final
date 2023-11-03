// ----------------------------------------------------
// Copyright (c) Pampel Games e.K. All Rights Reserved.
// https://www.pampelgames.com
// ----------------------------------------------------

using System;
using UnityEngine;

namespace PampelGames.Shared.Editor.EditorTools
{
    public class PGEditorSchedulerDescr
    {
        internal float duration;
        internal float currentTime;
        internal bool completed;
        
        public Action onComplete;
        
        
    }
}