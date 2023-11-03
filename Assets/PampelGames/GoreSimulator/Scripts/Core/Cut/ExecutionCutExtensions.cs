// ----------------------------------------------------
// Gore Simulator
// Copyright (c) Pampel Games e.K. All Rights Reserved.
// https://www.pampelgames.com
// ----------------------------------------------------

using System.Collections.Generic;
using UnityEngine;

namespace PampelGames.GoreSimulator
{
    public static class ExecutionCutExtensions
    {
        
        public static void RemoveIndexes(this ExecutionCutClass executionCutClass, int index)
        {
            executionCutClass.cutIndexes.RemoveAt(index);
            executionCutClass.sewIndexes.RemoveAt(index);
            executionCutClass.sewTriangles.RemoveAt(index);
        }

        public static void ClearIndexes(this ExecutionCutClass executionCutClass)
        {
            executionCutClass.newIndexes.Clear();
            
            executionCutClass.cutIndexes.Clear();
            executionCutClass.sewIndexes.Clear();
            executionCutClass.sewTriangles.Clear();
        }

        public static void AddExecutionIndexes(this ExecutionCutClass executionCutClass, List<int> cutIndexes, List<int> sewIndexes, List<int> sewTriangles)
        {
            executionCutClass.cutIndexes.Add(cutIndexes);
            executionCutClass.sewIndexes.Add(sewIndexes);
            executionCutClass.sewTriangles.Add(sewTriangles);
        }

        /********************************************************************************************************************************/
        
        public static void ResetCutted(this BonesClass bonesClass)
        {
            bonesClass.cutted = false;
            bonesClass.cuttedIndex = -1;
            bonesClass.firstChildCutted = false;
        }
        
    }
}