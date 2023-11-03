// ---------------------------------------------------
// Copyright (c) Pampel Games e.K. All Rights Reserved.
// https://www.pampelgames.com
// ---------------------------------------------------

using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

namespace PampelGames.Shared.Utility
{
    public static class PGVisualElementExtensions
    {

        public static void PGDisplayStyleFlex(this VisualElement visualElement, bool flex)
        {
            visualElement.style.display = flex ? DisplayStyle.Flex : DisplayStyle.None;
        }
        public static void PGBoldText(this VisualElement visualElement)
        {
            visualElement.style.unityFontStyleAndWeight = new StyleEnum<FontStyle>(FontStyle.Bold);
        }
        
        public static void PGNormalText(this VisualElement visualElement)
        {
            visualElement.style.unityFontStyleAndWeight = new StyleEnum<FontStyle>(FontStyle.Normal);
        }
        
        /// <summary>
        ///     Sets the default PG header style for a Visual Element. After that, add a Label to it.
        /// </summary>
        public static void PGHeaderWrapper(this VisualElement visualElement)
        {
            visualElement.style.justifyContent = Justify.Center;
            visualElement.style.flexDirection = FlexDirection.Row;
            visualElement.PGPadding(0,0,5,5);
            visualElement.PGMargin(0,0,5,5);
        }
        
        /// <summary>
        ///     Get the text input field of a Visual Element.
        /// </summary>
        public static VisualElement PGGetTextInput(this VisualElement visualElement)
        {
            var textInput = visualElement.Q<VisualElement>(PGConstantsUSS.TextInputField);
            return textInput;
        }

        /// <summary>
        ///     Narrows the label width of this element.
        /// </summary>
        public static void PGRemoveLabelSpace(this VisualElement visualElement)
        {
            var textLabel = visualElement.Q<Label>();
            textLabel.style.minWidth = 0f;
        }

        /// <summary>
        ///     Sets a margin all around.
        /// </summary>
        public static void PGMargin(this VisualElement visualElement, float margin)
        {
            visualElement.style.marginBottom = margin;
            visualElement.style.marginTop = margin;
            visualElement.style.marginLeft = margin;
            visualElement.style.marginRight = margin;
        }
        
        /// <summary>
        ///     Sets a margin specified.
        /// </summary>
        public static void PGMargin(this VisualElement visualElement, float marginLeft, float marginRight, float marginTop, float marginBottom)
        {
            visualElement.style.marginBottom = marginBottom;
            visualElement.style.marginTop = marginTop;
            visualElement.style.marginLeft = marginLeft;
            visualElement.style.marginRight = marginRight;
        }

        /// <summary>
        ///     Sets padding all around.
        /// </summary>
        public static void PGPadding(this VisualElement visualElement, float padding)
        {
            visualElement.style.paddingBottom = padding;
            visualElement.style.paddingTop = padding;
            visualElement.style.paddingLeft = padding;
            visualElement.style.paddingRight = padding;
        }

        /// <summary>
        ///     Sets padding specified.
        /// </summary>
        public static void PGPadding(this VisualElement visualElement, float paddingLeft, float paddingRight, float paddingTop, float paddingBottom)
        {
            visualElement.style.paddingBottom = paddingBottom;
            visualElement.style.paddingTop = paddingTop;
            visualElement.style.paddingLeft = paddingLeft;
            visualElement.style.paddingRight = paddingRight;
        }
        
        /// <summary>
        ///     Sets the border color.
        /// </summary>
        public static void PGBorderColor(this VisualElement visualElement, Color borderColor)
        {
            visualElement.style.borderBottomColor = new StyleColor(borderColor);
            visualElement.style.borderLeftColor = new StyleColor(borderColor);
            visualElement.style.borderRightColor = new StyleColor(borderColor);
            visualElement.style.borderTopColor = new StyleColor(borderColor);
        }

        /// <summary>
        ///     Sets the border width all around.
        /// </summary>
        public static void PGBorderWidth(this VisualElement visualElement, float width)
        {
            visualElement.style.borderBottomWidth = width;
            visualElement.style.borderLeftWidth = width;
            visualElement.style.borderRightWidth = width;
            visualElement.style.borderTopWidth = width;
        }
        
        /// <summary>
        ///     Sets the border width specified.
        /// </summary>
        public static void PGBorderWidth(this VisualElement visualElement, float widthLeft, float widthRight, float widthTop, float widthBottom)
        {
            visualElement.style.borderLeftWidth = widthLeft;
            visualElement.style.borderRightWidth = widthRight;
            visualElement.style.borderTopWidth = widthTop;
            visualElement.style.borderBottomWidth = widthBottom;
        }

        /// <summary>
        ///     Sets the border corner radius all around.
        /// </summary>
        public static void PGBorderRadius(this VisualElement visualElement, float radius)
        {
            visualElement.style.borderBottomLeftRadius = radius;
            visualElement.style.borderBottomRightRadius = radius;
            visualElement.style.borderTopLeftRadius = radius;
            visualElement.style.borderTopRightRadius = radius;
        }

        /// <summary>
        ///     Sets the border corner radius specified.
        /// </summary>
        public static void PGBorderRadius(this VisualElement visualElement, float radiusTopLeft, float radiusBottomLeft, float radiusTopRight, float radiusBottomRight)
        {
            visualElement.style.borderBottomLeftRadius = radiusBottomLeft;
            visualElement.style.borderBottomRightRadius = radiusBottomRight;
            visualElement.style.borderTopLeftRadius = radiusTopLeft;
            visualElement.style.borderTopRightRadius = radiusTopRight;
        }
        
        /// <summary>
        ///     Sets the Background color when hovering over with the mouse.
        /// </summary>
        public static void PGBackgroundColorHover(this VisualElement element, Color hoverColor, StyleColor originalColor = default)
        {
            if (originalColor == default) originalColor = element.style.backgroundColor;
            element.RegisterCallback<MouseEnterEvent>(evt => element.style.backgroundColor = hoverColor);
            element.RegisterCallback<MouseLeaveEvent>(evt => element.style.backgroundColor = originalColor);
        }
        
        /// <summary>
        ///     Sets the image color when hovering over with the mouse.
        /// </summary>
        public static void PGImageColorHover(this VisualElement element, Color hoverColor, StyleColor originalColor = default)
        {
            if (originalColor == default) originalColor = element.style.backgroundColor;
            element.RegisterCallback<MouseEnterEvent>(evt => element.style.unityBackgroundImageTintColor = hoverColor);
            element.RegisterCallback<MouseLeaveEvent>(evt => element.style.unityBackgroundImageTintColor = originalColor);
        }
        
        /// <summary>
        ///     Sets the opacity when hovering over with the mouse.
        /// </summary>
        public static void PGOpacityHover(this VisualElement element, float value = 0.75f)
        {
            element.RegisterCallback<MouseEnterEvent>(evt => element.style.opacity = value);
            element.RegisterCallback<MouseLeaveEvent>(evt => element.style.opacity = 1f);
        }

        /// <summary>
        ///     Sets the Background color when clicking with the mouse.
        /// </summary>
        public static void PGBackgroundColorClick(this VisualElement element, Color clickColor, StyleColor originalColor = default)
        {
            if (originalColor == default) originalColor = element.style.backgroundColor;
            element.RegisterCallback<MouseDownEvent>(evt => element.style.backgroundColor = clickColor);
            element.RegisterCallback<MouseUpEvent>(evt => element.style.backgroundColor = originalColor);
        }

        /// <summary>
        ///     Sets the opacity when clicking with the mouse.
        /// </summary>
        public static void PGOpacityClick(this VisualElement element, float value = 0.5f)
        {
            element.RegisterCallback<MouseDownEvent>(evt => element.style.opacity = value);
            element.RegisterCallback<MouseUpEvent>(evt => element.style.opacity = 1f);
        }
        
        /// <summary>
        ///     Set to the style of the child text input to Unity default Read Only.
        /// </summary>
        public static void PGTextStyleReadOnly(this VisualElement element)
        {
            var textInputField = element.PGGetTextInput();
            textInputField.style.backgroundColor = PGColors.ReadOnlyBackground();
            textInputField.style.color = PGColors.ReadOnlyText();
        }

        /// <summary>
        ///     Setup the element to a graphic style as an icon that can be clicked.
        /// </summary>
        public static void PGSetupClickableIcon(this VisualElement element, Color32 fallBackColor = default)
        {
            var defaultTintColor = element.style.unityBackgroundImageTintColor;
            if (!EqualityComparer<Color32>.Default.Equals(fallBackColor, default)) defaultTintColor = new StyleColor(fallBackColor);
            var defaultOpacity = element.style.opacity;
            element.RegisterCallback<MouseDownEvent>(evt => element.style.unityBackgroundImageTintColor = PGColors.ScenePlayPauseBlue());
            element.RegisterCallback<MouseUpEvent>(evt => element.style.unityBackgroundImageTintColor = defaultTintColor);
            element.RegisterCallback<MouseEnterEvent>(evt => element.style.opacity = 0.75f);
            element.RegisterCallback<MouseLeaveEvent>(evt =>
            {
                element.style.unityBackgroundImageTintColor = defaultTintColor;
                element.style.opacity = defaultOpacity;
            });
        }

        /// <summary>
        ///     Draw a simple line over the Visual Element.
        /// </summary>
        /// <param name="margin">Adds a margin of 4 above the line.</param>
        public static void PGDrawTopLine(this VisualElement element, bool margin = false)
        {
            element.style.borderTopWidth = 1f;
            element.style.borderTopColor = new StyleColor(PGColors.InspectorBorder());
            element.style.paddingTop = 4f;
            if(margin) element.style.marginTop = 4f;
        }

        /// <summary>
        ///     Draw a simple line under the Visual Element.
        /// </summary>
        /// <param name="margin">Adds a margin of 4 below the line.</param>
        public static void PGDrawBottomLine(this VisualElement element, bool margin = false)
        {
            element.style.borderBottomWidth = 1f;
            element.style.borderBottomColor = new StyleColor(PGColors.InspectorBorder());
            element.style.paddingBottom = 4f;
            if(margin) element.style.marginBottom = 4f;
        }
    }
}