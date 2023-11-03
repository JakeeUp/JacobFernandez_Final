// ----------------------------------------------------
// Copyright (c) Pampel Games e.K. All Rights Reserved.
// https://www.pampelgames.com
// ----------------------------------------------------

using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace PampelGames.Shared.Tools
{
    
    [Serializable]
    public class SerializableMesh
    {
        public int[] indexes;
        public Vector3[] vertices;
        public Vector3[] normals;
        public Vector4[] tangents;
        public int[] triangles;
        public List<SubMeshTriangles> subMeshTriangles;
        public Vector2[] uv;
        public Matrix4x4[] bindposes;
        public int subMeshCount;

        public void FillDataFromMesh(Mesh mesh)
        {
            vertices = mesh.vertices;
            indexes = Enumerable.Range(0, vertices.Length).ToArray();
            normals = mesh.normals;
            tangents = mesh.tangents;
            uv = mesh.uv;
            triangles = mesh.triangles;
            subMeshCount = mesh.subMeshCount;
            
            subMeshTriangles = new List<SubMeshTriangles>();
            
            for (int i = 0; i < mesh.subMeshCount; i++)
            {
                subMeshTriangles.Add(new SubMeshTriangles());
                int[] _triangles = mesh.GetTriangles(i);
                subMeshTriangles[i].triangles = new int[_triangles.Length];
                Array.Copy(_triangles, subMeshTriangles[i].triangles, _triangles.Length);
            }

            if(mesh.bindposes.Length > 0)
            {
                bindposes = mesh.bindposes;
            }
        }
    }
    
    [Serializable]
    public class SubMeshTriangles
    {
        public int[] triangles;
    }
}
