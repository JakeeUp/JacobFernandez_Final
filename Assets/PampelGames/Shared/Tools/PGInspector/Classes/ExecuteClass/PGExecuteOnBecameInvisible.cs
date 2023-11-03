// ----------------------------------------------------
// Copyright (c) Pampel Games e.K. All Rights Reserved.
// https://www.pampelgames.com
// ----------------------------------------------------

using System;
using UnityEngine;

namespace PampelGames.Shared.Tools.PGInspector
{
    public class PGExecuteOnBecameInvisible : PGExecuteClassBase
    {
        /* Editor Virtual******************************************************************************************************************/

#if UNITY_EDITOR
        public override string ExecuteName()
        {
            return "On Became Invisible";
        }
        
        public override string ExecuteInfo()
        {
            return "Starts when an attached renderer is no longer visible by any camera.";
        }
#endif

        public override void ComponentOnBecameInvisible(MonoBehaviour baseComponent, Action ExecuteAction)
        {
            base.ComponentOnBecameInvisible(baseComponent, ExecuteAction);
            ExecuteAction();
        }
        
    }
}