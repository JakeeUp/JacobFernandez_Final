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
    public static class BonesUtility
    {
        
        public static List<Transform> GetChildren(HashSet<Transform> smrBones, Transform bone, Enums.Children childrenEnum)
        {
            List<Transform> children = new List<Transform>();
            if(childrenEnum != Enums.Children.None) GetChildrenIteration(smrBones, bone, children, childrenEnum);
            return children;
        }
        
        private static void GetChildrenIteration(HashSet<Transform> smrBones, Transform bone, List<Transform> unsharedBones, Enums.Children childrenEnum)
        {
            foreach (Transform child in bone)
            {
                if (!ContainsSMRBonesInHierarchy(smrBones, child))
                {
                    if (childrenEnum == Enums.Children.Rendered)
                    {
                        if (child.TryGetComponent<Renderer>(out var renderer))
                            unsharedBones.Add(child);
                        else
                            GetChildrenIteration(smrBones, child, unsharedBones, childrenEnum);
                    }
                    else
                    {
                        unsharedBones.Add(child);
                    }
                        
                }
                else
                    GetChildrenIteration(smrBones, child, unsharedBones, childrenEnum);
            }
        }
        
        private static bool ContainsSMRBonesInHierarchy(HashSet<Transform> smrBones, Transform transform)
        {
            if (smrBones.Contains(transform)) return true;
            
            for (int i = 0; i < transform.childCount; ++i)
                if (ContainsSMRBonesInHierarchy(smrBones, transform.GetChild(i))) return true;
            
            return false;
        }
        
        /********************************************************************************************************************************/
        
        public static List<Transform> GetOrderedBones(List<Transform> bones)
        {
            // Dictionary for faster lookup
            HashSet<Transform> boneSet = new HashSet<Transform>(bones);

            // Filter the distinct bones that are present in the original bone list.
            List<Transform> orderedBones = bones.First().root.GetComponentsInChildren<Transform>()
                .Where(boneSet.Contains).ToList();

            return orderedBones;
        }

        public static List<BonesClass> GetAllChildBoneClasses(Transform parentBone, List<Transform> bones, List<BonesClass> bonesClasses)
        {
            var childBones = new List<Transform>();
            CollectChildBonesRecursive(childBones, parentBone, bones);
            List<BonesClass> childBoneClasses = bonesClasses.FindAll(bonesClass => childBones.Contains(bonesClass.bone));
            return childBoneClasses;
        }
        
        public static List<BonesClass> GetFirstChildBoneClasses(List<Transform> bones, List<BonesClass> bonesClasses)
        {
            List<BonesClass> childBoneClasses = bonesClasses.FindAll(bonesClass => bones.Contains(bonesClass.bone));
            return childBoneClasses;
        }
        
        public static List<BonesStorageClass> GetFirstChildBoneStorageClasses(List<Transform> bones, List<BonesStorageClass> bonesClasses)
        {
            List<BonesStorageClass> childBoneClasses = bonesClasses.FindAll(bonesClass =>
            {
                return bones.Exists(bone => bone.name == bonesClass.boneName);
            });

            return childBoneClasses;
        }
        public static List<Transform> GetAllChildBones(Transform parentBone, List<Transform> bones)
        {
            var childBones = new List<Transform>();
            CollectChildBonesRecursive(childBones, parentBone, bones);
            return childBones;
        }

        private static void CollectChildBonesRecursive(List<Transform> result, Transform parent, List<Transform> bones)
        {
            foreach (Transform child in parent)
            {
                if (bones.Contains(child) && !result.Contains(child))
                {
                    result.Add(child);
                }
                CollectChildBonesRecursive(result, child, bones);
            }
        }

        
        public static List<Transform> GetDirectChildBones(Transform parentBone, SkinnedMeshRenderer smr)
        {
            var boneSet = new HashSet<Transform>(smr.bones);
            var childBones = new List<Transform>();
            foreach (Transform child in parentBone)
            {
                if (boneSet.Contains(child))
                {
                    childBones.Add(child);
                }
            }
            return childBones;
        }

        
        public static Transform GetFirstParentBone(Transform bone, List<Transform> bones)
        {
            Transform currentParent = bone.parent;

            while (currentParent != null)
            {
                if (bones.Contains(currentParent)) return currentParent;
                currentParent = currentParent.parent;
            }

            return null;
        }
        
        
        public static List<Transform> GetDirectBoneChildrenSel(Transform parentBone, List<Transform> boneChildren)
        {
            List<Transform> directBoneChildren = new List<Transform>();
            foreach (Transform child in boneChildren)
            {
                if (!IsDirectChild(parentBone, boneChildren, child)) continue;
                if (!directBoneChildren.Contains(child)) directBoneChildren.Add(child);
            }

            return directBoneChildren;
        }

        private static bool IsDirectChild(Transform parentBone, List<Transform> boneChildren, Transform child, int depth = 0)
        {
            if (depth > 8) return false;
            var parent = child.parent;
            if (parent == null) return false;
            if (boneChildren.Contains(parent)) return false;
            if (parent == parentBone) return true;
            return IsDirectChild(parentBone, boneChildren, parent, depth + 1);
        }
        

        public static bool DetermineCentralBone(BonesClass bonesClass)
        {

            if (bonesClass.firstChildren.Count > 1) return true;
            

            return false;
        }
        

    }
}