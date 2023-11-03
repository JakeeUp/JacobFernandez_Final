// ----------------------------------------------------
// Gore Simulator
// Copyright (c) Pampel Games e.K. All Rights Reserved.
// https://www.pampelgames.com
// ----------------------------------------------------

using UnityEditor;
using UnityEditor.UIElements;
using UnityEngine.UIElements;

namespace PampelGames.GoreSimulator.Editor
{
    [CustomEditor(typeof(GUIProfiler))]  
    public class GUIProfilerInspector : UnityEditor.Editor  
    {  
        private GUIProfiler _guiProfiler;
	    
        private VisualElement container;  
        private HelpBox infoBox;

        private SerializedProperty colorProperty; 
        private ColorField color;
        private SerializedProperty offsetProperty; 
        private Vector2Field offset;
        private SerializedProperty dontDestroyOnLoadProperty; 
        private Toggle dontDestroyOnLoad;
        
        protected void OnEnable()  
        {         
            _guiProfiler = target as GUIProfiler;
	          
            container = new VisualElement();  
            infoBox = new HelpBox();
            color = new ColorField("Text Color");
            offset = new Vector2Field("Offset");
            dontDestroyOnLoad = new Toggle("DontDestroyOnLoad");
            
            BindElements();
        }

        private void BindElements()
        {
            colorProperty = serializedObject.FindProperty(nameof(GUIProfiler.color));
            color.BindProperty(colorProperty);
            offsetProperty = serializedObject.FindProperty(nameof(GUIProfiler.offset));
            offset.BindProperty(offsetProperty);
            dontDestroyOnLoadProperty = serializedObject.FindProperty(nameof(GUIProfiler.dontDestroyOnLoad));
            dontDestroyOnLoad.BindProperty(dontDestroyOnLoadProperty);
        }
        
        /********************************************************************************************************************************/
        
        public override VisualElement CreateInspectorGUI()  
        {  
            
            infoBox.messageType = HelpBoxMessageType.Info;
            infoBox.text = "This GameObject was created in Gore Simulator - Global Settings.\n" +
                           "It draws runtime information about active Gore Simulators onto the screen.\n" +
                           "Can be removed if not required anymore.";
            
            dontDestroyOnLoad.tooltip = "Do not destroy the GameObject when loading a new Scene.";
            
            container.Add(infoBox); 
            container.Add(color);
            container.Add(offset);
            container.Add(dontDestroyOnLoad);
            return container;  
        }


    }
}