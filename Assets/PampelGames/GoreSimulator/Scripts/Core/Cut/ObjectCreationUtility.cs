// ----------------------------------------------------
// Gore Simulator
// Copyright (c) Pampel Games e.K. All Rights Reserved.
// https://www.pampelgames.com
// ----------------------------------------------------

using System.Collections.Generic;
using System.Linq;
using PampelGames.Shared.Tools;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace PampelGames.GoreSimulator
{
    
    public static class ObjectCreationUtility
    {
        
        public static Dictionary<int, List<int>> BuildAdjacencyDict(Mesh mesh)
        {
            var adjacencyList = new Dictionary<int, List<int>>();
            var triangles = mesh.triangles;

            for (var i = 0; i < triangles.Length; i += 3)
            {
                // Add adjacency for first vertex of the triangle
                if (!adjacencyList.ContainsKey(triangles[i]))
                    adjacencyList[triangles[i]] = new List<int>();
                adjacencyList[triangles[i]].Add(triangles[i + 1]);
                adjacencyList[triangles[i]].Add(triangles[i + 2]);

                // Add adjacency for second vertex of the triangle
                if (!adjacencyList.ContainsKey(triangles[i + 1]))
                    adjacencyList[triangles[i + 1]] = new List<int>();
                adjacencyList[triangles[i + 1]].Add(triangles[i]);
                adjacencyList[triangles[i + 1]].Add(triangles[i + 2]);

                // Add adjacency for third vertex of the triangle
                if (!adjacencyList.ContainsKey(triangles[i + 2]))
                    adjacencyList[triangles[i + 2]] = new List<int>();
                adjacencyList[triangles[i + 2]].Add(triangles[i]);
                adjacencyList[triangles[i + 2]].Add(triangles[i + 1]);
            }

            return adjacencyList;
        }
        


        public static GameObject CreateMeshObject(GoreSimulator goreSimulator, Mesh mesh, string name)
        {
            var newObject = Pool.Get(goreSimulator._defaultReferences.pooledMesh, goreSimulator.GetInstanceID());
            newObject.name = name;
            
            if(newObject.TryGetComponent<MeshFilter>(out var meshFilter))
                meshFilter.mesh = mesh;
            
            newObject.transform.SetParent(null);
            SceneManager.MoveGameObjectToScene(newObject, goreSimulator.gameObject.scene);
            
            goreSimulator.AddDetachedObject(newObject);

            return newObject;
        }
        
        
        
        

#if UNITY_EDITOR
        public static GameObject CreateMeshObjectEditor(GoreSimulator goreSimulator, Mesh mesh, string name)
        {
            var transform = goreSimulator.smr.transform;
            var newObject = new GameObject(name)
            {
                transform =
                {
                    position = transform.position,
                    rotation = transform.rotation,
                    localScale = transform.localScale
                }
            };

            var meshFilter = newObject.AddComponent<MeshFilter>();
            meshFilter.mesh = mesh;

            var renderer = newObject.AddComponent<MeshRenderer>();
            
            Material[] currentMaterials = goreSimulator.smr.sharedMaterials;
            Material[] newMaterials = new Material[currentMaterials.Length + 1];
            currentMaterials.CopyTo(newMaterials, 0);

            newMaterials[^1] = goreSimulator.cutMaterial;
            renderer.materials = newMaterials;
            
            return newObject;
        }
#endif
    }
}