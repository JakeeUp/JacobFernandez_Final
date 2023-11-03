// ----------------------------------------------------
// Copyright (c) Pampel Games e.K. All Rights Reserved.
// https://www.pampelgames.com
// ----------------------------------------------------

using System;
using System.Collections;
using UnityEngine;

namespace PampelGames.Shared.Tools.PGInspector
{
    public class PGExecuteGetButtonDown : PGExecuteClassBase
    {
        /* Editor Virtual******************************************************************************************************************/

#if UNITY_EDITOR
        public override string ExecuteName()
        {
            return "Get Button Down";
        }
        public override string ExecuteInfo()
        {
            return "Starts when Input.GetButtonDown() recognizes the specified button.\n" +
                   "Edit > ProjectSettings > InputManager";
        }
#endif

        [Tooltip("Identifier of the Button the user needs to press down.\n" +
                 "Edit > ProjectSettings > InputManager")]
        public string buttonName = "Fire1";

        private Coroutine checkButtonCoroutine;
        
        public override void ComponentOnEnable(MonoBehaviour baseComponent, Action ExecuteAction)
        {
            base.ComponentOnEnable(baseComponent, ExecuteAction);
            checkButtonCoroutine = baseComponent.StartCoroutine(_GetButtonDownStartCheck(ExecuteAction));
        }
        
        private IEnumerator _GetButtonDownStartCheck(Action ExecuteAction)
        {
            for (;;)
            {
                if (isPaused) yield return null;
                if (Input.GetButtonDown(buttonName))
                    ExecuteAction();
                yield return null;
            }
        }
    }
}