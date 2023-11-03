// ----------------------------------------------------
// Gore Simulator
// Copyright (c) Pampel Games e.K. All Rights Reserved.
// https://www.pampelgames.com
// ----------------------------------------------------

#if UNITY_EDITOR
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace PampelGames.GoreSimulator
{
    /// <summary>
    ///     Used for the Editor Bones Tree View.
    /// </summary>
    [Serializable]
    public class BonesListClass
    {
        public Transform bone;

        /// <summary>
        ///     Used to sort the bones tree.
        /// </summary>
        public List<int> id = new();

        /// <summary>
        ///     Whether to send the OnDeath event when it is being cut.
        /// </summary>
        public bool sendOnDeath;
    }
    
    public static class BonesListClassUtility
    {
        public static void CheckAndRemoveItems(GoreSimulator _goreSimulator)
        {
            var itemsToRemove = new List<BonesListClass>();

            foreach (var item in _goreSimulator.bonesListClasses)
            {
                if (!HasAncestors(_goreSimulator.bonesListClasses, item))
                    itemsToRemove.Add(item);
            }

            foreach (var item in itemsToRemove)
                _goreSimulator.bonesListClasses.Remove(item);
        }

        private static bool HasAncestors(List<BonesListClass> list, BonesListClass item)
        {
            if(item.id.Count == 1) return true;
    
            var parentId = new List<int>(item.id);
            parentId.RemoveAt(parentId.Count - 1); 

            var parentExists = list.Any(b => b.id.SequenceEqual(parentId));

            if(parentExists)
                return HasAncestors(list, list.First(b => b.id.SequenceEqual(parentId)));
            else
                return false;
        }
    }
    
    public class BonesListClassComparer : IComparer<BonesListClass>
    {
        public int Compare(BonesListClass x, BonesListClass y)
        {
            int minLength = Math.Min(x.id.Count, y.id.Count);
            for (int index = 0; index < minLength; index++)
            {
                int comparison = x.id[index].CompareTo(y.id[index]);
                if (comparison != 0) return comparison;
            }

            return x.id.Count.CompareTo(y.id.Count);
        }
    }

}
#endif
