// ----------------------------------------------------
// Copyright (c) Pampel Games e.K. All Rights Reserved.
// https://www.pampelgames.com
// ----------------------------------------------------

using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace PampelGames.Shared.Utility
{
    public static class PGRenderTextureUtility
    {
        /// <summary>
        ///     Creates a new 2D texture with automatic camera creation, capturing at position 0,0,0. Uses current scene lighting.
        /// </summary>
        /// <param name="captureCameraObj">Gameobject with a Camera component. Make sure Clear Flags (HDRP: Brackground Type) is set to Solid Color with (0,0,0,0).</param>
        /// <param name="resolution">Resolution of the texture in pixels.</param>
        /// <param name="bounds">Mesh bounds.</param>
        /// <param name="direction">Direction the camera looks to.</param>
        public static Texture2D CaptureToTextureAutomatic(GameObject captureCameraObj, int resolution, Bounds bounds, PGEnums.AxisEnum direction)
        {
            return CaptureToTextureAutomaticInternal(captureCameraObj, resolution, bounds, direction);
        }
        
        /// <summary>
        ///     Create new 2D texture from a scene camera.
        /// </summary>
        /// <param name="captureCamera">Scene camera. Make sure Clear Flags (HDRP: Brackground Type) is set to Solid Color with (0,0,0,0).</param>
        /// <param name="resolution">Resolution of the texture in pixels.</param>
        public static Texture2D CaptureToTexture(Camera captureCamera, int resolution)
        {
            return CaptureToTextureInternal(captureCamera, resolution);
        }
        

        /********************************************************************************************************************************/
        /********************************************************************************************************************************/
        
        private static Texture2D CaptureToTextureAutomaticInternal(GameObject captureCameraObj, int resolution, Bounds bounds, PGEnums.AxisEnum direction)
        {
            GameObject[] allObjects = SceneManager.GetActiveScene().GetRootGameObjects();

            List<GameObject> disabledObjects = new List<GameObject>();
            foreach (var obj in allObjects)
            {
                Traverse(obj.transform, disabledObjects);
            }

            GameObject cameraObj = Object.Instantiate(captureCameraObj);
            cameraObj.name = "FTC Render Texture Camera";
            Camera camera = cameraObj.GetComponent<Camera>();
            var axisDirection = PGEnums.GetAxis(direction);

            Vector3 axisSwitched = new Vector3(1 - axisDirection.x, 1 - axisDirection.y, 1 - axisDirection.z);
            camera.transform.position = new Vector3(bounds.center.x * axisSwitched.x, bounds.center.y * axisSwitched.y, 
                bounds.center.z * axisSwitched.z);
    
            float maxLength = Mathf.Max(bounds.size.x, bounds.size.y, bounds.size.z);
            var translate = axisDirection * -(maxLength);
    
            camera.transform.Translate(translate, Space.World);
            camera.transform.LookAt(bounds.center);

            var texture = CaptureToTextureInternal(camera, resolution);
            if(Application.isPlaying) Object.Destroy(cameraObj);
            else Object.DestroyImmediate(cameraObj);

            foreach (var disabledObject in disabledObjects)
                disabledObject.SetActive(true);
    
            return texture;
        }

        private static void Traverse(Transform curr, List<GameObject> disabledObjects)
        {
            if(curr.gameObject.hideFlags == HideFlags.HideInHierarchy || !curr.gameObject.activeInHierarchy) return;
            if (curr.gameObject.TryGetComponent(out MeshRenderer meshRenderer) || curr.gameObject.TryGetComponent(out SkinnedMeshRenderer skinnedMeshRenderer))
            {
                GameObject gameObject;
                (gameObject = curr.gameObject).SetActive(false);
                disabledObjects.Add(gameObject);
            }

            foreach (Transform child in curr)
            {
                Traverse(child, disabledObjects);
            }
        }

        // private static Texture2D CaptureToTextureAutomaticInternal(GameObject captureCameraObj, int resolution, Bounds bounds, PGEnums.AxisEnum direction)
        // {
        //     GameObject[] allObjects = SceneManager.GetActiveScene().GetRootGameObjects();
        //
        //     List<GameObject> disabledObjects = new List<GameObject>();
        //     foreach (var obj in allObjects)
        //     {
        //         if(obj.hideFlags == HideFlags.HideInHierarchy) continue;
        //         if(!obj.activeInHierarchy) continue;
        //         if (obj.TryGetComponent(out MeshRenderer meshRenderer)|| obj.TryGetComponent(out SkinnedMeshRenderer skinnedMeshRenderer))
        //         {
        //             obj.SetActive(false);
        //             disabledObjects.Add(obj);
        //         }
        //     }
        //
        //     GameObject cameraObj = Object.Instantiate(captureCameraObj);
        //     cameraObj.name = "FTC Render Texture Camera";
        //     Camera camera = cameraObj.GetComponent<Camera>();
        //     var axisDirection = PGEnums.GetAxis(direction);
        //
        //     Vector3 axisSwitched = new Vector3(1 - axisDirection.x, 1 - axisDirection.y, 1 - axisDirection.z);
        //     camera.transform.position = new Vector3(bounds.center.x * axisSwitched.x, bounds.center.y * axisSwitched.y, 
        //         bounds.center.z * axisSwitched.z);
        //     
        //     float maxLength = Mathf.Max(bounds.size.x, bounds.size.y, bounds.size.z);
        //     var translate = axisDirection * -(maxLength);
        //     
        //     camera.transform.Translate(translate, Space.World);
        //     camera.transform.LookAt(bounds.center);
        //
        //     var texture = CaptureToTextureInternal(camera, resolution);
        //     if(Application.isPlaying) Object.Destroy(cameraObj);
        //     else Object.DestroyImmediate(cameraObj);
        //
        //     foreach (var disabledObject in disabledObjects)disabledObject.SetActive(true);
        //     
        //     return texture;
        // }
        
        private static Texture2D CaptureToTextureInternal(Camera captureCamera, int resolution)
        {
            RenderTexture targetTexture = new RenderTexture(resolution, resolution, 24);
            Texture2D captureTexture = new Texture2D(targetTexture.width, targetTexture.height,
                TextureFormat.RGBA32, false);
            captureCamera.targetTexture = targetTexture;
            captureCamera.Render();
            RenderTexture.active = targetTexture;
            captureTexture.ReadPixels(new Rect(0, 0, targetTexture.width, targetTexture.height), 0, 0);
            captureTexture.Apply();
            captureCamera.targetTexture = null;
            RenderTexture.active = null;
            return captureTexture;
        }
    }
}