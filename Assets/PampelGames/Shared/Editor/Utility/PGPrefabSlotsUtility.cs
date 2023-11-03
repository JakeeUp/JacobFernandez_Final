// ----------------------------------------------------
// Copyright (c) Pampel Games e.K. All Rights Reserved.
// https://www.pampelgames.com
// ----------------------------------------------------

using PampelGames.Shared.Utility;
using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;

namespace PampelGames.Shared.Editor
{
    
    /// <summary>
    ///     Helper class for drawing prefabs.
    ///     Methods may require the PrefabSlotsUSS style sheet in the UXML file.
    /// </summary>
    public static class PGPrefabSlotsUtility
    {

        /// <summary>
        ///     Creates a dragDropSlot with a dragDropLabel as child.
        /// </summary>
        public static VisualElement CreateDragDropSlot()
        {
            var dragDropSlot = new VisualElement();
            dragDropSlot.name = "dragDropSlot";
            dragDropSlot.tooltip = "Drag & drop prefabs here.";
            
            dragDropSlot.style.opacity = 70;
            dragDropSlot.style.flexDirection = FlexDirection.Row;
            dragDropSlot.style.flexShrink = 0;
            dragDropSlot.style.alignItems = Align.Center;
            dragDropSlot.style.justifyContent = Justify.Center;
            dragDropSlot.style.height = 66;
            dragDropSlot.style.flexGrow = 1f;
            dragDropSlot.PGMargin(1);
            dragDropSlot.style.backgroundColor = PGColors.InputFieldBackground();
            dragDropSlot.PGBorderColor(PGColors.InspectorBorder());
            dragDropSlot.PGBorderRadius(24);
            dragDropSlot.PGBorderWidth(1);
            dragDropSlot.AddToClassList("hoverSlot");


            var dragDropLabel = new Label();
            dragDropLabel.name = "dragDropLabel";
            dragDropLabel.text = "Drop \n" + "here";
            
            dragDropLabel.style.fontSize = 14;
            dragDropLabel.style.unityFontStyleAndWeight = new StyleEnum<FontStyle>(FontStyle.Bold);
            dragDropLabel.style.unityTextAlign = TextAnchor.MiddleCenter;
            dragDropLabel.style.color = PGColors.ReadOnlyText();
            
            dragDropSlot.Add(dragDropLabel);

            return dragDropSlot;
        }

        
        /// <summary>
        ///     Creates a slot for a prefab.
        /// </summary>
        public static VisualElement CreatePrefabSlot()
        {
            var newSlot = new VisualElement();
            newSlot.AddToClassList("slot");
            newSlot.AddToClassList("hoverSlot");
            newSlot.focusable = true;
            newSlot.style.justifyContent = Justify.SpaceBetween;
            newSlot.style.alignItems = Align.Stretch;
            newSlot.style.flexDirection = FlexDirection.Column;
            return newSlot;
        }
        
        
        /// <summary>
        ///     Assigns a preview Texture2D to a VisualElement.
        /// </summary>
        public static void CreateObjectPreview(VisualElement visualElement, Object obj)
        {
            if (visualElement == null) return;
            if (obj == null) return;
            
            var texture = PGAssetUtility.GetPrefabPreview(obj);

            if ((obj as GameObject)?.GetComponent<ParticleSystem>())
                texture = EditorGUIUtility.ObjectContent(null, typeof(ParticleSystem)).image as Texture2D;

            visualElement.style.backgroundImage = texture != null ? texture : visualElement.style.backgroundImage;
        }
        
        
    }
    
}