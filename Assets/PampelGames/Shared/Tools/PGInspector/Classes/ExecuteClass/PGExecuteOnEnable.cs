// ----------------------------------------------------
// Copyright (c) Pampel Games e.K. All Rights Reserved.
// https://www.pampelgames.com
// ----------------------------------------------------

using System;
using UnityEngine;

namespace PampelGames.Shared.Tools.PGInspector
{
    public class PGExecuteOnEnable : PGExecuteClassBase
    {
        /* Editor Virtual******************************************************************************************************************/

#if UNITY_EDITOR
        public override string ExecuteName()
        {
            return "OnEnable";
        }
        public override string ExecuteInfo()
        {
            return "Starts automatically with OnEnable().";
        }
#endif

        public override void ComponentOnEnable(MonoBehaviour baseComponent, Action ExecuteAction)
        {
            base.ComponentOnEnable(baseComponent, ExecuteAction);
            ExecuteAction();
        }
        
    }
}