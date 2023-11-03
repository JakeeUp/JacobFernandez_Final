// ----------------------------------------------------
// Copyright (c) Pampel Games e.K. All Rights Reserved.
// https://www.pampelgames.com
// ----------------------------------------------------

using System;
using UnityEngine;

namespace PampelGames.Shared.Tools.PGInspector
{
    public class PGExecuteOnBecameVisible : PGExecuteClassBase
    {
        /* Editor Virtual******************************************************************************************************************/

#if UNITY_EDITOR
        public override string ExecuteName()
        {
            return "On Became Visible";
        }
        public override string ExecuteInfo()
        {
            return "Starts when an attached renderer became visible by any camera.";
        }
#endif

        public override void ComponentOnBecameVisible(MonoBehaviour baseComponent, Action ExecuteAction)
        {
            base.ComponentOnBecameVisible(baseComponent, ExecuteAction);
            ExecuteAction();
        }
        
    }
}