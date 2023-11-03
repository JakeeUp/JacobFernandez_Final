// ----------------------------------------------------
// Copyright (c) Pampel Games e.K. All Rights Reserved.
// https://www.pampelgames.com
// ----------------------------------------------------

#if UNITY_EDITOR
using UnityEditor;
using UnityEditor.UIElements;
#endif
using UnityEngine;
using UnityEngine.UIElements;

namespace PampelGames.Shared.Utility
{
    [AddComponentMenu("Pampel Games/Read Me")]
    public class ReadMe : MonoBehaviour
    {
        public string readMeText;
    }



#if UNITY_EDITOR
    [CustomEditor(typeof(ReadMe))]  
    public class ReadMeInspector : UnityEditor.Editor  
    {  
        private ReadMe _readMe;
	    
        private VisualElement container;  
        private TextField infoText;

        private VisualElement readMeButtonWrapper;
        private Button showReadMeTextButton;

        private TextField readMeText;
        
        protected void OnEnable()  
        {         
            _readMe = target as ReadMe;
	          
            container = new VisualElement();
            infoText = new TextField();
            infoText.multiline = true;
            infoText.SetEnabled(false);
            infoText.style.fontSize = 14f;
            infoText.style.whiteSpace = WhiteSpace.Normal;
            var input = infoText.PGGetTextInput();
            input.style.color = new StyleColor(Color.white);
            showReadMeTextButton = new();
            readMeButtonWrapper = new();
            readMeText = new TextField();
            readMeText.multiline = true;
        }        
        
        public override VisualElement CreateInspectorGUI()
        {
            showReadMeTextButton.text = "+";
            showReadMeTextButton.style.flexShrink = 1f;
            showReadMeTextButton.style.flexGrow = 0f;

            readMeButtonWrapper.style.justifyContent = Justify.FlexEnd;
            readMeButtonWrapper.style.alignItems = Align.FlexEnd;
            showReadMeTextButton.style.width = 10f;

            readMeText.style.display = DisplayStyle.None;
            
            var readMeTextProperty = serializedObject.FindProperty(nameof(ReadMe.readMeText));
            readMeText.BindProperty(readMeTextProperty);
            readMeText.RegisterValueChangedCallback(evt => SetText());
            
            showReadMeTextButton.clicked += () =>
            {
                readMeText.style.display = DisplayStyle.Flex;
            };
            
            readMeButtonWrapper.Add(showReadMeTextButton);
            container.Add(infoText);  
            container.Add(readMeButtonWrapper);
            container.Add(readMeText);
            
            return container;  
        }

        private void SetText()
        {
            infoText.value = _readMe.readMeText;
        }
    }  
#endif
}


