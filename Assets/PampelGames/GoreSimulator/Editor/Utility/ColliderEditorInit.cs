// ----------------------------------------------------
// Gore Simulator
// Copyright (c) Pampel Games e.K. All Rights Reserved.
// https://www.pampelgames.com
// ----------------------------------------------------

using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Object = UnityEngine.Object;

namespace PampelGames.GoreSimulator.Editor
{
    internal static class ColliderEditorInit
    {
        public static void CreateColliders(SkinnedMeshRenderer smr, Mesh bakedMesh, List<BonesClass> bonesClasses,
            List<BonesStorageClass> bonesStorageClasses)
        {
            var centerMesh = CreateTempMesh(bonesStorageClasses[^1], bakedMesh);
            var centerMinBound = Mathf.Min(centerMesh.bounds.extents.x, Mathf.Min(centerMesh.bounds.extents.y, centerMesh.bounds.extents.z));
            Object.DestroyImmediate(centerMesh);

            for (var i = 0; i < bonesClasses.Count; i++)
            {
                var bonesClass = bonesClasses[i];
                var bonesStorageClass = bonesStorageClasses[i];
                var goreBone = bonesClass.bone.GetComponent<GoreBone>();
                if(goreBone._collider != null) continue;
                var tempMesh = CreateTempMesh(bonesStorageClass, bakedMesh);
                var collider = CreateCollider(bonesClass, smr, tempMesh, bonesClasses);
                collider.radius = centerMinBound;
                if (!bonesClass.centralBone) collider.radius *= 0.5f;
                
                goreBone._collider = collider;
                Object.DestroyImmediate(tempMesh);
            }
        }

        /********************************************************************************************************************************/

        private static Mesh CreateTempMesh(BonesStorageClass bonesStorageClass, Mesh bakedMesh)
        {
            var mesh = new Mesh();
            var tempVertices = new List<Vector3>();
            for (var i = 0; i < bonesStorageClass.cutIndexClasses.Count; i++)
            {
                var chunkIndexes = bonesStorageClass.cutIndexClasses[i].chunkIndexes;

                for (var j = 0; j < chunkIndexes.Count; j++) tempVertices.Add(bakedMesh.vertices[chunkIndexes[j]]);
            }

            mesh.vertices = tempVertices.ToArray();
            mesh.RecalculateBounds();
            return mesh;
        }
        
        private static CapsuleCollider CreateCollider(BonesClass bonesClass, SkinnedMeshRenderer smr, Mesh tempMesh, List<BonesClass> bonesClasses)
        {
            var collider = bonesClass.bone.gameObject.AddComponent<CapsuleCollider>();

            // Using bone's relevant child or parent to determine the direction
            var relevantBone = GetRelevantDirectionalBone(bonesClass.bone, smr.bones);
            if (relevantBone != null)
            {
                var localDirection = bonesClass.bone.InverseTransformDirection(relevantBone.position - bonesClass.bone.position);
                collider.direction = DominantAxis(localDirection); // x = 0, y = 1, z = 2
            }

            var bounds = tempMesh.bounds;
            var minBound = Mathf.Min(bounds.size.x, Mathf.Min(bounds.size.y, bounds.size.z));
            var maxBound = Mathf.Max(bounds.size.x, Mathf.Max(bounds.size.y, bounds.size.z));

            collider.height = maxBound;

            var center = bonesClass.bone.position;
            var childPosition = DetermineChildPosition(bonesClass, smr.bones, bonesClasses);
            if (childPosition != Vector3.zero)
            {
                collider.height = Vector3.Distance(center, childPosition);
                center = (center + childPosition) / 2;
            }

            var localCenter = collider.transform.InverseTransformPoint(center);
            collider.center = localCenter;

            return collider;
        }

        private static Transform GetRelevantDirectionalBone(Transform bone, Transform[] allBones)
        {
            for (int i = 0; i < bone.childCount; i++)
            {
                var child = bone.GetChild(i);
                if (Array.IndexOf(allBones, child) >= 0)
                    return child;
            }

            if (bone.parent != null && Array.IndexOf(allBones, bone.parent) >= 0)
                return bone.parent;
    
            return null;
        }

        private static int DominantAxis(Vector3 v3)
        {
            v3 = new Vector3(Mathf.Abs(v3.x), Mathf.Abs(v3.y), Mathf.Abs(v3.z));
            if (v3.x > v3.y)
            {
                if (v3.x > v3.z) return 0;
                return 2;
            }
            else
            {
                if (v3.y > v3.z) return 1;
                return 2;
            }
        }
        
        

        private static Vector3 DetermineChildPosition(BonesClass bonesClass, Transform[] bones, List<BonesClass> bonesClasses)
        {
            if (bonesClass.firstChildren.Count == 1) return bonesClass.firstChildren[0].position;

            var sumOfPositions = Vector3.zero;
            var count = 0;

            if (bonesClass.firstChildren.Count == 0)
            {
                SearchChildBones(bonesClass.bone);

                void SearchChildBones(Transform currentBone)
                {
                    foreach (Transform childTransform in currentBone)
                    {
                        if (Array.Exists(bones, bone => bone == childTransform))
                        {
                            sumOfPositions += childTransform.position;
                            count++;
                        }

                        SearchChildBones(childTransform);
                    }
                }
            }
            else
            {
                // If multiple children (expecially for center), the average child positions could be in the middle. 
                // So trying to get one direction.
                var childBonesClasses = new List<BonesClass>();
                foreach (var classFirstChild in bonesClass.firstChildren)
                    childBonesClasses.Add(bonesClasses.FirstOrDefault(b => b.bone == classFirstChild));

                var grouped = childBonesClasses.GroupBy(b => b.firstChildren.Count).ToList();
                var uniqueItem = grouped.FirstOrDefault(group => group.Count() == 1)?.First();
                if (uniqueItem != null)
                    return uniqueItem.bone.position;
                for (var i = 0; i < bonesClass.firstChildren.Count; i++)
                {
                    sumOfPositions += bonesClass.firstChildren[i].position;
                    count++;
                }
            }

            if (count > 0)
            {
                var averagePosition = sumOfPositions / count;
                return averagePosition;
            }

            return Vector3.zero;
        }
        
    }
}