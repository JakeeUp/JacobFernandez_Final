// ----------------------------------------------------
// Gore Simulator
// Copyright (c) Pampel Games e.K. All Rights Reserved.
// https://www.pampelgames.com
// ----------------------------------------------------

using System.Collections.Generic;
using UnityEngine;

namespace PampelGames.GoreSimulator
{
    internal static class SkinnedChildren
    {

        public static List<Transform> CreateSkinnedChildren(GoreSimulator _goreSimulator)
        {
            List<Transform> detachedSkinnedChildren = new List<Transform>();
            for (int i = 0; i < _goreSimulator.skinnedChildren.Count; i++)
            {
                if(!_goreSimulator.skinnedChildren[i].enabled) continue;
                _goreSimulator.skinnedChildren[i].BakeMesh(_goreSimulator.skinnedChildrenBakedMeshes[i]);
                var detachedSkinnedChild = ObjectCreationUtility.CreateMeshObject(_goreSimulator, _goreSimulator.skinnedChildrenBakedMeshes[i],
                    _goreSimulator.gameObject.name + " - " + _goreSimulator.skinnedChildren[i].name);
                detachedSkinnedChild.transform.SetPositionAndRotation(_goreSimulator.skinnedChildren[i].transform.position, _goreSimulator.skinnedChildren[i].transform.rotation);
                if (detachedSkinnedChild.TryGetComponent<Renderer>(out var skinnedChildRenderer))
                    skinnedChildRenderer.materials = _goreSimulator.skinnedChildren[i].materials;
                _goreSimulator.skinnedChildren[i].enabled = false;
                _goreSimulator.AddDetachedObject(detachedSkinnedChild);
                detachedSkinnedChild.AddComponent<DetachedChild>();
                detachedSkinnedChildren.Add(detachedSkinnedChild.transform);
            }

            return detachedSkinnedChildren;
        }
        
        public static List<Transform> CreateSkinnedChildrenForBone(GoreSimulator _goreSimulator, Transform bone)
        {
            List<Transform> detachedSkinnedChildren = new List<Transform>();
            for (int i = 0; i < _goreSimulator.skinnedChildren.Count; i++)
            {
                if(!_goreSimulator.skinnedChildren[i].enabled) continue;
                if(!_goreSimulator.skinnedChildren[i].rootBone.IsChildOf(bone) && 
                   !bone.IsChildOf(_goreSimulator.skinnedChildren[i].rootBone)) continue;
                
                _goreSimulator.skinnedChildren[i].BakeMesh(_goreSimulator.skinnedChildrenBakedMeshes[i]);
                var detachedSkinnedChild = ObjectCreationUtility.CreateMeshObject(_goreSimulator, _goreSimulator.skinnedChildrenBakedMeshes[i],
                    _goreSimulator.gameObject.name + " - " + _goreSimulator.skinnedChildren[i].name);
                detachedSkinnedChild.transform.SetPositionAndRotation(_goreSimulator.skinnedChildren[i].transform.position, _goreSimulator.skinnedChildren[i].transform.rotation);
                if (detachedSkinnedChild.TryGetComponent<Renderer>(out var skinnedChildRenderer))
                    skinnedChildRenderer.materials = _goreSimulator.skinnedChildren[i].materials;
                _goreSimulator.skinnedChildren[i].enabled = false;
                _goreSimulator.AddDetachedObject(detachedSkinnedChild);
                detachedSkinnedChild.AddComponent<DetachedChild>();
                detachedSkinnedChildren.Add(detachedSkinnedChild.transform);
            }

            return detachedSkinnedChildren;
        }
    }
}
