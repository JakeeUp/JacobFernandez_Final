// ----------------------------------------------------
// Copyright (c) Pampel Games e.K. All Rights Reserved.
// https://www.pampelgames.com
// ----------------------------------------------------

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace PampelGames.Shared.Utility
{
    
    public static class PGCollectionsUtility
    {

        /// <summary>
        ///     Checks whether an object is a list, array or IEnumerable.
        /// </summary>
        /// <param name="obj">object to check for.</param>
        /// <returns>True if object is a collection.</returns>
        public static bool IsCollection(object obj)
        {
            if (obj.GetType().IsArray) return true;
            if (obj is IList) return true;
            if (obj is IEnumerable) return true;
            return false;
        }
        
        /// <summary>
        ///     Create a list of the specified type.
        /// </summary>
        /// <returns>New List.</returns>
        public static IList CreateListOfType(Type type)
        {
            Type genericListType = typeof(List<>);
            Type specificListType = genericListType.MakeGenericType(type);
            IList list = (IList) Activator.CreateInstance(specificListType);
            return list;
        }
        
        /// <summary>
        ///     Create an array of the specified type.
        /// </summary>
        /// <returns>New Array. Values can be accessed like this: array.GetValue(i));</returns>
        public static Array CreateArrayOfType(Type type, int length)
        {
            return Array.CreateInstance(type, length);
        }
        
        /// <summary>
        ///     Get the size of a list or array.
        /// </summary>
        /// <param name="collection">List or array as object.</param>
        /// <returns>Collection size. Returns -1 if not valid.</returns>
        public static int GetCollectionSize(object collection)
        {
            if (collection is Array array) return array.Length;
            if (collection is IList list) return list.Count;
            if (collection is IEnumerable enumerable)
            {
                int count = 0;
                foreach (var item in enumerable) count++;
                return count;
            }
            return -1;
        }

        
        /// <summary>
        ///     Get an item from a collection.
        /// </summary>
        /// <param name="collection">List or array as object.</param>
        /// <param name="index">Index of the collection.</param>
        /// <returns>Item at index. Can be null.</returns>
        public static object GetCollectionItemAtIndex(object collection, int index)
        {
            if (index < 0) return null;
            if (collection is IList list && index < list.Count) return list[index];
            if (collection is Array array && index < array.Length) return array.GetValue(index);
            if (collection is IEnumerable enumerable)
            {
                int currentIndex = 0;
                foreach (var item in enumerable)
                {
                    if (currentIndex == index) return item;
                    currentIndex++;
                }
            }
            return null;
        }
    }
}