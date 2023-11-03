// ----------------------------------------------------
// Gore Simulator
// Copyright (c) Pampel Games e.K. All Rights Reserved.
// https://www.pampelgames.com
// ----------------------------------------------------

using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Pool;

namespace PampelGames.GoreSimulator
{
    public class SubModuleClass
    {
        public Vector3 cutPosition = Vector3.zero;
        public Vector3 cutDirection = Vector3.zero;
        public Vector3 position = Vector3.zero;
        public Vector3 force = Vector3.zero;
        public Vector3 centerPosition = Vector3.zero;

        public GameObject parent;
        public List<Transform> children = new();
        public Mesh centerMesh;
        
        public bool subRagdoll;
        public bool multiCut;
        
        public readonly List<SubModuleObjectClass> subModuleObjectClasses = new();
        
        internal Transform centerBone; // Does only apply to Cut, not explosion. 
    }

    public class SubModuleObjectClass
    {
        public List<Vector3> cutCenters = new();
        public Vector3 centerPosition = Vector3.zero;

        public GameObject obj;
        public Mesh mesh;
        public Renderer renderer;
        
        public Vector3 force = Vector3.zero;
    }
    
    internal static class ExecutionClassesUtility
    {
        internal static SubModuleClass GetPoolSubModuleClass()
        {
            var executionClass = GenericPool<SubModuleClass>.Get();
            return executionClass;
        }
        
        internal static void ReleaseSubModuleClass(SubModuleClass subModuleClass)
        {
            subModuleClass.cutPosition = Vector3.zero;
            subModuleClass.cutDirection = Vector3.zero;
            subModuleClass.position = Vector3.zero;
            subModuleClass.force = Vector3.zero;
            subModuleClass.centerPosition = Vector3.zero;
            
            subModuleClass.parent = null;
            subModuleClass.children.Clear();
            subModuleClass.centerMesh = null;
            subModuleClass.centerBone = null;

            subModuleClass.subRagdoll = false;
            subModuleClass.multiCut = false;
            
            subModuleClass.subModuleObjectClasses.Clear();

            GenericPool<SubModuleClass>.Release(subModuleClass);
        }
        
        internal static ExecutionCutClass GetPoolExecutionCutClass()
        {
            var executionCutClass = GenericPool<ExecutionCutClass>.Get();
            return executionCutClass;
        }
        
        internal static void ReleaseExecutionCutClass(ExecutionCutClass executionCutClass)
        {
            executionCutClass.newIndexes.Clear();
            executionCutClass.cutIndexes.Clear();
            executionCutClass.sewIndexes.Clear();
            executionCutClass.sewTriangles.Clear();
            
            GenericPool<ExecutionCutClass>.Release(executionCutClass);
        }
    }
    
}
