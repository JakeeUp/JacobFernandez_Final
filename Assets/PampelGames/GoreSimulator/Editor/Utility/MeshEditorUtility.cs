// ----------------------------------------------------
// Gore Simulator
// Copyright (c) Pampel Games e.K. All Rights Reserved.
// https://www.pampelgames.com
// ----------------------------------------------------

using System;
using System.Collections.Generic;
using System.Linq;
using PampelGames.Shared.Editor;
using UnityEditor;
using UnityEngine;

namespace PampelGames.GoreSimulator.Editor
{
    public static class MeshEditorUtility
    {
        
        /// <summary>
        ///     Initial GoreSimulator component setup.
        /// </summary>
        public static void InitializeMeshSetup(GoreSimulator _goreSimulator)
        {
            
            _goreSimulator.bonesListClasses = new List<BonesListClass>();
            _goreSimulator.bonesListClasses.Add(new BonesListClass());
            var parentBoneListClass = _goreSimulator.bonesListClasses[0]; 
            parentBoneListClass.id.Add(0);
            
            List<string> centerBoneNames = Constants.BonesSetupCenter();
            
            for (int i = 0; i < centerBoneNames.Count; i++)
            {
                var boneIndexes = FindBoneIndexesByName(_goreSimulator.smr, centerBoneNames[i]);
                if(boneIndexes.Count == 0) continue;
                parentBoneListClass.bone = _goreSimulator.smr.bones[boneIndexes[0]];
                parentBoneListClass.sendOnDeath = true;
                break;
            }
            
            EditorUtility.SetDirty(_goreSimulator);
        }

        private static List<int> FindBoneIndexesByName(SkinnedMeshRenderer smr, string boneName)
        {
            List<int> matchingIndexes = new List<int>();

            for (int i = 0; i < smr.bones.Length; i++)
                if (smr.bones[i].name.Contains(boneName))
                    matchingIndexes.Add(i);

            boneName = char.ToUpper(boneName[0]) + boneName.Substring(1);

            for (int i = 0; i < smr.bones.Length; i++)
                if (smr.bones[i].name.Contains(boneName))
                    matchingIndexes.Add(i);

            return matchingIndexes;
        }
        
        /********************************************************************************************************************************/

        public static void AutoSetupHumanoid(GoreSimulator _goreSimulator)
        {
            if (!InitializeBoneSetup(_goreSimulator)) return;
            var bonesListClasses = _goreSimulator.bonesListClasses;

            List<Transform> bones = new List<Transform>();
            
            List<string> boneNames = Constants.BonesSetupHumanoid();

            for (int i = 0; i < boneNames.Count; i++)
            {
                var boneIndexes = FindBoneIndexesByName(_goreSimulator.smr, boneNames[i]);
                for (int j = 0; j < boneIndexes.Count; j++)
                {
                    if(!bones.Contains(_goreSimulator.smr.bones[boneIndexes[j]]))
                        bones.Add(_goreSimulator.smr.bones[boneIndexes[j]]);
                }
            }

            var removeDuplicated = Constants.BonesSetupDuplicated();
            for (int i = 0; i < removeDuplicated.Count; i++) RemoveDuplicated(bones, removeDuplicated[i]);    
            
            
            // foreach (var bone in _goreSimulator.smr.bones) AddBoneToClass(bone, bones, bonesListClasses);
            // Parent bones first:
            var rootBones = _goreSimulator.smr.bones.Where(bone => bone.parent == null);
            var childBones = _goreSimulator.smr.bones.Except(rootBones);

            var orderedBones = rootBones.Concat(childBones.OrderBy(bone => GetDepth(bone)));                

            foreach (var bone in orderedBones) 
                AddBoneToClass(bone, bones, bonesListClasses);

        }
        
        public static void AutoSetupGeneric(GoreSimulator _goreSimulator)
        {
            if (!InitializeBoneSetup(_goreSimulator)) return;
            var bonesListClasses = _goreSimulator.bonesListClasses;
            
            List<Transform> bones = new List<Transform>(_goreSimulator.smr.bones);

            var removeDuplicated = Constants.BonesSetupDuplicated();
            for (int i = 0; i < removeDuplicated.Count; i++) RemoveDuplicated(bones, removeDuplicated[i]);   

            
            // foreach (var bone in _goreSimulator.smr.bones) AddBoneToClass(bone, bones, bonesListClasses);
            // Parent bones first:
            var rootBones = _goreSimulator.smr.bones.Where(bone => bone.parent == null);
            var childBones = _goreSimulator.smr.bones.Except(rootBones);

            var orderedBones = rootBones.Concat(childBones.OrderBy(bone => GetDepth(bone)));                

            foreach (var bone in orderedBones) 
                AddBoneToClass(bone, bones, bonesListClasses);

            for (int i = bonesListClasses.Count - 1; i >= 0; i--)
            {
                if(bonesListClasses[i].id.Count > 6) bonesListClasses.RemoveAt(i);  
            }
        }
        
        /********************************************************************************************************************************/
        
        private static int GetDepth(Transform transform)
        {
            int depth = 0;
            while (transform.parent != null)
            {
                depth++;
                transform = transform.parent;
            }
            return depth;
        }
        
        private static bool InitializeBoneSetup(GoreSimulator _goreSimulator)
        {
            var centerBonesListClass = _goreSimulator.bonesListClasses[0];
            if (centerBonesListClass.bone == null)
            {
                PGEditorMessages.PopUpMessage("Missing Bone", "Please assign the center bone first.");
                return false;
            }
            var bonesListClasses = _goreSimulator.bonesListClasses;
            bonesListClasses.Clear();
            bonesListClasses.Add(centerBonesListClass);
            return true;
        }

        private static void RemoveDuplicated(List<Transform> bones, string boneName)
        {
            List<Transform> spineBones = bones.FindAll(b => b.name.Contains(boneName));

            if(spineBones.Count > 1)
            {
                int middleIndex = spineBones.Count / 2;
                Transform middleBone = spineBones[middleIndex];

                foreach(Transform bone in spineBones)
                    if (bone != middleBone) bones.Remove(bone);
            }
            
            boneName = char.ToUpper(boneName[0]) + boneName.Substring(1);
            
            spineBones = bones.FindAll(b => b.name.Contains(boneName));

            if(spineBones.Count > 1)
            {
                int middleIndex = spineBones.Count / 2;
                Transform middleBone = spineBones[middleIndex];

                foreach(Transform bone in spineBones)
                    if (bone != middleBone) bones.Remove(bone);
            }
        }
        
                
        private static void AddBoneToClass(Transform currentBone, List<Transform> bones, List<BonesListClass> bonesListClasses)
        {
            if (!bones.Contains(currentBone) || bonesListClasses.Any(b => b.bone == currentBone)) return;
            
            List<int> newID = new List<int>();
            newID.Add(0);
        
            Transform currentLevel = currentBone;
            Transform parent = null;
            for (;;)
            {
                if (currentLevel.parent == null) break;
                parent = currentLevel.parent;
                currentLevel = currentLevel.parent;
                if (bones.Contains(parent)) break;
            }
                
            if (parent != null && bones.Contains(parent))
            {
                BonesListClass parentItem = bonesListClasses.FirstOrDefault(b => b.bone == parent);
                
                newID = new List<int>(parentItem.id);
                
                var sameLevelItems = bonesListClasses
                    .Where(_item => _item.id.Count == newID.Count + 1 && _item.id.Take(newID.Count).SequenceEqual(newID)).ToList();
                int nextAvailableId = 0;
                if (sameLevelItems.Any())
                {
                    var maxId = sameLevelItems.Max(_item => _item.id.Last());
                    nextAvailableId = maxId + 1;
                }
        
                newID.Add(nextAvailableId);
            }

            var sendOnDeath = false;
            List<string> onDeathBoneNames = Constants.HeadBones();
            List<string> headBoneNames = Constants.CenterBones();
            onDeathBoneNames.AddRange(headBoneNames);
            for (int i = 0; i < onDeathBoneNames.Count; i++)
            {
                if (currentBone.name.IndexOf(onDeathBoneNames[i], StringComparison.OrdinalIgnoreCase) >= 0) sendOnDeath = true;
            }
            
            bonesListClasses.Add(new BonesListClass
            {
                id = newID,
                bone = currentBone,
                sendOnDeath = sendOnDeath
            });
        
            foreach (Transform child in currentBone) AddBoneToClass(child, bones, bonesListClasses);
        }


        
        /********************************************************************************************************************************/
        
        
        /// <summary>
        ///     Returns the list of vertices that are influenced by the specified bone.
        /// </summary>
        public static List<int> GetBoneIndexes(SkinnedMeshRenderer smr, Transform bone, float weightThreshold)
        {
            var bones = smr.bones;
            var boneIndex = Array.IndexOf(bones, bone);
            if (boneIndex == -1) return new List<int>();
            var boneWeights = smr.sharedMesh.boneWeights;
            var resultList = new List<int>();
            for (var i = 0; i < boneWeights.Length; i++)
                if ((boneWeights[i].boneIndex0 == boneIndex && boneWeights[i].weight0 > weightThreshold) ||
                    (boneWeights[i].boneIndex1 == boneIndex && boneWeights[i].weight1 > weightThreshold) ||
                    (boneWeights[i].boneIndex2 == boneIndex && boneWeights[i].weight2 > weightThreshold) ||
                    (boneWeights[i].boneIndex3 == boneIndex && boneWeights[i].weight3 > weightThreshold)
                   )
                {
                    resultList.Add(i);
                }
            
            return resultList;
        }
        
    }
}