// ----------------------------------------------------
// Copyright (c) Pampel Games e.K. All Rights Reserved.
// https://www.pampelgames.com
// ----------------------------------------------------

using System;
using UnityEngine;

namespace PampelGames.Shared.Tools.PGInspector
{
    public class PGExecuteStart : PGExecuteClassBase
    {
        /* Editor Virtual******************************************************************************************************************/

#if UNITY_EDITOR
        public override string ExecuteName()
        {
            return "Start";
        }
        public override string ExecuteInfo()
        {
            return "Starts automatically with Start().";
        }
#endif

        public override void ComponentStart(MonoBehaviour baseComponent, Action ExecuteAction)
        {
            base.ComponentStart(baseComponent, ExecuteAction);
            ExecuteAction();
        }
        
    }
}
