// ----------------------------------------------------
// Gore Simulator
// Copyright (c) Pampel Games e.K. All Rights Reserved.
// https://www.pampelgames.com
// ----------------------------------------------------

using System.Collections.Generic;
using PampelGames.Shared.Editor;
using PampelGames.Shared.Utility;
using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;

namespace PampelGames.GoreSimulator.Editor
{
    public class CombineSkinnedMeshes : EditorWindow
    {
        private VisualElement wrapper;


        private readonly List<SkinnedMeshRenderer> skinnedMeshRenderers = new();
        private ListView skinnedMeshRenderersList;

        private Button addChildrenButton;
        private Button combineMeshesButton;

        /********************************************************************************************************************************/
        private void OnEnable()
        {
            CreateEditorWindow();
            CreateListView();

            CreateRuntimeWrapper();

            BindElements();
        }

        private void CreateEditorWindow()
        {
            var elementNames = new string[1];
            elementNames[0] = "Skinned Mesh Renderers";

            PGEditorWindowSetup.CreateEditorWindow("Combine Skinned Meshes", elementNames, out var _parentElement, out var _elementsArray);

            wrapper = _elementsArray[0];

            rootVisualElement.Add(_parentElement);
        }

        private void CreateListView()
        {
            skinnedMeshRenderersList = new ListView();
            skinnedMeshRenderersList.tooltip = "Assign the Skinned Mesh Renderers for combination.\n" +
                                               "Ensure they are in the scene hierarchy and nested under a single character parent.";
            skinnedMeshRenderersList.PGSetupObjectListViewEditor(skinnedMeshRenderers);
            skinnedMeshRenderersList.PGObjectListViewStyle();
        }

        private void CreateRuntimeWrapper()
        {
            combineMeshesButton = new Button();
            combineMeshesButton.text = "Combine Meshes";

            addChildrenButton = new Button();
            addChildrenButton.text = "Add Children";
            addChildrenButton.tooltip = "Select the character to add all Skinned Mesh Renderers from its child objects to the list.";

            wrapper.Add(addChildrenButton);
            wrapper.Add(skinnedMeshRenderersList);
            wrapper.Add(combineMeshesButton);
        }


        private void BindElements()
        {
        }


        /********************************************************************************************************************************/

        public void CreateGUI()
        {
            combineMeshesButton.clicked += () =>
            {
                if (!VerifySkinnedMeshRenderers()) return;
                MergeSkinnedMeshes();
            };
            
            addChildrenButton.clicked += () =>
            {
                var childrenSMR = Selection.activeGameObject.GetComponentsInChildren<SkinnedMeshRenderer>();
                foreach (var childSMR in childrenSMR)
                {
                    if(!skinnedMeshRenderers.Contains(childSMR)) skinnedMeshRenderers.Add(childSMR);
                }
                skinnedMeshRenderersList.Rebuild();
            };
        }

        private bool VerifySkinnedMeshRenderers()
        {
            if (skinnedMeshRenderers.Count == 0) return false;

            for (var i = 0; i < skinnedMeshRenderers.Count; i++)
            {
                var renderer = skinnedMeshRenderers[i];
                if (renderer != null) continue;
                Debug.LogError("Item " + i + " is null.");
                return false;
            }

            foreach (var renderer in skinnedMeshRenderers)
            {
                if (renderer.gameObject.scene.rootCount != 0) continue;
                Debug.LogError("Skinned Mesh Renderer: " + renderer.gameObject.name + " is not in the scene hierarchy.");
                return false;
            }

            var parentTransform = skinnedMeshRenderers[0].transform.parent;
            foreach (var renderer in skinnedMeshRenderers)
            {
                if (renderer.transform.parent == parentTransform) continue;
                Debug.LogError("All Skinned Mesh Renderers need to have the same parent.");
                return false;
            }

            return true;
        }

        private void MergeSkinnedMeshes()
        {
            var newObject = new GameObject("SM_Combined");
            if (!PGSkinnedMeshUtility.CombineSkinnedMeshes(newObject, skinnedMeshRenderers, true))
            {
                DestroyImmediate(newObject);
                return;
            }

            newObject.transform.SetParent(skinnedMeshRenderers[0].transform.parent);

            foreach (var renderer in skinnedMeshRenderers) renderer.gameObject.SetActive(false);

            Debug.Log($"Successfully combined {skinnedMeshRenderers.Count} Skinned Mesh Renderers.");

            Selection.activeGameObject = newObject;
        }
    }
}