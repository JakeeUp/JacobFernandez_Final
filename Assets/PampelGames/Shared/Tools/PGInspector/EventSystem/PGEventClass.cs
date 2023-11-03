// ----------------------------------------------------
// Copyright (c) Pampel Games e.K. All Rights Reserved.
// https://www.pampelgames.com
// ----------------------------------------------------

using System;
using UnityEngine.Events;

namespace PampelGames.Shared.Tools.PGInspector
{
    [Serializable]
    public class PGEventClass
    {
        public bool activated;
        public UnityEvent unityEvent;
    }
}
