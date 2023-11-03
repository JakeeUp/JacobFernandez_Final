// ----------------------------------------------------
// Gore Simulator
// Copyright (c) Pampel Games e.K. All Rights Reserved.
// https://www.pampelgames.com
// ----------------------------------------------------

using System;
using System.Collections.Generic;
using PampelGames.Shared.Utility;
using UnityEngine;
using UnityEngine.Profiling;
using UnityEngine.Rendering;
using Object = UnityEngine.Object;

namespace PampelGames.GoreSimulator
{
    public class SubModuleDecal : SubModuleBase
    {
        public override string ModuleName()
        {
            return "Decal";
        }

        public override string ModuleInfo()
        {
            return "Projects custom decals on renderers.";
        }

        public override int imageIndex()
        {
            return 0;
        }

        public override bool CompatibleRagdoll()
        {
            return false;
        }


        public override void ModuleAdded(Type type)
        {
            base.ModuleAdded(type);
#if UNITY_EDITOR
            uvTransformation = _goreSimulator._defaultReferences.uvTransformation;
#endif
        }

        /********************************************************************************************************************************/

        public float radius = 0.25f;
        public float strength = 0.75f;

        public Material uvTransformation;

        private Material centerMaterial;
        private CommandBuffer command;
        private RenderTexture centerMaskRenderTexture;
        
        private readonly Dictionary<GameObject, RenderTexture> renderTextures = new();
        
        private const int TEXTURE_RESOLUTION = 2048;
        private const float HARDNESS = 1f;

        private bool centerMeshApplied;

        /********************************************************************************************************************************/

        public override void Initialize()
        {
            base.Initialize();
            command = new CommandBuffer();
            command.name = "UV Space Renderer";
            Reset();
        }

        public override void Reset()
        {
            base.Reset();
            ReleaseRenderTextures();
            centerMaskRenderTexture = new RenderTexture(TEXTURE_RESOLUTION, TEXTURE_RESOLUTION, 0);
            centerMeshApplied = false;
        }

        /********************************************************************************************************************************/

        public override void ExecuteModuleCut(SubModuleClass subModuleClass)
        {
            var cutCenters = new List<Vector3> {subModuleClass.cutPosition};
            
            // Center
            if(!subModuleClass.multiCut) CreateTriangleIndex(subModuleClass.centerMesh);
            if (!centerMeshApplied && subModuleClass.subModuleObjectClasses.Count > 0)
            {
                ApplyMaskToMesh(_goreSimulator.smr, centerMaskRenderTexture, subModuleClass.centerMesh, cutCenters);
                centerMeshApplied = true;
            }
            
            ExecuteModuleInternal(subModuleClass, cutCenters);
        }

        public override void ExecuteModuleExplosion(SubModuleClass subModuleClass)
        {
            ExecuteModuleInternal(subModuleClass, new List<Vector3>());
        }

        public override void ExecuteModuleRagdoll(List<GoreBone> goreBones)
        {
        }


        /********************************************************************************************************************************/

        private void ExecuteModuleInternal(SubModuleClass subModuleClass, List<Vector3> cutCenters)
        {
            if (ExecutionUtility.AddDecalMaterial(_goreSimulator))
                _goreSimulator.smr.materials[^1].SetTexture(ShaderConstants.MaskTexture, centerMaskRenderTexture);


            bool newEntry = false;
            if (!renderTextures.TryGetValue(subModuleClass.parent, out var renderTexture))
            {
                renderTexture = new RenderTexture(TEXTURE_RESOLUTION, TEXTURE_RESOLUTION, 0);
                renderTextures.Add(subModuleClass.parent, renderTexture);
                newEntry = true;
            }
            
            
            /********************************************************************************************************************************/
            // Adding the new decal material instance, assuming all child renderer using the same materials.
            
            var _rendererReference = subModuleClass.subModuleObjectClasses[^1].renderer;
            var materials = _rendererReference.materials;
            
            if (newEntry)
            {
                var newDecalMaterial = Object.Instantiate(_goreSimulator.decalMaterial);
                newDecalMaterial.SetTexture(ShaderConstants.MaskTexture, renderTexture);
            
                var decalMaterialName = _goreSimulator.decalMaterial.name;
                var decalMaterialAdded = false;
                for (var j = 0; j < materials.Length; j++)
                {
                    if (!materials[j].name.Contains(decalMaterialName)) continue;
                    materials[j] = newDecalMaterial;
                    decalMaterialAdded = true;
                    break;
                }

                if (!decalMaterialAdded)
                {
                    Array.Resize(ref materials, materials.Length + 1);
                    materials[^1] = newDecalMaterial;
                }
            }
            /********************************************************************************************************************************/
            
            bool trianglesAlreadySet = TrianglesAlreadySet(subModuleClass.subModuleObjectClasses[0].mesh);
            for (var i = 0; i < subModuleClass.subModuleObjectClasses.Count; i++)
            {
                var subModuleObjectClass = subModuleClass.subModuleObjectClasses[i];
                var _renderer = subModuleObjectClass.renderer;
                
                _renderer.materials = materials;
                
                if (!subModuleClass.multiCut && !trianglesAlreadySet) CreateTriangleIndex(subModuleObjectClass.mesh);

                var usingCutCenters = cutCenters.Count > 0 ? cutCenters : subModuleObjectClass.cutCenters; 
                if (!subModuleClass.multiCut)
                    ApplyMaskToMesh(_renderer, centerMaskRenderTexture, subModuleObjectClass.mesh, usingCutCenters);
                ApplyMaskToMesh(_renderer, renderTexture, subModuleObjectClass.mesh, usingCutCenters);
            }
        }

        /********************************************************************************************************************************/

        private void CreateTriangleIndex(Mesh mesh)
        {
            var allTriangles = mesh.triangles;
            var originalSubmeshCount = mesh.subMeshCount;
            mesh.subMeshCount += 1;
            mesh.SetTriangles(allTriangles, originalSubmeshCount);
        }

        private bool TrianglesAlreadySet(Mesh mesh)
        {
            if (mesh.subMeshCount < 2) return false;
            
            int[] lastSubMeshIndices = mesh.GetTriangles(mesh.subMeshCount - 1);
            int countOfLastSubMeshTriangles = lastSubMeshIndices.Length;

            int countOfSubMeshTriangles = 0;
            for (int i = 0; i < mesh.subMeshCount - 1; i++)
            {
                int[] subMeshIndices = mesh.GetTriangles(i);
                countOfSubMeshTriangles += subMeshIndices.Length;
            }
            return countOfSubMeshTriangles == countOfLastSubMeshTriangles;
        }


        private void ApplyMaskToMesh(Renderer _renderer, RenderTexture renderTexture, Mesh mesh, List<Vector3> cutCenters)
        {
            uvTransformation.SetFloat(ShaderConstants.hardnessID, HARDNESS);
            uvTransformation.SetFloat(ShaderConstants.strengthID, strength);
            uvTransformation.SetFloat(ShaderConstants.radiusID, radius);
            uvTransformation.SetInt(ShaderConstants.blendOpID, (int) BlendOp.Add);

            command.Clear();
            command.SetRenderTarget(renderTexture);


            foreach (var center in cutCenters)
            {
                uvTransformation.SetVector(ShaderConstants.centerID, center);
                for (var i = 0; i < mesh.subMeshCount; i++)
                    command.DrawRenderer(_renderer, uvTransformation, i);
            }

            Graphics.ExecuteCommandBuffer(command);
        }


        /********************************************************************************************************************************/

        private void ReleaseRenderTextures()
        {
            if (centerMaskRenderTexture != null) centerMaskRenderTexture.Release();
            foreach (KeyValuePair<GameObject, RenderTexture> pair in renderTextures) pair.Value.Release();
            renderTextures.Clear();
        }
        
        public override void Destroyed()
        {
            base.Destroyed();
            ReleaseRenderTextures();
            command.Release();
        }
    }
}