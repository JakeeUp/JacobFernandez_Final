// ----------------------------------------------------
// Gore Simulator
// Copyright (c) Pampel Games e.K. All Rights Reserved.
// https://www.pampelgames.com
// ----------------------------------------------------

using System.Collections.Generic;
using System.Linq;
using PampelGames.Shared.Utility;
using UnityEngine;

namespace PampelGames.GoreSimulator
{
    public class GoreMultiCut : MonoBehaviour, IGoreObjectParent
    {
        
        public GoreSimulator _goreSimulator;
        public BonesClass bonesClass;
        public Mesh bakedMesh;
        
        internal Enums.MultiCutStatus status = Enums.MultiCutStatus.Mesh;
        private Dictionary<string, List<MultiCutChildClass>> childrenDict;


        public void ExecuteCut(string boneName, Vector3 position)
        {
            ExecuteCut(boneName, position, Vector3.zero);
        }
        
        public void ExecuteCut(string boneName, Vector3 position, Vector3 force)
        {
            ExecuteCutInternal(boneName, position, force);
        }
        
        public void ExecuteExplosion()
        {
            PrepareRagdollExplosion();
            var centerPosition = CalculateCenterPosition();
            ExecuteExplosionInternal(Vector3.zero, 0, centerPosition);
        }

        public void ExecuteExplosion(float radialForce)
        {
            PrepareRagdollExplosion();
            var centerPosition = CalculateCenterPosition();
            ExecuteExplosionInternal(centerPosition, radialForce, centerPosition);
        }

        public void ExecuteExplosion(Vector3 position, float force)
        {
            PrepareRagdollExplosion();
            var centerPosition = CalculateCenterPosition();
            ExecuteExplosionInternal(position, force, centerPosition);
        }
        
        /********************************************************************************************************************************/

        private void PrepareRagdollExplosion()
        {
            if (status == Enums.MultiCutStatus.Ragdoll)
            {
                CreateChildObjects();
            }
        }

        private void ExecuteExplosionInternal(Vector3 position, float force, Vector3 centerPosition)
        {
            var subModuleClass = ExecutionClassesUtility.GetPoolSubModuleClass();
            subModuleClass.multiCut = true;
            subModuleClass.cutPosition = position;
            subModuleClass.centerPosition = centerPosition;
            subModuleClass.parent = gameObject;
            subModuleClass.position = position;
            subModuleClass.subModuleObjectClasses.Clear();
            
            if (status == Enums.MultiCutStatus.Ragdoll)
            {
                subModuleClass.multiCut = false;
                subModuleClass.subRagdoll = true;
                status = Enums.MultiCutStatus.Mesh;
            }

            foreach (KeyValuePair<string, List<MultiCutChildClass>> kvp in childrenDict)
            {
                foreach (MultiCutChildClass child in kvp.Value)
                {
                    SubModuleObjectClass subModuleObjectClass = new SubModuleObjectClass();
                    subModuleObjectClass.obj = child.childObject;
                    subModuleObjectClass.renderer = child.childObject.GetComponent<Renderer>();
                    subModuleObjectClass.mesh = child.chunkClass.mesh;
                    Vector3 worldCenter = subModuleObjectClass.obj.transform.TransformPoint(subModuleObjectClass.mesh.bounds.center);
                    subModuleObjectClass.centerPosition = worldCenter;
                    if (force != 0)
                    {
                        subModuleObjectClass.force = (worldCenter - position).normalized * force;    
                    }
                    subModuleClass.subModuleObjectClasses.Add(subModuleObjectClass);
                    if(child.childObject.TryGetComponent<GoreMesh>(out var goreMesh)) Destroy(goreMesh);
                    child.childObject.transform.SetParent(null);
                }
            }

            for (var k = 0; k < _goreSimulator.explosionModules.Count; k++)
            {
                if(!_goreSimulator.explosionModules[k].moduleActive) continue;
                _goreSimulator.explosionModules[k].ExecuteModuleExplosion(subModuleClass);
            }

            ExecutionClassesUtility.ReleaseSubModuleClass(subModuleClass);
            Destroy(gameObject);
        }
        
        
        private void ExecuteCutInternal(string boneName, Vector3 position, Vector3 force)
        {
            if (childrenDict.Count == 0) return;

            var subModuleClass = ExecutionClassesUtility.GetPoolSubModuleClass();
            subModuleClass.multiCut = true;
            subModuleClass.cutPosition = position;
            subModuleClass.force = force;
            
            if (status == Enums.MultiCutStatus.Ragdoll)
            {
                CreateChildObjects();
                status = Enums.MultiCutStatus.Mesh;
                subModuleClass.multiCut = false;
            }
            
            subModuleClass.centerMesh = bakedMesh;
            

            List<MultiCutChildClass> children01 = new List<MultiCutChildClass>();
            List<MultiCutChildClass> children02 = new List<MultiCutChildClass>();

            if (!childrenDict.TryGetValue(boneName, out var multiCutChildClassList)) return;
            foreach (var multiCutChildClass in new List<MultiCutChildClass>(multiCutChildClassList))
            {
                children01.Add(multiCutChildClass);
                multiCutChildClassList.Remove(multiCutChildClass);
            }
            
            if (children01.Count == 0) return;

            childrenDict.Remove(boneName);
            
            
            Vector3 directionLocal = (children01[0].chunkClass.mesh.bounds.center - children01[^1].chunkClass.mesh.bounds.center).normalized;
            Vector3 direction = children01[0].childObject.transform.TransformDirection(directionLocal);
            
            
            for (int i = children01.Count - 1; i >= 0; i--)
            {
                var localBoundsCenter = children01[i].chunkClass.mesh.bounds.center;
                var worldBoundsCenter = children01[i].childObject.transform.TransformPoint(localBoundsCenter);
                
                Vector3 toObject = worldBoundsCenter - position;  
                if (Vector3.Dot(toObject.normalized, direction.normalized) > 0)
                {
                    children02.Add(children01[i]);
                    children01.RemoveAt(i);
                }
            }
            

            if (children01.Count > 0 && children02.Count == 0)
            {
                AddChunkChildren(children02, boneName);
                children01.AddRange(childrenDict.SelectMany(pair => pair.Value));
            }
            else if (children02.Count > 0 && children01.Count == 0)
            {
                AddChunkChildren(children01, boneName);
                children02.AddRange(childrenDict.SelectMany(pair => pair.Value));
            }
            else
            {
                var maxCutIndexClassIndex01 = children01
                    .Select(c => (int?)c.chunkClass.cutIndexClassIndex)
                    .Max();

                var maxCutIndexClassIndex02 = children02
                    .Select(c => (int?)c.chunkClass.cutIndexClassIndex)
                    .Max();

                if (maxCutIndexClassIndex01 > maxCutIndexClassIndex02)
                {
                    AddChunkChildren(children01, boneName);
                    children02.AddRange(childrenDict.SelectMany(pair => pair.Value));
                }
                else
                {
                    AddChunkChildren(children01, boneName);
                    children01.AddRange(childrenDict.SelectMany(pair => pair.Value));
                }
            }
            
            
            if (children01.Count < children02.Count)
            {
                (children01, children02) = (children02, children01);
            }

            if (children02.Count > 0)
            {
                var worldBounds01 = children01[0].childObject.transform.TransformDirection(children01[0].chunkClass.mesh.bounds.center);
                var worldBounds02 = children02[0].childObject.transform.TransformDirection(children02[0].chunkClass.mesh.bounds.center);
                subModuleClass.cutDirection = worldBounds02 - worldBounds01;
            }
            
            /********************************************************************************************************************************/
            // New 
            
            var thisGameObject = gameObject;

            if(children02.Count > 0)
            {
                var detachedObject = new GameObject(thisGameObject.name + " - Cut");
                detachedObject.transform.SetPositionAndRotation(thisGameObject.transform.position, thisGameObject.transform.rotation);
                subModuleClass.parent = detachedObject;

                var goreMultiCut = detachedObject.AddComponent<GoreMultiCut>();
                goreMultiCut._goreSimulator = _goreSimulator;
                goreMultiCut.bonesClass = bonesClass;
                goreMultiCut.bakedMesh = bakedMesh;
                goreMultiCut.CreateMultiCutChildClassDict(children02);

                for (var i = 0; i < children02.Count; i++)
                {
                    if (children02[i].childObject.TryGetComponent<GoreMesh>(out var goreMesh)) goreMesh._goreMultiCut = goreMultiCut;
                    SubModuleObjectClass subModuleObjectClass = new SubModuleObjectClass();
                    subModuleObjectClass.obj = children02[i].childObject;
                    subModuleObjectClass.renderer = children02[i].childObject.GetComponent<Renderer>();
                    subModuleObjectClass.mesh = children02[i].chunkClass.mesh;
                    subModuleObjectClass.obj.transform.SetParent(detachedObject.transform);
                    subModuleClass.subModuleObjectClasses.Add(subModuleObjectClass);
                }

                for (var k = 0; k < _goreSimulator.cutModules.Count; k++)
                {
                    if(!_goreSimulator.cutModules[k].moduleActive) continue;
                    _goreSimulator.cutModules[k].ExecuteModuleCut(subModuleClass);
                }

                _goreSimulator.AddDestroyableObject(detachedObject);
            }
            
            /********************************************************************************************************************************/
            // Existing
            
            subModuleClass.parent = gameObject;
            subModuleClass.subModuleObjectClasses.Clear();
            for (var i = 0; i < children01.Count; i++)
            {
                SubModuleObjectClass subModuleObjectClass = new SubModuleObjectClass();
                subModuleObjectClass.obj = children01[i].childObject;
                subModuleObjectClass.renderer = children01[i].childObject.GetComponent<Renderer>();
                subModuleObjectClass.mesh = children01[i].chunkClass.mesh;
                subModuleClass.subModuleObjectClasses.Add(subModuleObjectClass);
            }
            
            for (var k = 0; k < _goreSimulator.cutModules.Count; k++)
            {
                _goreSimulator.cutModules[k].ExecuteModuleCut(subModuleClass);
            }
            
            CreateMultiCutChildClassDict(children01);
            
            ExecutionClassesUtility.ReleaseSubModuleClass(subModuleClass);
        }
        
        /********************************************************************************************************************************/

        private void CreateChildObjects()
        {
            // ElementAt returns random value from the dict. Used when created with sub-ragdoll.
            var randomItem = childrenDict.ElementAt(0);
            var childRenderer = randomItem.Value[0].childObject.GetComponent<SkinnedMeshRenderer>();
            var childRendererTransform = childRenderer.transform;

            childRenderer.sharedMesh = _goreSimulator.originalMesh;
            childRenderer.BakeMesh(bakedMesh);
            var bakedVertices = PGMeshUtility.CreateVertexList(bakedMesh);

            var transform1 = transform;
            List<GameObject> currentChildren = new List<GameObject>(transform1.childCount);
            foreach (Transform child in transform) currentChildren.Add(child.gameObject);

            foreach (KeyValuePair<string, List<MultiCutChildClass>> kvp in childrenDict)
            {
                foreach (MultiCutChildClass multiCutChildClass in kvp.Value)
                {
                    MeshCutJobs.IndexesSnapshotExplosion(transform, multiCutChildClass.chunkClass, bakedVertices);

                    var detachedChild = ObjectCreationUtility.CreateMeshObject(_goreSimulator, multiCutChildClass.chunkClass.mesh,
                        gameObject.name + " - " + multiCutChildClass.chunkClass.boneName + " - " + multiCutChildClass.chunkClass.cutIndexClassIndex);
                    detachedChild.transform.SetPositionAndRotation(childRendererTransform.position, childRendererTransform.rotation);
                    if (detachedChild.TryGetComponent<Renderer>(out var _renderer)) _renderer.materials = childRenderer.materials;
                    detachedChild.transform.SetParent(transform1);
                    multiCutChildClass.childObject = detachedChild;
                    multiCutChildClass.chunkClass.mesh.RecalculateBounds();
                    var goreMesh = detachedChild.AddComponent<GoreMesh>();
                    goreMesh._boneName = multiCutChildClass.chunkClass.boneName;
                    goreMesh._goreMultiCut = this;
                }
            }

            foreach (var currentChild in currentChildren) Destroy(currentChild);
        }

        private void AddChunkChildren(List<MultiCutChildClass> multiCutChildClasses, string boneName)
        {
            if(!_goreSimulator.bonesDict.TryGetValue(boneName, out var tuple)) return;
            for (int i = 0; i < tuple.Item1.boneChildrenSel.Count; i++)
            {
                if (!childrenDict.TryGetValue(tuple.Item1.boneChildrenSel[i].name, out var childMultiCutChildClasses)) continue;
                multiCutChildClasses.AddRange(childMultiCutChildClasses);
                childrenDict.Remove(tuple.Item1.boneChildrenSel[i].name);
            }
        }

        private Vector3 CalculateCenterPosition()
        {
            List<Vector3> worldCenters = new List<Vector3>();
            foreach (KeyValuePair<string, List<MultiCutChildClass>> kvp in childrenDict)
            {
                foreach (MultiCutChildClass child in kvp.Value)
                {
                    child.chunkClass.mesh.RecalculateBounds();
                    var localBoundsCenter = child.chunkClass.mesh.bounds.center;
                    worldCenters.Add(child.childObject.transform.TransformPoint(localBoundsCenter));
                }
            }
            
            var avgX = worldCenters.Average(pos => pos.x);
            var avgY = worldCenters.Average(pos => pos.y);
            var avgZ = worldCenters.Average(pos => pos.z);

            return new Vector3(avgX, avgY, avgZ);
        }
        
        /********************************************************************************************************************************/

        internal void CreateMultiCutChildClassDict(List<MultiCutChildClass> multiCutChildClasses)
        {
            childrenDict = new Dictionary<string, List<MultiCutChildClass>>();

            for (int i = 0; i < multiCutChildClasses.Count; i++)
            {
                var currentKey = multiCutChildClasses[i].chunkClass.boneName;
                if (childrenDict.ContainsKey(currentKey))
                {
                    childrenDict[currentKey].Add(multiCutChildClasses[i]);
                }
                else
                {
                    childrenDict[currentKey] = new List<MultiCutChildClass> { multiCutChildClasses[i] };
                }
            }
        }
        
        
    }
}