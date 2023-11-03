// ----------------------------------------------------
// Gore Simulator
// Copyright (c) Pampel Games e.K. All Rights Reserved.
// https://www.pampelgames.com
// ----------------------------------------------------

using System.Collections.Generic;
using PampelGames.Shared.Tools;
using Unity.Collections;
using Unity.Jobs;
using UnityEngine;

namespace PampelGames.GoreSimulator
{
    public class MeshNativeDataClass
    {

        public NativeArray<JobHandle> jobHandles;
        public NativeArray<int> indexesNative;
        public NativeList<int> newTrianglesNative;
        public NativeHashMap<int, int> origToNewMapNative;
        public List<NativeArray<int>> trianglesNativePerSubM;
        
        
        public void InitializeRuntimeMeshData(SerializableMesh serializableMesh)
        {
            jobHandles = new NativeArray<JobHandle>(serializableMesh.subMeshCount, Allocator.Persistent);
            indexesNative = new NativeArray<int>(serializableMesh.indexes, Allocator.Persistent);
            newTrianglesNative = new NativeList<int>(serializableMesh.triangles.Length,Allocator.Persistent);
            origToNewMapNative = new NativeHashMap<int, int>(serializableMesh.triangles.Length, Allocator.Persistent);

            trianglesNativePerSubM = new List<NativeArray<int>>();
            for (int i = 0; i < serializableMesh.subMeshCount; i++) 
            {
                trianglesNativePerSubM.Add(new NativeArray<int>(serializableMesh.subMeshTriangles[i].triangles, Allocator.Persistent));
            }
        }
        
        public void DisposeRuntimeMeshData()
        {
            jobHandles.Dispose();
            indexesNative.Dispose();
            newTrianglesNative.Dispose();
            origToNewMapNative.Dispose();
            for (int i = 0; i < trianglesNativePerSubM.Count; i++)
            {
                trianglesNativePerSubM[i].Dispose();
            }
            
        }
    }
}
