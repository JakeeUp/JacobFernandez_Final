// ----------------------------------------------------
// Copyright (c) Pampel Games e.K. All Rights Reserved.
// https://www.pampelgames.com
// ----------------------------------------------------

using System.Collections.Generic;
#if UNITY_EDITOR
using UnityEditor;
#endif
using UnityEngine;

namespace PampelGames.Shared.Utility
{
    public static class PGSkinnedMeshUtility 
    {
 
        /// <summary>
        ///     Combines the Skinned Mesh Renderers into a new one.
        /// </summary>
        /// <param name="rootObj">GameObject to attach the renderer to.</param>
        /// <param name="renderers">List of SMR.</param>
        /// <param name="saveMesh">Save the Mesh into the project (Editor only).</param>
        /// <returns></returns>
        public static bool CombineSkinnedMeshes(GameObject rootObj, List<SkinnedMeshRenderer> renderers, bool saveMesh)
        {
            var allBones = new List<Transform>();
            var allBoneWeights = new List<BoneWeight>();
            var allCombineInstances = new List<CombineInstance>();
            var allBindPoses = new List<Matrix4x4>();
            var allMaterials = new List<Material>();

            int boneOffset = 0;
            
            foreach (var renderer in renderers)
            {
                // Gather the bones, bind poses and materials
                allBones.AddRange(renderer.bones);
                allMaterials.AddRange(renderer.sharedMaterials);

                // Adjust the boneweights
                BoneWeight[] boneWeights = renderer.sharedMesh.boneWeights;
                for (int i = 0; i < boneWeights.Length; i++)
                {
                    BoneWeight boneWeight = boneWeights[i];
                    boneWeight.boneIndex0 += boneOffset;
                    boneWeight.boneIndex1 += boneOffset;
                    boneWeight.boneIndex2 += boneOffset;
                    boneWeight.boneIndex3 += boneOffset;
                    allBoneWeights.Add(boneWeight);
                }

                boneOffset += renderer.bones.Length;
        
                // Adjust the bind poses
                for (int i = 0; i < renderer.sharedMesh.bindposes.Length; ++i)
                {
                    allBindPoses.Add(renderer.sharedMesh.bindposes[i] * renderer.transform.worldToLocalMatrix);
                }

                CombineInstance combineInstance = new CombineInstance
                {
                    mesh = renderer.sharedMesh,
                    transform = renderer.transform.localToWorldMatrix,
                };
                allCombineInstances.Add(combineInstance);
            }

            SkinnedMeshRenderer combinedRenderer = rootObj.GetComponent<SkinnedMeshRenderer>();
            if (combinedRenderer == null) combinedRenderer = rootObj.AddComponent<SkinnedMeshRenderer>();

            
            /********************************************************************************************************************************/
            // Move all bone weights to the first duplicated bone.
            for (int i = 0; i < allBoneWeights.Count; i++)
            {
                var boneWeight = allBoneWeights[i];
    
                // Check if the bone in boneWeight is duplicated in allBones
                var firstIndex = allBones.IndexOf(allBones[boneWeight.boneIndex0]);
                if (firstIndex != -1 && firstIndex != boneWeight.boneIndex0)
                {
                    boneWeight.boneIndex0 = firstIndex;
                    allBoneWeights[i] = boneWeight; // Assign modified instance back to the list
                }

                firstIndex = allBones.IndexOf(allBones[boneWeight.boneIndex1]);
                if (firstIndex != -1 && firstIndex != boneWeight.boneIndex1)
                {
                    boneWeight.boneIndex1 = firstIndex;
                    allBoneWeights[i] = boneWeight;
                }

                firstIndex = allBones.IndexOf(allBones[boneWeight.boneIndex2]);
                if (firstIndex != -1 && firstIndex != boneWeight.boneIndex2)
                {
                    boneWeight.boneIndex2 = firstIndex;
                    allBoneWeights[i] = boneWeight;
                }

                firstIndex = allBones.IndexOf(allBones[boneWeight.boneIndex3]);
                if (firstIndex != -1 && firstIndex != boneWeight.boneIndex3)
                {
                    boneWeight.boneIndex3 = firstIndex;
                    allBoneWeights[i] = boneWeight;
                }
            }
            
            // Move all bindPoses to the first duplicated bone.
            for (int i = 0; i < allBindPoses.Count; i++)
            {
                // Get the related bone for this bindPose
                var bone = allBones[i];

                // Check if this bone is duplicated in allBones
                var firstIndex = allBones.IndexOf(bone);
                if (firstIndex != -1 && firstIndex != i)
                {
                    // If the bone is duplicated, assign the bindPose of the first instance to this bindPose
                    var bindPose = allBindPoses[firstIndex];
                    allBindPoses[i] = bindPose;
                }
            }
            /********************************************************************************************************************************/
            
            
            Mesh mesh = new Mesh();
            mesh.CombineMeshes(allCombineInstances.ToArray(), false, true);
            mesh.boneWeights = allBoneWeights.ToArray();
            mesh.bindposes = allBindPoses.ToArray();
            mesh.RecalculateBounds();

            combinedRenderer.sharedMesh = mesh;
            combinedRenderer.sharedMaterials = allMaterials.ToArray();
            combinedRenderer.bones = allBones.ToArray();
            combinedRenderer.rootBone = GetMostUpperRootBone(renderers);
            
            
#if UNITY_EDITOR
            if (saveMesh)
            {
                if(!SaveMesh(mesh)) return false;
            }
#endif
            return true;
        }

        private static Transform GetMostUpperRootBone(List<SkinnedMeshRenderer> renderers)
        {
            Transform mostUpperRootBone = null;
            int highestDepth = int.MaxValue;

            foreach (var renderer in renderers)
            {
                int depth = 0;
                Transform parent = renderer.rootBone;

                while (parent != null)
                {
                    depth++;
                    parent = parent.parent;
                }

                if (depth < highestDepth)
                {
                    highestDepth = depth;
                    mostUpperRootBone = renderer.rootBone;
                }
            }

            return mostUpperRootBone;
        }
        
        
        

#if UNITY_EDITOR
        private static bool SaveMesh(Mesh mesh)
        {
            string defaultPath = "Assets/";
            string defaultName = "CombinedSkinnedMesh.asset";
            string message = "Save Combined Skinned Mesh";
            string defaultExtension = "asset";

            string savePath = EditorUtility.SaveFilePanelInProject(message, defaultName, defaultExtension, 
                "Please enter a file name to save the mesh to.", defaultPath);
            if (string.IsNullOrEmpty(savePath)) return false;
            
            AssetDatabase.CreateAsset(mesh, savePath);
            AssetDatabase.SaveAssets();
            return true;

        }
#endif
    }
}