// ----------------------------------------------------
// Gore Simulator
// Copyright (c) Pampel Games e.K. All Rights Reserved.
// https://www.pampelgames.com
// ----------------------------------------------------

using PampelGames.Shared;
using PampelGames.Shared.Editor;
using UnityEditor;
using UnityEditor.UIElements;
using UnityEngine;
using UnityEngine.UIElements;

namespace PampelGames.GoreSimulator.Editor
{
    public class GlobalSettings : EditorWindow
    {
        [HideInInspector] public SO_GlobalSettings globalSettingsSo;
        private SerializedObject serializedObject;
        
        private VisualElement poolingWrapper;
        private VisualElement profilingWrapper;
        
        private Button resetButton;

        
        // Editor

        
        // Pool
        private SerializedProperty hidePooledObjectsProperty;
        private Toggle hidePooledObjects;
        private SerializedProperty cutPreloadProperty;
        private IntegerField cutPreload;
        private SerializedProperty cutLimitedProperty;
        private Toggle cutLimited;
        
        private SerializedProperty particlePreloadProperty;
        private IntegerField particlePreload;
        private SerializedProperty particleLimitedProperty;
        private Toggle particleLimited;
        
        private SerializedProperty createGUIProfilerButtonProperty;
        private Button createGUIProfilerButton;

        /********************************************************************************************************************************/
        private void OnEnable()
        {
            if (globalSettingsSo == null)
                globalSettingsSo = PGAssetUtility.LoadAsset<SO_GlobalSettings>(Constants.GlobalSettings);
            
            serializedObject ??= new SerializedObject(globalSettingsSo);

            CreateEditorWindow();
            CreateEditorWrapper();
            CreateRuntimeWrapper();
            CreateResetButton();

            BindElements();
        }

        private void CreateEditorWindow()
        {
            string[] elementNames = new string[2];
            elementNames[0] = "Pool";
            elementNames[1] = "Profiling";

            PGEditorWindowSetup.CreateEditorWindow("Gore Simulator - Global Settings", elementNames, out var _parentElement, out var _elementsArray);
            
            poolingWrapper = _elementsArray[0];
            profilingWrapper = _elementsArray[1];

            rootVisualElement.Add(_parentElement);
        }

        private void CreateEditorWrapper()
        {

        }

        private void CreateRuntimeWrapper()
        {

            hidePooledObjects = new Toggle();
            hidePooledObjects.label = "Hide Pooled Objects";
            hidePooledObjects.tooltip = "Hide pooled objects in the scene hierarchy. This option won't have any impact on the built application.";
            
            cutPreload = new IntegerField("Mesh Preload");
            cutPreload.tooltip = "GameObjects used for cut and explosion operations preloaded into the pool in Awake. " +
                                       "Shared among all Gore Simulator components.";
            cutPreload.PGClampValue();
            
            particlePreload = new IntegerField("Particle Preload");
            particlePreload.tooltip = "Particles preloaded into the pool in Awake. " +
                                      "Shared among all Gore Simulator components, where one pool is created for each different system.";
            particlePreload.PGClampValue();

            string limitedTooltip = "If true, the preload amount also defines the max count of pooled GameObjects in the scene.\n" +
                                    "Upon exceeding, oldest active objects are reutilized.";
            cutLimited = new Toggle("Limit Cut");
            cutLimited.tooltip = limitedTooltip;
            
            particleLimited = new Toggle("Limit Particles");
            particleLimited.tooltip = limitedTooltip;
            

            createGUIProfilerButton = new Button();
            createGUIProfilerButton.text = "Create GUI Profiler";
            createGUIProfilerButton.tooltip = "Create a GameObject that shows runtime infos on the screen about active Gore Simulators.";
            
            poolingWrapper.Add(hidePooledObjects);
            poolingWrapper.Add(cutPreload);
            poolingWrapper.Add(cutLimited);
            poolingWrapper.Add(particlePreload);
            poolingWrapper.Add(particleLimited);
            
            profilingWrapper.Add(createGUIProfilerButton);
        }
        
        private void CreateResetButton()
        {
            resetButton = PGEditorWindowSetup.CreateResetButton();
            resetButton.clicked += ResetValuesClicked;
            
            rootVisualElement.Add(resetButton);
        }
        
        private void ResetValuesClicked()
        {
            if (EditorUtility.DisplayDialog("Reset Settings", "Reset all global settings to their default values?", "Ok", "Cancel"))
            {
                globalSettingsSo.ResetValues();
                EditorUtility.SetDirty(globalSettingsSo);
            }
        }
        private void BindElements()
        {
            hidePooledObjectsProperty = serializedObject.FindProperty(nameof(SO_GlobalSettings.hidePooledObjects));
            hidePooledObjects.BindProperty(hidePooledObjectsProperty);
            cutPreloadProperty = serializedObject.FindProperty(nameof(SO_GlobalSettings.cutPreload));
            cutPreload.BindProperty(cutPreloadProperty);
            cutLimitedProperty = serializedObject.FindProperty(nameof(SO_GlobalSettings.cutLimited));
            cutLimited.BindProperty(cutLimitedProperty);
            particlePreloadProperty = serializedObject.FindProperty(nameof(SO_GlobalSettings.particlePreload));
            particlePreload.BindProperty(particlePreloadProperty);
            particleLimitedProperty = serializedObject.FindProperty(nameof(SO_GlobalSettings.particleLimited));
            particleLimited.BindProperty(particleLimitedProperty);
            
        }
        
        
        /********************************************************************************************************************************/

        public void CreateGUI()
        {
            
            createGUIProfilerButton.clicked += () =>
            {
                var existingGUIProfiler = (GUIProfiler) FindObjectOfType(typeof(GUIProfiler));
                if (existingGUIProfiler != null)
                {
                    if (EditorUtility.DisplayDialog("GUI Profiler", "There is allready a GUI Profiler in the scene.", "Remove", "Cancel"))
                    {
                        DestroyImmediate(existingGUIProfiler.gameObject);
                    }
                    return;
                }

                var newGUIProfiler = new GameObject("Gore Simulator Profiler");
                newGUIProfiler.AddComponent<GUIProfiler>();
                newGUIProfiler.transform.position = Vector3.zero;
            };

        }
    }
}