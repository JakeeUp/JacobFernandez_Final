// ----------------------------------------------------
// Copyright (c) Pampel Games e.K. All Rights Reserved.
// https://www.pampelgames.com
// ----------------------------------------------------

using System;
using System.Collections.Generic;
using UnityEngine;

namespace PampelGames.Shared.Tools.PGInspector
{
    public class PGExecuteOnCollisionEnter : PGExecuteClassBase
    {
        /* Editor Virtual******************************************************************************************************************/

#if UNITY_EDITOR
        public override string ExecuteName()
        {
            return "On Collision Enter";
        }
        public override string ExecuteInfo()
        {
            return "Starts when an attached non-trigger collider collides with another.";
        }
#endif

        [Tooltip("Layer Filter: Only execute when one of the specified Layers matches.")]
        public bool useLayerFilter;
        
        public LayerMask matchingLayers;

        [Tooltip("Tag Filter: Only execute when one of the specified Tags matches.")]
        public bool useTagFilter;

        public List<string> matchingTags = new();
        
        public override void ComponentOnCollisionEnter(MonoBehaviour baseComponent, Action ExecuteAction, Collision collision)
        {
            base.ComponentOnCollisionEnter(baseComponent, ExecuteAction, collision);
            
            if (useLayerFilter && matchingLayers != (matchingLayers | (1 << collision.gameObject.layer)) && !useTagFilter) return;
            if (useTagFilter && !matchingTags.Contains(collision.gameObject.tag)) return;
            ExecuteAction();
        }
        
    }
}