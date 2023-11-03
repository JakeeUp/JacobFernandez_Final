// ----------------------------------------------------
// Copyright (c) Pampel Games e.K. All Rights Reserved.
// https://www.pampelgames.com
// ----------------------------------------------------

using System;
using System.Collections.Generic;
using PampelGames.Shared.Utility;
using UnityEngine;

namespace PampelGames.Shared.Tools.PGInspector
{
    public class PGExecuteOnTriggerExit : PGExecuteClassBase
    {
        /* Editor Virtual******************************************************************************************************************/

#if UNITY_EDITOR
        public override string ExecuteName()
        {
            return "On Trigger Exit";
        }
        
        public override string ExecuteInfo()
        {
            return "Starts when an attached trigger collider has stopped touching the trigger.";
        }
#endif

        [Tooltip("Layer Filter: Only execute when one of the specified Layers matches.")]
        public bool useLayerFilter;
        
        public LayerMask matchingLayers;

        [Tooltip("Tag Filter: Only execute when one of the specified Tags matches.")]
        public bool useTagFilter;

        public List<string> matchingTags = new();
        
        public override void ComponentOnTriggerExit(MonoBehaviour baseComponent, Action ExecuteAction, Collider other)
        {
            base.ComponentOnTriggerExit(baseComponent, ExecuteAction, other);
            
            if (useLayerFilter && matchingLayers != (matchingLayers | (1 << other.transform.gameObject.layer)) && !useTagFilter) return;
            if (useTagFilter && !matchingTags.Contains(other.gameObject.tag)) return;
            ExecuteAction();
        }
        
    }
}