// ----------------------------------------------------
// Gore Simulator
// Copyright (c) Pampel Games e.K. All Rights Reserved.
// https://www.pampelgames.com
// ----------------------------------------------------

using System;
using PampelGames.Shared.Tools;
using UnityEngine;

namespace PampelGames.GoreSimulator
{
    [Serializable]
    public class MeshDataClass
    {
        public SerializableMesh serializableMesh;
        
        public BoneWeight[] boneWeights;


        public void InitializeEditorMeshData(Mesh mesh)
        {
            serializableMesh = new SerializableMesh();
            serializableMesh.FillDataFromMesh(mesh);
        }

        public void InitializeRuntimeMeshData(Mesh mesh)
        {
            boneWeights = mesh.boneWeights;
        }
    }
}