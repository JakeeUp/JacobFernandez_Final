// ----------------------------------------------------
// Copyright (c) Pampel Games e.K. All Rights Reserved.
// https://www.pampelgames.com
// ----------------------------------------------------

#if UNITY_EDITOR
using UnityEditor;
using UnityEngine.UIElements;

namespace PampelGames.Shared.Tools.Editor
{
    
    [CustomEditor(typeof(PGPoolable))]
    public class PGPoolableInspector : UnityEditor.Editor
    {
        
        protected VisualElement container;
        
        protected virtual void OnEnable()
        {
            container = new VisualElement();
        }
        
        public override VisualElement CreateInspectorGUI()
        {
            var existingHelpBox = container.Q<HelpBox>("helpBox");
            if (existingHelpBox == null)
            {
                var  helpBox = new HelpBox("To despawn this object into the pool, either call PGPool.Release(obj) or Release() on this component.", 
                    HelpBoxMessageType.Info);
                helpBox.name = "helpBox";
                container.Add(helpBox);    
            }
            return container;
        }
        
    }
    
}
#endif