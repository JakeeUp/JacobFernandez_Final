// ----------------------------------------------------
// Copyright (c) Pampel Games e.K. All Rights Reserved.
// https://www.pampelgames.com
// ----------------------------------------------------

using System;
using System.Collections;
using UnityEngine;

namespace PampelGames.Shared.Tools.PGInspector
{
    public class PGExecuteGetKeyDown : PGExecuteClassBase
    {
        /* Editor Virtual******************************************************************************************************************/

#if UNITY_EDITOR
        public override string ExecuteName()
        {
            return "Get Key Down";
        }
        public override string ExecuteInfo()
        {
            return "Starts when Input.GetKeyDown() recognizes the specified key.";
        }
#endif

        [Tooltip("Identifier of the Keyboard Key the user needs to press down.")]
        public KeyCode keyCode = KeyCode.Space;

        private Coroutine checkKeyCodeCoroutine;
        
        public override void ComponentOnEnable(MonoBehaviour baseComponent, Action ExecuteAction)
        {
            base.ComponentOnEnable(baseComponent, ExecuteAction);
            checkKeyCodeCoroutine = baseComponent.StartCoroutine(_GetKeyDownStartCheck(ExecuteAction));
        }
        
        private IEnumerator _GetKeyDownStartCheck(Action ExecuteAction)
        {
            for (;;)
            {
                if (isPaused) yield return null;
                if (Input.GetKeyDown(keyCode))
                    ExecuteAction();
                yield return null;
            }
        }
    }
}