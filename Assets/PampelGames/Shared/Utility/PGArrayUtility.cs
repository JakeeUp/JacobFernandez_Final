// ----------------------------------------------------
// Copyright (c) Pampel Games e.K. All Rights Reserved.
// https://www.pampelgames.com
// ----------------------------------------------------

using System;

namespace PampelGames.Shared.Utility
{
    public static class PGArrayUtility
    {
        public static void Insert<T>(ref T[] array, int index, T item)
        {
            if (index < 0 || index > array.Length) return;
            var newArray = new T[array.Length + 1];
            Array.Copy(array, 0, newArray, 0, index);
            newArray[index] = item;
            Array.Copy(array, index, newArray, index + 1, array.Length - index);
            array = newArray;
        }

        public static void Add<T>(ref T[] array, T item)
        {
            Array.Resize(ref array, array.Length + 1);
            array[^1] = item;
        }

        public static void RemoveAt<T>(ref T[] array, int index)
        {
            if (index < 0 || index >= array.Length) return;
            var newArray = new T[array.Length - 1];
            Array.Copy(array, 0, newArray, 0, index);
            Array.Copy(array, index + 1, newArray, index, array.Length - index - 1);
            array = newArray;
        }

        /// <summary>
        ///     Move each item up in the hierarchy and wrapping around to the bottom when necessary.
        /// </summary>
        /// <param name="amount">Indexes each item moves up.</param>
        /// <param name="startIdx">Optional start index.</param>
        /// <param name="endIdx">Optional end index.</param>
        public static void ReorderUp<T>(ref T[] array, int amount, int startIdx = 0, int endIdx = -1)
        {
            if (endIdx == -1) endIdx = array.Length - 1;
            if (startIdx < 0 || startIdx >= array.Length) return;
            if (endIdx < 0 || endIdx >= array.Length) return;
            var length = array.Length;
            var tempArray = new T[length];
            amount %= length;
            for (var i = startIdx; i <= endIdx; i++)
            {
                var newIndex = (i - amount - startIdx + length) % (endIdx - startIdx + 1) + startIdx;
                tempArray[newIndex] = array[i];
            }

            for (var i = 0; i < length; i++)
                if (i >= startIdx && i <= endIdx)
                    array[i] = tempArray[i];
        }
    }
}