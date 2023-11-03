// ----------------------------------------------------
// Copyright (c) Pampel Games e.K. All Rights Reserved.
// https://www.pampelgames.com
// ----------------------------------------------------

using System;
using System.Collections.Generic;
using PampelGames.Shared.Utility;
using UnityEngine;
using UnityEngine.Events;
using Random = UnityEngine.Random;

namespace PampelGames.Shared.Tools.PGInspector
{
    [Serializable]
    public class PGRandomEventClass
    {
        [Tooltip("Call InvokeRandomEvent() in PropertyEngine to start the selection.\n" +
                 "\n" +
                 "Single: Invokes one event from the list, calculating the relative weight for each using the slider probability.\n" +
                 "\n" +
                 "Multi: Makes an independent random check for each event. 1 is certain invoke, 0 is certain skip.\n" +
                 "")]
        public RandomModeEnum randomMode = RandomModeEnum.Single;

        public List<RandomEventWrapperClass> randomEvents = new();

        [Serializable]
        public class RandomEventWrapperClass
        {
            public float instanceWeight = 0.5f;
            public UnityEvent unityEvent = new();
        }

        public enum RandomModeEnum
        {
            Single,
            Multi
        }

        /********************************************************************************************************************************/

        public void InvokeRandomEvent()
        {
            if (randomMode == RandomModeEnum.Single) InvokeSingle();
            else if (randomMode == RandomModeEnum.Multi) InvokeMulti();
        }

        private void InvokeSingle()
        {
            var instancesWeights = new List<float>();
            for (var i = 0; i < randomEvents.Count; i++) instancesWeights.Add(randomEvents[i].instanceWeight);
            var randomArrayEntry = PGMathUtility.GetRandomArrayEntry(instancesWeights.Count, instancesWeights);
            randomEvents[randomArrayEntry].unityEvent.Invoke();
        }

        private void InvokeMulti()
        {
            for (var i = 0; i < randomEvents.Count; i++)
            {
                var random = Random.value < randomEvents[i].instanceWeight;
                if (random) randomEvents[i].unityEvent.Invoke();
            }
        }
    }
}