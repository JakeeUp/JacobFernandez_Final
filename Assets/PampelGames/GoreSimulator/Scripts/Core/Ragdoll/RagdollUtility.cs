// ----------------------------------------------------
// Gore Simulator
// Copyright (c) Pampel Games e.K. All Rights Reserved.
// https://www.pampelgames.com
// ----------------------------------------------------

using System.Collections.Generic;
using UnityEngine;

namespace PampelGames.GoreSimulator
{
    public static class RagdollUtility
    {
        public static void ToggleRagdoll(IEnumerable<GoreBone> goreBones, bool active)
        {
            if (active)
            {
                foreach (var goreBone in goreBones)
                {
                    goreBone._rigidbody.isKinematic = false;
                    goreBone._rigidbody.velocity = Vector3.zero;
                    goreBone._rigidbody.angularVelocity = Vector3.zero;
                }    
            }
            else
            {
                foreach (var goreBone in goreBones)
                {
                    // goreBone._rigidbody.velocity = Vector3.zero;
                    // goreBone._rigidbody.angularVelocity = Vector3.zero;
                    goreBone._rigidbody.isKinematic = true;
                }   
            }
            
        }
        
        
        public static GameObject CreateRagdollObject(SkinnedMeshRenderer smr, string objName)
        {
            var smrTransform = smr.transform;
            var originalSMRParent = smrTransform.parent;
            var originalRootBoneParent = smr.rootBone.parent;

            GameObject tempObj = new GameObject();
            tempObj.transform.SetPositionAndRotation(smrTransform.position, smrTransform.rotation);

            smr.transform.SetParent(tempObj.transform);
            smr.rootBone.SetParent(tempObj.transform);
            var ragdollCutCopy = Object.Instantiate(tempObj);
            ragdollCutCopy.name = objName;
            
            smr.transform.SetParent(originalSMRParent);
            smr.rootBone.SetParent(originalRootBoneParent);
            
            Object.Destroy(tempObj);
            return ragdollCutCopy;
        }
        
        
        
    }
}
