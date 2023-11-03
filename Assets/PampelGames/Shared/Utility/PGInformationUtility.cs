// ----------------------------------------------------
// Copyright (c) Pampel Games e.K. All Rights Reserved.
// https://www.pampelgames.com
// ----------------------------------------------------

using UnityEngine;
using UnityEngine.Rendering;

namespace PampelGames.Shared.Utility
{
    public static class PGInformationUtility
    {
        /// <summary>
        ///     Checks for the render pipeline that is used in the project.
        /// </summary>
        public static PGEnums.RenderPipelineEnum GetRenderPipeline()
        {
            var currentPipeline = GraphicsSettings.renderPipelineAsset;
            if (currentPipeline == null)
                return PGEnums.RenderPipelineEnum.BuiltIn;
            if (currentPipeline.GetType().Name.Contains("UniversalRenderPipelineAsset"))
                return PGEnums.RenderPipelineEnum.URP;
            if (currentPipeline.GetType().Name.Contains("HighDefinitionRenderPipelineAsset") || 
                currentPipeline.GetType().Name.Contains("HDRenderPipelineAsset"))
                return PGEnums.RenderPipelineEnum.HDRP;
            return PGEnums.RenderPipelineEnum.BuiltIn;
        }
        
        /// <summary>
        ///     Get the year of the Unity version being used. 
        /// </summary>
        public static string GetUnityVersionYear()
        {
            var unityVersion = Application.unityVersion.PGCutAfter(".", true).Trim();
            return unityVersion;
        }


        /// <summary>
        ///     Create a primitive sphere to get visual information about a position.
        /// </summary>
        public static GameObject CreateSphere(Vector3 position, float scaleMultiplier = 0.1f)
        {
            GameObject parent = GameObject.Find("SphereParent");
            if (parent == null) parent = new GameObject("SphereParent");
            var sphere = GameObject.CreatePrimitive(PrimitiveType.Sphere);
            sphere.transform.position = position;
            sphere.transform.localScale *= scaleMultiplier;
            sphere.transform.parent = parent.transform;
            if (sphere.TryGetComponent<Collider>(out var collider))
                collider.enabled = false;
            return sphere;
        }
        public static GameObject CreateSphere(Vector3 position, string name)
        {
            var sphere = CreateSphere(position);
            sphere.name = name;
            return sphere;
        }

        public static GameObject CreateQuad(Vector3 position, Vector3 planeNormal)
        {
            var quad = GameObject.CreatePrimitive(PrimitiveType.Quad);
            quad.transform.rotation = Quaternion.LookRotation(planeNormal);
            quad.transform.position = position;
            return quad;
        }
    }
}
