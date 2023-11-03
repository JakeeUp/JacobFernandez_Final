// ----------------------------------------------------
// Copyright (c) Pampel Games e.K. All Rights Reserved.
// https://www.pampelgames.com
// ----------------------------------------------------

using System;
using UnityEngine;

namespace PampelGames.Shared.Tools.PGInspector
{
    public class PGStopOnBecameInvisible : PGStopClassBase
    {
        /* Editor Virtual******************************************************************************************************************/

#if UNITY_EDITOR
        public override string StopName()
        {
            return "On Became Invisible";
        }
        
        public override string StopInfo()
        {
            return "Stops when an attached renderer is no longer visible by any camera.";
        }
#endif

        public override void ComponentOnBecameInvisible(MonoBehaviour baseComponent, Action StopAction)
        {
            base.ComponentOnBecameInvisible(baseComponent, StopAction);
            StopAction();
        }
        
    }
}