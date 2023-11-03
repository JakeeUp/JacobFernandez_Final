// ----------------------------------------------------
// Gore Simulator
// Copyright (c) Pampel Games e.K. All Rights Reserved.
// https://www.pampelgames.com
// ----------------------------------------------------

using System;
using System.Collections.Generic;
using System.Linq;
using Unity.Burst;
using Unity.Collections;
using Unity.Jobs;
using Unity.Mathematics;
using UnityEngine;

namespace PampelGames.GoreSimulator
{
    [BurstCompile]
    public static class MeshCutJobs
    {
        
        
        public static void GetVerticesCutSides(List<int> indexes, Mesh bakedMesh, Vector3 cutPoint, Vector3 planeNormal,  Transform transform,
            out List<int> indexesOnCutSide, out List<int> indexesOnNonCutSide)
        {

            Vector3 localCutPoint = transform.InverseTransformPoint(cutPoint);
            Vector3 localPlaneNormal = transform.InverseTransformDirection(planeNormal);

            var indexesArray = new NativeArray<int>(indexes.ToArray(), Allocator.TempJob);
            var verticesArray = new NativeArray<Vector3>(bakedMesh.vertices, Allocator.TempJob);
            
            var indexesOnCutSideNative = new NativeList<int>(Allocator.TempJob);
            var indexesOnNonCutSideNative = new NativeList<int>(Allocator.TempJob);

            var job = new GetVerticesOnCutSideJob
            {
                count = indexes.Count,
                indexes = indexesArray,
                vertices = verticesArray,
                cutPoint = localCutPoint,
                planeNormal = localPlaneNormal,
                indexesOnCutSide = indexesOnCutSideNative,
                indexesOnNonCutSide = indexesOnNonCutSideNative,
            };

            JobHandle handle = job.Schedule();
            handle.Complete();

            indexesOnCutSide = new List<int>(indexesOnCutSideNative.AsArray());
            indexesOnNonCutSide = new List<int>(indexesOnNonCutSideNative.AsArray());

            indexesArray.Dispose();
            verticesArray.Dispose();
            indexesOnCutSideNative.Dispose();
            indexesOnNonCutSideNative.Dispose();
        }

        [BurstCompile]
        private struct GetVerticesOnCutSideJob : IJob
        {
            [ReadOnly] public int count;
            [ReadOnly] public NativeArray<int> indexes;
            [ReadOnly] public NativeArray<Vector3> vertices;
            [ReadOnly] public float3 cutPoint;
            [ReadOnly] public float3 planeNormal;

            public NativeList<int> indexesOnCutSide;
            public NativeList<int> indexesOnNonCutSide;

            public void Execute()
            {
                for (int i = 0; i < count; i++)
                {
                    if (indexes[i] < vertices.Length)
                    {
                        float3 worldVertex = vertices[indexes[i]]; // Assume the vertex is in the world space
                        float3 worldVertexToCutPoint = worldVertex - cutPoint;

                        // Dot product > 0 means it's on the positive side of the plane.
                        if (math.dot(worldVertexToCutPoint, planeNormal) > 0)
                        {
                            indexesOnCutSide.Add(indexes[i]);
                        }
                        else
                        {
                            indexesOnNonCutSide.Add(indexes[i]);
                        }
                    }
                }
            }
        }
        
        /********************************************************************************************************************************/
        
        
        
        public static void GetDirectConnections(List<int> indexesCut,List<int> indexesNonCut, Dictionary<int, List<int>> adjacencyList,
            out List<int> connectionsCut)
        {
            var indexesCutNative = new NativeList<int>(indexesCut.Count, Allocator.TempJob);
            var indexesNonCutNative = new NativeList<int>(indexesNonCut.Count, Allocator.TempJob);
            for (var i = 0; i < indexesCut.Count; i++) indexesCutNative.Add(indexesCut[i]);
            for (var i = 0; i < indexesNonCut.Count; i++) indexesNonCutNative.Add(indexesNonCut[i]);
        
            var trianglesCutNative = new NativeList<int>(0, Allocator.TempJob);
        
            var connectionsCutNative = new NativeList<int>(0, Allocator.TempJob);
        
            foreach (var vertexIndex in indexesCutNative)
                if (adjacencyList.TryGetValue(vertexIndex, out var triangles))
                    foreach (var tri in triangles) trianglesCutNative.Add(tri);
            
            var findConnectionsJobCut = new FindConnectionsJob
            {
                TrianglesNative = trianglesCutNative,
                IndexesNative = indexesNonCutNative,
                ConnectionsNative = connectionsCutNative
            };
            JobHandle cutJobHandle = findConnectionsJobCut.Schedule();
            cutJobHandle.Complete();
            
            connectionsCut = new List<int>(connectionsCutNative.AsArray());
        
            indexesCutNative.Dispose();
            indexesNonCutNative.Dispose();
            trianglesCutNative.Dispose();
            connectionsCutNative.Dispose();
        }
        
        
        [BurstCompile]
        private struct FindConnectionsJob : IJob
        {
            [ReadOnly] public NativeList<int> TrianglesNative;
            [ReadOnly] public NativeList<int> IndexesNative;
            public NativeList<int> ConnectionsNative;


            public void Execute()
            {
                for (var index = 0; index < TrianglesNative.Length; index++)
                {
                    var adjacentVertex = TrianglesNative[index];
                    if (IndexesNative.Contains(adjacentVertex) && !ConnectionsNative.Contains(adjacentVertex))
                        ConnectionsNative.Add(adjacentVertex);
                }
            }
        }
        
        
        /********************************************************************************************************************************/
        
        
        public static List<int> GetOppositeIndexes(MeshNativeDataClass meshNativeDataClass, List<int> indexes)
        {
            NativeHashSet<int> indexSet = new NativeHashSet<int>(indexes.Count, Allocator.TempJob);
            foreach (var index in indexes) indexSet.Add(index);
            NativeList<int> nativeOppositeIndexes = new NativeList<int>(Allocator.TempJob);

            GetOppositeIndexJob job = new GetOppositeIndexJob
            {
                allIndexes = meshNativeDataClass.indexesNative,
                indexSet = indexSet,
                oppositeIndexes = nativeOppositeIndexes
            };

            JobHandle handle = job.Schedule();
            handle.Complete();

            var oppositeIndexes = nativeOppositeIndexes.AsArray().ToList();
                
            nativeOppositeIndexes.Dispose();
            indexSet.Dispose();

            return oppositeIndexes;
        }
        
        [BurstCompile]
        struct GetOppositeIndexJob : IJob
        {
            [ReadOnly] public NativeArray<int> allIndexes;
            [ReadOnly] public NativeHashSet<int> indexSet;
            public NativeList<int> oppositeIndexes;

            public void Execute()
            {
                for (int i = 0; i < allIndexes.Length; i++)
                {
                    if (!indexSet.Contains(allIndexes[i]))
                    {
                        oppositeIndexes.Add(allIndexes[i]);
                    }
                }
            }
        }
        
        /********************************************************************************************************************************/
        
        
        public static void IndexesSnapshotExplosion(Transform smrTransform, ChunkClass chunkClass, List<Vector3> bakedVertices)
        {
            var newVertices = chunkClass.mesh.vertices;
            var cutCenters = new List<Vector3>();
            
            for (var i = 0; i < chunkClass.keys.Count; i++)
            {
                var oldIndex = chunkClass.keys[i];
                var newIndex = chunkClass.values[i];
                newVertices[newIndex] = bakedVertices[oldIndex];
            }
            
            // Moving sewIndexes to the middle.
            for (int i = 0; i < chunkClass.indexClasses.Count; i++)
            {
                var indexClass = chunkClass.indexClasses[i]; 
                if(indexClass.cutIndexes.Count == 0) continue;
                Vector3 sum = Vector3.zero;
                int division = indexClass.cutIndexes.Count / 4; // Only 4, less expensive.
                for (var j = 0; j < 4; j++)
                {
                    int index = indexClass.cutIndexes[division * j];
                    sum += newVertices[index];
                }
                cutCenters.Add(sum / 4);
                for (int j = 0; j < indexClass.sewIndexes.Count; j++) newVertices[indexClass.sewIndexes[j]] = cutCenters[^1];
            }

            for (var i = 0; i < cutCenters.Count; i++) cutCenters[i] = smrTransform.TransformPoint(cutCenters[i]);
            
            chunkClass.cutCenters = cutCenters;
            chunkClass.mesh.SetVertices(newVertices);
        }
        
        
        public static void IndexesSnapshotCut(Transform smrTransform, MeshDataClass meshDataClass, MeshNativeDataClass meshNativeDataClass, 
            Mesh bakedMesh, ExecutionCutClass executionCutClass, Mesh mesh, bool baked, out List<Vector3> cutCenters)
        {
            
            cutCenters = new List<Vector3>();
            mesh.triangles = Array.Empty<int>();
            
            List<int> newIndexes = new List<int>(executionCutClass.newIndexes);

            // Adding sewIndexes to the newIndexes.
            for (int i = 0; i < executionCutClass.sewIndexes.Count; i++)
            {
                newIndexes.AddRange(executionCutClass.sewIndexes[i]);
            }
            
            
            var originalBakedVertices = baked ? bakedMesh.vertices : meshDataClass.serializableMesh.vertices;
            Vector3[] bakedVertices = new Vector3[originalBakedVertices.Length];
            Array.Copy(originalBakedVertices, bakedVertices, originalBakedVertices.Length);

            // Moving sewIndexes to the middle.
            for (int i = 0; i < executionCutClass.cutIndexes.Count; i++)
            {
                if(executionCutClass.cutIndexes[i].Count == 0) continue;
                Vector3 sum = Vector3.zero;
                int division = executionCutClass.cutIndexes[i].Count / 4; // Only 4, less expensive.
                for (var j = 0; j < 4; j++)
                {
                    int index = executionCutClass.cutIndexes[i][division * j];
                    sum += originalBakedVertices[index];
                }
                cutCenters.Add(sum / 4);
                for (int j = 0; j < executionCutClass.sewIndexes[i].Count; j++) 
                    bakedVertices[executionCutClass.sewIndexes[i][j]] = cutCenters[^1];
            }
            
            
            
            var bakedNormals = baked ? bakedMesh.normals : meshDataClass.serializableMesh.normals;
            var bakedTangents = baked ? bakedMesh.tangents : meshDataClass.serializableMesh.tangents;
        
            var newVertices = new Vector3[newIndexes.Count];
            var newNormals = new Vector3[newIndexes.Count];
            var newTangents = new Vector4[newIndexes.Count];
            var newBoneWeights = new BoneWeight[newIndexes.Count];
            var newUV = new Vector2[newIndexes.Count];
        
            var newTrianglesPerSubmesh = new List<NativeList<int>>();
            for (int i = 0; i < bakedMesh.subMeshCount; i++) 
            {
                newTrianglesPerSubmesh.Add(new NativeList<int>(Allocator.TempJob));
            }
            
            meshNativeDataClass.origToNewMapNative.Clear();
            
            var newIndex = 0;
            foreach (var vertexIndex in newIndexes)
            {
                meshNativeDataClass.origToNewMapNative[vertexIndex] = newIndex;
                newVertices[newIndex] = bakedVertices[vertexIndex];
                newNormals[newIndex] = bakedNormals[vertexIndex];
                newTangents[newIndex] = bakedTangents[vertexIndex];
                newBoneWeights[newIndex] = meshDataClass.boneWeights[vertexIndex];
                newUV[newIndex] = meshDataClass.serializableMesh.uv[vertexIndex];
                newIndex++;
            }
            
            for (int i = 0; i < bakedMesh.subMeshCount; i++)
            {
                var triangleMappingJob = new TriangleMappingJob
                {
                    origToNewMap = meshNativeDataClass.origToNewMapNative,
                    bakedTriangles = meshNativeDataClass.trianglesNativePerSubM[i],
                    newTriangles = newTrianglesPerSubmesh[i],
                };
            
                meshNativeDataClass.jobHandles[i] = triangleMappingJob.Schedule();
            }
            
            JobHandle.CompleteAll(meshNativeDataClass.jobHandles);
            
            var newSewTriangles = new List<int>();
            for (int i = 0; i < executionCutClass.sewTriangles.Count; i++)
            {
                var sewTriangles = executionCutClass.sewTriangles[i];
                for (int k = 0; k < sewTriangles.Count; k++)
                {
                    if (meshNativeDataClass.origToNewMapNative.TryGetValue(sewTriangles[k], out var newTriangleIndex))
                        newSewTriangles.Add(newTriangleIndex);
                    else
                    {
                        Debug.LogError("Triangle Index not existing!");
                    }
                }    
            }
            
            mesh.SetVertices(newVertices);
            mesh.SetNormals(newNormals);
            mesh.SetTangents(newTangents);
            mesh.SetUVs(0, newUV);
            mesh.boneWeights = newBoneWeights;
            mesh.bindposes = meshDataClass.serializableMesh.bindposes;
            mesh.subMeshCount = bakedMesh.subMeshCount;
            
            for (int i = 0; i < mesh.subMeshCount; i++)
                mesh.SetTriangles(newTrianglesPerSubmesh[i].AsArray().ToList(), i);

            mesh.subMeshCount++;
            mesh.SetTriangles(newSewTriangles, mesh.subMeshCount - 1);
            
            for (int i = 0; i < bakedMesh.subMeshCount; i++) newTrianglesPerSubmesh[i].Dispose();
            
            for (var i = 0; i < cutCenters.Count; i++) cutCenters[i] = smrTransform.TransformPoint(cutCenters[i]);
        }
        
        
        
        
        
        [BurstCompile]
        private struct TriangleMappingJob : IJob
        {
            [ReadOnly] public Mesh.MeshData meshData;
            [ReadOnly] public NativeHashMap<int, int> origToNewMap;
            [ReadOnly] public NativeArray<int> bakedTriangles;
            public NativeList<int> newTriangles;
        
        
            public void Execute()
            {
                for (var i = 0; i < bakedTriangles.Length; i += 3)
                {
                    if (origToNewMap.TryGetValue(bakedTriangles[i], out var newIdx1) &&
                        origToNewMap.TryGetValue(bakedTriangles[i + 1], out var newIdx2) &&
                        origToNewMap.TryGetValue(bakedTriangles[i + 2], out var newIdx3))
                    {
                        newTriangles.Add(newIdx1);
                        newTriangles.Add(newIdx2);
                        newTriangles.Add(newIdx3);
                    }
                }
            }
        }
        
        /********************************************************************************************************************************/
        
        public static List<int> GetInfluencingBones(Mesh mesh, List<int> vertexIndexes)
        {
            HashSet<int> boneIndexes = new HashSet<int>();
            var boneWeights = mesh.boneWeights;

            for (var i = 0; i < vertexIndexes.Count; i++)
            {
                var boneWeight = boneWeights[vertexIndexes[i]];

                if (boneWeight.weight0 > 0)
                    boneIndexes.Add(boneWeight.boneIndex0);

                if (boneWeight.weight1 > 0)
                    boneIndexes.Add(boneWeight.boneIndex1);

                if (boneWeight.weight2 > 0)
                    boneIndexes.Add(boneWeight.boneIndex2);

                if (boneWeight.weight3 > 0)
                    boneIndexes.Add(boneWeight.boneIndex3);
            }

            return boneIndexes.ToList();
        }
    }
}