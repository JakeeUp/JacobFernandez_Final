// ----------------------------------------------------
// Gore Simulator
// Copyright (c) Pampel Games e.K. All Rights Reserved.
// https://www.pampelgames.com
// ----------------------------------------------------

using System;
using System.Collections.Generic;
using PampelGames.Shared.Utility;
using UnityEngine;

namespace PampelGames.GoreSimulator
{
    [Serializable]
    public class GoreModuleExplosion : GoreModuleBase
    {
        public override string ModuleName()
        {
            return "Explosion";
        }

        public override string ModuleInfo()
        {
            return "Explosion Module: Enables mesh explosion.\n" + "\n" +
                   "GoreSimulator.ExecuteExplosion();";
        }

        public override int imageIndex()
        {
            return 3;
        }
        
        public override void ClearSubmodules()
        {
            _goreSimulator.explosionModules.Clear();
        }

        public override void Reset(List<BonesClass> bonesClasses)
        {
            if (!_goreSimulator.meshCutInitialized) return;
            base.Reset(bonesClasses);
            for (int i = 0; i < _goreSimulator.explosionModules.Count; i++) _goreSimulator.explosionModules[i].Reset();
            ToggleCollider(_goreSimulator.goreBones, true);
        }

        /********************************************************************************************************************************/

        public void ExecuteExplosion(Vector3 position, float force)
        {
            ToggleCollider(_goreSimulator.goreBones, false);
            ExecutionUtility.AddCutMaterial(_goreSimulator);

            _goreSimulator.smr.sharedMesh = _goreSimulator.originalMesh;
            _goreSimulator.smr.BakeMesh(_goreSimulator.bakedMesh);

            
            var bakedVertices = PGMeshUtility.CreateVertexList(_goreSimulator.bakedMesh);
            
            var subModuleClass = ExecutionClassesUtility.GetPoolSubModuleClass();
            subModuleClass.parent = _goreSimulator.gameObject;
            subModuleClass.children = _goreSimulator.nonBoneChildren;
            subModuleClass.centerMesh = _goreSimulator.bakedMesh;
            var boundsCenter = _goreSimulator.bakedMesh.bounds.center;
            subModuleClass.centerPosition = _goreSimulator.smr.transform.TransformPoint(boundsCenter);
            subModuleClass.position = position;
            
            // Skinned Children
            var detachedSkinnedChildren = SkinnedChildren.CreateSkinnedChildren(_goreSimulator);
            subModuleClass.children.AddRange(detachedSkinnedChildren);
            
            for (var i = 0; i < _goreSimulator.bonesClasses.Count; i++)
            {
                var bonesClass = _goreSimulator.bonesClasses[i];
                var chunkClasses = bonesClass.indexesStorageClass.chunkClasses;
                
                for (var j = 0; j < chunkClasses.Count; j++)
                {
                    if (bonesClass.cutted && j >= bonesClass.cuttedIndex) continue;

                    subModuleClass.subModuleObjectClasses.Add(new SubModuleObjectClass());
                    var subModuleObjClass = subModuleClass.subModuleObjectClasses[^1];
                    
                    MeshCutJobs.IndexesSnapshotExplosion(_goreSimulator.smr.transform, chunkClasses[j], bakedVertices);

                    var detachedObj = ObjectCreationUtility.CreateMeshObject(_goreSimulator, chunkClasses[j].mesh,
                        _goreSimulator.gameObject.name + " - " + chunkClasses[j].boneName + " - " + chunkClasses[j].cutIndexClassIndex);

                    if (detachedObj.TryGetComponent<Renderer>(out var renderer))
                    {
                        subModuleObjClass.renderer = renderer;
                        renderer.materials = _goreSimulator.smr.materials;
                    }

                    var smrTransform = _goreSimulator.smr.transform;
                    detachedObj.transform.SetPositionAndRotation(smrTransform.position, smrTransform.rotation);
                    
                    subModuleObjClass.obj = detachedObj;
                    subModuleObjClass.mesh = chunkClasses[j].mesh;
                    subModuleObjClass.cutCenters = chunkClasses[j].cutCenters;

                    chunkClasses[j].mesh.RecalculateBounds();
                    Vector3 worldCenter = subModuleObjClass.obj.transform.TransformPoint(chunkClasses[j].mesh.bounds.center);
                    subModuleObjClass.centerPosition = worldCenter;
                    if (force != 0)
                    {
                        subModuleObjClass.force = (worldCenter - position).normalized * force;
                    }
                    

                }
            }

            _goreSimulator.smr.enabled = false;
            
            for (var k = 0; k < _goreSimulator.explosionModules.Count; k++)
            {
                if(!_goreSimulator.explosionModules[k].moduleActive) continue;
                _goreSimulator.explosionModules[k].ExecuteModuleExplosion(subModuleClass);
            }
            
            foreach (var child in _goreSimulator.nonBoneChildren) child.parent = null;
            
            
            ExecutionClassesUtility.ReleaseSubModuleClass(subModuleClass);
        }
        
        /********************************************************************************************************************************/
        
        public void ToggleCollider(IEnumerable<GoreBone> goreBones, bool active)
        {
            if (!_goreSimulator.colliderInitialized) return;
            foreach (var goreBone in goreBones)
            {
                goreBone._collider.enabled = active;
            }    
        }
        
    }
}