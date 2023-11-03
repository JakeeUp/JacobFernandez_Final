// ----------------------------------------------------
// Copyright (c) Pampel Games e.K. All Rights Reserved.
// https://www.pampelgames.com
// ----------------------------------------------------

using System;
using UnityEngine;

namespace PampelGames.Shared.Tools.PGInspector
{
    public class PGStopOnBecameVisible : PGStopClassBase
    {
        /* Editor Virtual******************************************************************************************************************/

#if UNITY_EDITOR
        public override string StopName()
        {
            return "On Became Visible";
        }
        public override string StopInfo()
        {
            return "Stops when an attached renderer became visible by any camera.";
        }
#endif

        public override void ComponentOnBecameVisible(MonoBehaviour baseComponent, Action StopAction)
        {
            base.ComponentOnBecameVisible(baseComponent, StopAction);
            StopAction();
        }
        
    }
}