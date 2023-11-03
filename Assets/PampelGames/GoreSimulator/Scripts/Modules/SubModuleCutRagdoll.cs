// ----------------------------------------------------
// Gore Simulator
// Copyright (c) Pampel Games e.K. All Rights Reserved.
// https://www.pampelgames.com
// ----------------------------------------------------

using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace PampelGames.GoreSimulator
{
    public class SubModuleCutRagdoll : SubModuleBase
    {
        public override string ModuleName()
        {
            return "Sub Ragdoll";
        }

        public override string ModuleInfo()
        {
            return "Creates ragdolls for cutted bones. Requires the initialized Ragdoll module.";
        }

        public override int imageIndex()
        {
            return 5;
        }

        public override bool CompatibleExplosion()
        {
            return false;
        }

        public override bool CompatibleRagdoll()
        {
            return false;
        }

        public override void ExecuteModuleExplosion(SubModuleClass subModuleClass)
        {
        }

        public override void ExecuteModuleRagdoll(List<GoreBone> goreBones)
        {
        }

        /********************************************************************************************************************************/

        public int minimumBoneAmount = 2;
        public float drag = 0.5f;
        public float angularDrag = 1.5f;

        /********************************************************************************************************************************/

        public override void ExecuteModuleCut(SubModuleClass subModuleClass)
        {
        }

        /// <summary>
        ///     This module is executed directly from within <see cref="GoreModuleCut"/>.
        /// </summary>
        internal void ExecuteRagdollSubModule(GameObject detachedObject, Mesh mesh, CutIndexClass cutIndexClass, BonesClass bonesClass,
            List<Vector3> cutCenters, SubModuleClass subModuleClass, GoreMultiCut goreMultiCut)
        {
            
            var detachedSMR = detachedObject.GetComponentInChildren<SkinnedMeshRenderer>();
            detachedSMR.sharedMesh = mesh;
            var bones = detachedSMR.bones;
            var detachedBoneParent = bones[cutIndexClass.detachedParent];

            var deleteBones = new List<Transform>(cutIndexClass.RemovableChildren.Count);
            deleteBones.AddRange(cutIndexClass.RemovableChildren.Select(t => bones[t]));

            for (int i = 0; i < cutIndexClass.nonActiveBones.Count; i++)
            {
                var nonActiveChild = bones[cutIndexClass.nonActiveBones[i]];
                if (nonActiveChild.gameObject.TryGetComponent<Collider>(out var collider))
                    Object.Destroy(collider);
            }
                
            foreach (var deleteBone in deleteBones) Object.Destroy(deleteBone.gameObject);
            detachedBoneParent.SetParent(detachedObject.transform);
            if(detachedSMR.rootBone != detachedBoneParent) Object.Destroy(detachedSMR.rootBone.gameObject);
                
            detachedSMR.rootBone = detachedBoneParent;
                
            var detachedGoreBones = detachedBoneParent.GetComponentsInChildren<GoreBone>();
            for (int i = 0; i < detachedGoreBones.Length; i++)
            {
                detachedGoreBones[i]._rigidbody.drag = drag;
                detachedGoreBones[i]._rigidbody.angularDrag = angularDrag;
                detachedGoreBones[i]._goreMultiCut = goreMultiCut;
            }
            RagdollUtility.ToggleRagdoll(detachedGoreBones, true);
            
            subModuleClass.subModuleObjectClasses.Add(new SubModuleObjectClass());
            var subModuleObjClass = subModuleClass.subModuleObjectClasses[^1];
            subModuleObjClass.obj = detachedSMR.gameObject;
            subModuleObjClass.mesh = mesh;
            subModuleObjClass.renderer = detachedSMR;
            subModuleObjClass.cutCenters = cutCenters;
        }
    }
}