// ----------------------------------------------------
// Copyright (c) Pampel Games e.K. All Rights Reserved.
// https://www.pampelgames.com
// ----------------------------------------------------

using System;
using System.Collections.Generic;
using System.Linq;

namespace PampelGames.Shared.Utility
{
    public static class PGStringUtility
    {
        
        /// <summary>
        ///     Remove all characters before the cutString.
        /// </summary>
        /// <param name="value">Original string.</param>
        /// <param name="cutString">Characters to look for.</param>
        /// <param name="removeCutstring">If true, removes also the cutString itself.</param>
        public static string PGCutBefore(this string value, string cutString, bool removeCutstring)
        {
            var index = value.IndexOf(cutString, StringComparison.Ordinal);
            if (index == -1) return value;
            if(removeCutstring) return value.Substring(index + cutString.Length); 
            return cutString + value.Substring(index + cutString.Length);
        }

        /// <summary>
        ///     Remove all characters after the cutString.
        /// </summary>
        /// <param name="value">Original string.</param>
        /// <param name="cutString">Characters to look for.</param>
        /// <param name="removeCutstring">If true, removes also the cutString itself.</param>
        public static string PGCutAfter(this string value, string cutString, bool removeCutstring)
        {
            var index = value.IndexOf(cutString, StringComparison.Ordinal);
            if (index == -1) return value;
            if(!removeCutstring) return value.Substring(0, index + cutString.Length);
            return value.Substring(0, index);

        }

        /// <summary>
        ///     Removes all characters before the last specified cutString.
        /// </summary>
        /// <param name="value">Original string.</param>
        /// <param name="cutString">Characters to look for.</param>
        /// <param name="removeCutstring">If true, removes also the cutString itself.</param>
        public static string PGCutBeforeLast(this string value, string cutString, bool removeCutstring)
        {
            var index = value.LastIndexOf(cutString, StringComparison.Ordinal);
            if (index == -1) return value;
            if(removeCutstring) return value.Substring(index + 1);
            return value.Substring(value.LastIndexOf(cutString, StringComparison.Ordinal));
        }
        
        /// <summary>
        ///     Removes all characters after the last specified cutString.
        /// </summary>
        /// <param name="value">The original string.</param>
        /// <param name="cutString">The characters to look for.</param>
        /// <param name="removeCutstring">If true, removes also the cutString itself.</param>
        /// <returns>The modified string.</returns>
        public static string PGCutAfterLast(this string value, string cutString, bool removeCutstring)
        {
            var index = value.LastIndexOf(cutString, StringComparison.Ordinal);
            if (index == -1) return value;
            if (removeCutstring) return value.Substring(0, index);
            return value.Substring(0, index + cutString.Length);
        }

        /// <summary>
        ///     Removes all characters between the two cutStrings.
        /// </summary>
        /// <param name="value">The original string.</param>
        /// <param name="cutString1"></param>
        /// <param name="cutString2"></param>
        /// <param name="removeCutstrings">If true, removes also the cutString itself.</param>
        /// <returns>The modified string.</returns>
        public static string PGCutBetween(this string value, string cutString1, string cutString2, bool removeCutstrings)
        {
            int startIndex = value.IndexOf(cutString1, StringComparison.Ordinal);
            if (startIndex == -1) return value;
            int endIndex = value.IndexOf(cutString2, startIndex + cutString1.Length, StringComparison.Ordinal);
            if (endIndex == -1) return value;
            if (!removeCutstrings)
            {
                startIndex += cutString1.Length;
                endIndex -= cutString2.Length;
            }
            return value.Substring(0, startIndex) + value.Substring(endIndex + cutString2.Length);
        }

        /// <summary>
        ///      Returns an array of integers representing the indexes where the string occurs.
        /// </summary>
        public static int[] GetIndexesOfString(string mainString, string subString) 
        {
            List<int> indexes = new List<int>();
            int index = mainString.IndexOf(subString, StringComparison.Ordinal);
            while (index >= 0) {
                indexes.Add(index);
                index = mainString.IndexOf(subString, index + 1, StringComparison.Ordinal);
            }
            return indexes.ToArray();
        }
        
        /// <summary>
        ///     Gets the combined, non empty character string before the specified index.
        /// </summary>
        public static string PGGetSubstringBeforeIndex(this string value, int index) 
        {
            int whitespaceIndex = value.LastIndexOf(" ", index, StringComparison.OrdinalIgnoreCase);
            if (whitespaceIndex >= 0) 
            {
                int startIndex = whitespaceIndex + 1;
                while (startIndex <= index && Char.IsWhiteSpace(value[startIndex])) 
                    startIndex++;
        
                return value.Substring(startIndex, index - startIndex + 1);
            }

            return value.Substring(0, index + 1);
        }

        /// <summary>
        ///     Gets the combined, non-empty character string after the specified index.
        /// </summary>
        public static string PGGetSubstringAfterIndex(this string value, int index)
        {
            for (int i = index; i < value.Length; i++)
            {
                if (char.IsWhiteSpace(value[i]))
                    return value.Substring(index, i - index);
            }
            return value.Substring(index);
        }


        



        
    }
}