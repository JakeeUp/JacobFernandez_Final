// ----------------------------------------------------
// Gore Simulator
// Copyright (c) Pampel Games e.K. All Rights Reserved.
// https://www.pampelgames.com
// ----------------------------------------------------

using System.Collections.Generic;
using PampelGames.Shared.Utility;
using UnityEngine;

namespace PampelGames.GoreSimulator
{
    internal static class CutUtility
    {
        
        public static int FindNearestTransformIndex(List<Transform> transforms, Vector3 position)
        {
            int nearestIndex = 0;
            float nearestSqrMagnitude = float.MaxValue;

            for (int i = 0; i < transforms.Count; i++)
            {
                Vector3 delta = transforms[i].position - position;
                float sqrMagnitude = delta.sqrMagnitude;
        
                if (sqrMagnitude < nearestSqrMagnitude)
                {
                    nearestSqrMagnitude = sqrMagnitude;
                    nearestIndex = i;
                }
            }

            return nearestIndex;
        }
        
        public static int GetCutIndex(Transform smrTransform, BonesStorageClass bonesStorageClass, List<Vector3> vertices, Vector3 position)
        {
            if (bonesStorageClass.cutIndexClasses.Count == 1) return 0;

            var chunkCenters = new List<Vector3>();

            for (var i = 0; i < bonesStorageClass.cutIndexClasses.Count; i++)
            {
                var averageIndexes = bonesStorageClass.cutIndexClasses[i].chunkAverageIndexes;

                var sum = Vector3.zero;
                for (var j = 0; j < averageIndexes.Count; j++) sum += vertices[averageIndexes[j]];
                var worldSpace = smrTransform.TransformPoint(sum / averageIndexes.Count);
                chunkCenters.Add(worldSpace);
            }

            var nearestIndex = 0;
            var nearestSqrMagnitude = float.MaxValue;

            for (var i = 0; i < chunkCenters.Count; i++)
            {
                var delta = chunkCenters[i] - position;
                var sqrMagnitude = delta.sqrMagnitude;

                if (sqrMagnitude < nearestSqrMagnitude)
                {
                    nearestSqrMagnitude = sqrMagnitude;
                    nearestIndex = i;
                }
            }

            return nearestIndex;
        }
        

        /// <summary>
        ///     Adding the chunks from the same bone until the used chunk.
        /// </summary>
        public static void AddSameBoneHierarchy(ExecutionCutClass executionCutClass, int cutIndex, BonesStorageClass usedBonesStorageClass,
            int usedCutIndex, List<ChunkClass> chunkClasses, List<ChunkClass> chunkClassesNew, 
            bool unionWithChunks, List<string> cuttedChildrenNames)
        {
            var addExecutionIndexes = true;
            if (usedCutIndex == -1)
            {
                usedCutIndex = usedBonesStorageClass.cutIndexClasses.Count;
                addExecutionIndexes = false;
            }

            for (var i = cutIndex; i < usedCutIndex; i++)
            {
                chunkClassesNew.Add(chunkClasses[i]);
                var usedCutIndexClass = usedBonesStorageClass.cutIndexClasses[i];
                if(!unionWithChunks) continue;
                
                executionCutClass.newIndexes.UnionWith(usedCutIndexClass.chunkIndexes);
                
                if (i == usedCutIndex - 1 && addExecutionIndexes)
                    for (var j = 0; j < usedCutIndexClass.cutIndexChildClasses.Count; j++)
                    {
                        var cutChildClass = usedCutIndexClass.cutIndexChildClasses[j];
                        if(!cuttedChildrenNames.Contains(cutChildClass.childBoneName)) continue;
                        executionCutClass.AddExecutionIndexes(cutChildClass.cutIndexes, cutChildClass.sewIndexes, cutChildClass.sewTriangles);
                    }
            }
        }

        /// <summary>
        ///     Adding the children chunks until the last cutted chunk.
        /// </summary>
        public static void AddChildBoneHierarchy(ExecutionCutClass executionCutClass, BonesStorageClass usedBonesStorageClass, int usedCutIndex,
            List<ChunkClass> chunkClasses, List<ChunkClass> chunkClassesNew, bool unionWithChunks)
        {
            var addExecutionIndexes = true;
            if (usedCutIndex == -1)
            {
                usedCutIndex = usedBonesStorageClass.cutIndexClasses.Count;
                addExecutionIndexes = false;
            }

            for (var i = 0; i < usedCutIndex; i++)
            {
                var usedCutIndexClass = usedBonesStorageClass.cutIndexClasses[i];

                chunkClassesNew.Add(chunkClasses[i]);
                if(!unionWithChunks) continue;

                executionCutClass.newIndexes.UnionWith(usedCutIndexClass.chunkIndexes);
                
                if (i == usedCutIndex - 1 && addExecutionIndexes)
                    for (var j = 0; j < usedCutIndexClass.cutIndexChildClasses.Count; j++)
                    {
                        var cutChildClass = usedCutIndexClass.cutIndexChildClasses[j];
                        executionCutClass.AddExecutionIndexes(cutChildClass.cutIndexes, cutChildClass.sewIndexes, cutChildClass.sewTriangles);
                    }
            }
        }

        public static void RemoveUsedChildren(GoreSimulator _goreSimulator, ExecutionCutClass centerExecutionCutClass, BonesClass bonesClass)
        {
            for (var i = _goreSimulator.usedBonesClasses.Count - 1; i >= 0; i--)
            {
                var usedBone = _goreSimulator.usedBonesClasses[i].usedBone;
                if (usedBone == bonesClass.bone)
                {
                    centerExecutionCutClass.RemoveIndexes(i);
                    _goreSimulator.usedBonesClasses.RemoveAt(i);
                    continue;
                }

                if (usedBone.IsChildOf(bonesClass.bone))
                {
                    centerExecutionCutClass.RemoveIndexes(i);
                    _goreSimulator.usedBonesClasses.RemoveAt(i);
                }
            }
        }
        
        
        public static SubModuleObjectClass CreateSubModuleObjectClass(GoreMultiCut goreMultiCut, ChunkClass chunkClass,
            Transform transform, List<Vector3> bakedVertices)
        {
            var subModuleObjClass = new SubModuleObjectClass();
                    
            MeshCutJobs.IndexesSnapshotExplosion(transform, chunkClass, bakedVertices);
        
            var detachedChild = ObjectCreationUtility.CreateMeshObject(goreMultiCut._goreSimulator, chunkClass.mesh,
                goreMultiCut.gameObject.name + " - " + chunkClass.boneName + " - " + chunkClass.cutIndexClassIndex);
                
            detachedChild.transform.SetPositionAndRotation(transform.position, transform.rotation);
                
            subModuleObjClass.obj = detachedChild;
            subModuleObjClass.mesh = chunkClass.mesh;
            if (detachedChild.TryGetComponent<Renderer>(out var _renderer))
            {
                subModuleObjClass.renderer = _renderer;
            }
            subModuleObjClass.cutCenters = chunkClass.cutCenters;
                
            var goreMesh = detachedChild.AddComponent<GoreMesh>();
            goreMesh._goreMultiCut = goreMultiCut;
            goreMesh._boneName = chunkClass.boneName;

            return subModuleObjClass;
        }


        public static Vector3 CreatePlaneNormalFromCutIndexes(Transform transform, List<Vector3> vertices, List<int> cutIndexes, 
            Vector3 boundsCenter, Vector3 cutCenter)
        {
            
            if (cutIndexes.Count < 3)
            {
                return Vector3.zero;
            }
            int step = cutIndexes.Count / 3;

            Vector3 point1 = transform.TransformPoint(vertices[cutIndexes[0 * step]]);
            Vector3 point2 = transform.TransformPoint(vertices[cutIndexes[1 * step]]);
            Vector3 point3 = transform.TransformPoint(vertices[cutIndexes[2 * step]]);

            Vector3 vec1 = point2 - point1;
            Vector3 vec2 = point3 - point1;

            Vector3 planeNormal = Vector3.Cross(vec1, vec2).normalized;
            Vector3 boundsWorld = transform.TransformPoint(boundsCenter);
            
            Vector3 directionToBoundsWorld = boundsWorld - cutCenter;
            Vector3 directionAwayFromBoundsWorld = cutCenter - boundsWorld;

            float dotProductTo = Vector3.Dot(planeNormal, directionToBoundsWorld);
            float dotProductAway = Vector3.Dot(planeNormal, directionAwayFromBoundsWorld);

            if (dotProductTo < dotProductAway) planeNormal *= (-1);
            
            return planeNormal;
        }
        
        
    }
}