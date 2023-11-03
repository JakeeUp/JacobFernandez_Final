// ----------------------------------------------------
// Copyright (c) Pampel Games e.K. All Rights Reserved.
// https://www.pampelgames.com
// ----------------------------------------------------

using PampelGames.Shared.Utility;
using UnityEngine;
using UnityEngine.UIElements;

namespace PampelGames.Shared.Editor
{
    
    public static class PGEditorWindowSetup
    {
        
        /// <summary>
        ///     Creates the default PG Editor Window style. Names all items accordingly.
        /// </summary>
        /// <param name="headerName">Name of the header.</param>
        /// <param name="elementNames">Names of elements to be created.</param>
        /// <param name="_parentElement">Main element everything is parented to.</param>
        /// <param name="_elementsArray">Each one consists of a nameWrapper and a nameLabel child.</param>
        public static void CreateEditorWindow(string headerName, string[] elementNames, out VisualElement _parentElement, out VisualElement[] _elementsArray)
        {
            _parentElement = new VisualElement();
            _parentElement.name = "parentElement";
            _parentElement.PGMargin(4);

            var headerWrapper = new VisualElement();
            headerWrapper.name = headerName + "Wrapper";
            headerWrapper.PGHeaderWrapper();
            var headerLabel = new Label();
            headerLabel.name = headerName + "Label";
            headerLabel.text = headerName;
            headerLabel.style.unityFontStyleAndWeight = new StyleEnum<FontStyle>(FontStyle.Bold);

            _elementsArray = new VisualElement[elementNames.Length];
            for (int i = 0; i < _elementsArray.Length; i++)
            {
                VisualElement wrapper = new VisualElement();
                wrapper.name = elementNames[i] + "Wrapper";
                Label label = new Label();
                label.name = elementNames[i] + "Label";
                label.text = elementNames[i];
                label.PGHeaderSmall();
                wrapper.Add(label);
                if (i == 0) wrapper.PGDrawTopLine();
                wrapper.PGDrawBottomLine();
                wrapper.PGPadding(0,0,0,6);
                _elementsArray[i] = wrapper;
            }

            headerWrapper.Add(headerLabel);
            _parentElement.Add(headerWrapper);
            for (int i = 0; i < _elementsArray.Length; i++) _parentElement.Add(_elementsArray[i]);
        }
     
        
        public static Button CreateResetButton()
        {
            var resetButton = new Button();
            resetButton.name = "resetButton";
            resetButton.PGMargin(0,0,8,0);
            resetButton.text = "Reset";
            resetButton.tooltip = "Reset settings to their default values.";
            return resetButton;
        }
    }
}