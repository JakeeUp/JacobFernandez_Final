﻿// ----------------------------------------------------
// Copyright (c) Pampel Games e.K. All Rights Reserved.
// https://www.pampelgames.com
// ----------------------------------------------------

using System;
using System.Collections;
using UnityEngine;

namespace PampelGames.Shared.Tools.PGInspector
{
    public class PGStopGetButtonDown : PGStopClassBase
    {
        /* Editor Virtual******************************************************************************************************************/

#if UNITY_EDITOR
        public override string StopName()
        {
            return "Get Button Down";
        }
        public override string StopInfo()
        {
            return "Stops when Input.GetButtonDown() recognizes the specified button.\n" +
                   "Edit > ProjectSettings > InputManager";
        }
#endif

        [Tooltip("Identifier of the Button the user needs to press down.\n" +
                 "Edit > ProjectSettings > InputManager")]
        public string buttonName = "Fire1";

        private Coroutine checkButtonCoroutine;
        
        public override void ExecutionStart(MonoBehaviour baseComponent, Action StopAction)
        {
            base.ExecutionStart(baseComponent, StopAction);
            checkButtonCoroutine = baseComponent.StartCoroutine(_GetButtonDownStopCheck(StopAction));
        }
        public override void ExecutionStop(MonoBehaviour baseComponent, Action StopAction)
        {
            base.ExecutionStop(baseComponent, StopAction);
            if(checkButtonCoroutine != null) baseComponent.StopCoroutine(checkButtonCoroutine);
        }
        
        private IEnumerator _GetButtonDownStopCheck(Action StopAction)
        {
            for (;;)
            {
                if (isPaused) yield return null;
                if (Input.GetButtonDown(buttonName))
                    StopAction();
                yield return null;
            }
        }
    }
}