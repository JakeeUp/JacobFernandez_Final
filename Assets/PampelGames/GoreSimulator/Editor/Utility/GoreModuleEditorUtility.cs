// ----------------------------------------------------
// Gore Simulator
// Copyright (c) Pampel Games e.K. All Rights Reserved.
// https://www.pampelgames.com
// ----------------------------------------------------

using System.Collections.Generic;
using PampelGames.Shared.Utility;
using UnityEditor.UIElements;
using UnityEngine;
using UnityEngine.UIElements;

namespace PampelGames.GoreSimulator.Editor
{
    internal static class GoreModuleEditorUtility
    {
        internal static GroupBox CreateGoreGroup(Texture2D image, int index)
        {
            GroupBox GoreModule = new GroupBox();
            GoreModule.name = "GoreModule" + index;
            
            VisualElement ModuleWrapper = new VisualElement();
            ModuleWrapper.name = "ModuleWrapper";

            Toolbar ModuleToolbar = new Toolbar();
            ModuleToolbar.name = "ModuleToolbar";
            
            VisualElement LeftSideWrapper = new VisualElement();
            LeftSideWrapper.name = "LeftSideWrapper";
            VisualElement SubIcons = new VisualElement();
            SubIcons.name = "SubIcons";
            ToolbarToggle ModuleToggle = new ToolbarToggle();
            ModuleToggle.name = "ModuleToggle";
            
            VisualElement RightSideWrapper = new VisualElement();
            RightSideWrapper.name = "RightSideWrapper";
            ToolbarMenu addSubGoreMenu = new ToolbarMenu();
            addSubGoreMenu.name = "addSubGoreMenu";
            ToolbarMenu ModuleMenu = new ToolbarMenu();
            ModuleMenu.name = "ModuleMenu";
            VisualElement ModuleImage = new VisualElement();
            ModuleImage.name = "ModuleImage";
            
            VisualElement ModuleWrapperBottom = new VisualElement();
            ModuleWrapper.name = "ModuleWrapperBottom";
            VisualElement ModuleProperties = new VisualElement();
            ModuleProperties.name = "ModuleProperties";
            GroupBox SubModuleParent = new GroupBox();
            SubModuleParent.name = "SubModuleParent";


            SubModuleParent.PGPadding(3,0,0,5);
            SubModuleParent.PGMargin(4,4,0,2);

            SubModuleParentVisibility();
            ModuleToggle.RegisterValueChangedCallback(evt =>
            {
                SubModuleParentVisibility();
            });

            void SubModuleParentVisibility()
            {
                SubModuleParent.style.display = ModuleToggle.value ? DisplayStyle.Flex : DisplayStyle.None;
            }

            
            LeftSideWrapper.style.flexDirection = FlexDirection.Row;
            SubIcons.style.flexDirection = FlexDirection.Row;

            RightSideWrapper.style.flexDirection = FlexDirection.Row;
            RightSideWrapper.style.flexGrow = 0f;
            
            ModuleToolbar.style.justifyContent = Justify.SpaceBetween;
            ModuleToolbar.style.width = StyleKeyword.Auto;
            ModuleToolbar.style.height = StyleKeyword.Auto;
            ModuleToolbar.PGBorderWidth(1);
            ModuleToolbar.PGBorderRadius(3);

            addSubGoreMenu.text = "+";
            addSubGoreMenu.style.unityFontStyleAndWeight = new StyleEnum<FontStyle>(FontStyle.Bold);
            addSubGoreMenu.style.fontSize = 16;
            
            
            ModuleImage.style.width = 50;
            ModuleImage.style.height = 50;
            ModuleImage.style.backgroundImage = image;
            
            ModuleMenu.PGBorderWidth(1, 0, 0, 0);
            
            
            ModuleToggle.Add(ModuleImage);
            LeftSideWrapper.Add(ModuleToggle);
            LeftSideWrapper.Add(SubIcons);
            ModuleToolbar.Add(LeftSideWrapper);
            RightSideWrapper.Add(addSubGoreMenu);
            RightSideWrapper.Add(ModuleMenu);
            ModuleToolbar.Add(RightSideWrapper);
            ModuleWrapperBottom.Add(ModuleProperties);
            ModuleWrapperBottom.Add(SubModuleParent);
            ModuleWrapper.Add(ModuleToolbar);
            ModuleWrapper.Add(ModuleWrapperBottom);
            GoreModule.Add(ModuleWrapper);

            return GoreModule;
        }

        internal static VisualElement CreateSubImage(Texture2D image, string tooltip)
        {
            var subIcon = new VisualElement();
            subIcon.style.backgroundImage = image;
            subIcon.style.width = 30;
            subIcon.style.height = 30;
            subIcon.tooltip = tooltip;
            return subIcon;
        }
        
        internal static VisualElement CreateSubGroup(Texture2D image, string labelName)
        {
            VisualElement SubModule = new VisualElement();
            SubModule.name = "SubModule";
            
            Toolbar SubModuleToolbar = new Toolbar();
            SubModuleToolbar.name = "SubModuleToolbar";
            VisualElement LeftSideWrapper = new VisualElement();
            VisualElement SubModuleImage = new VisualElement();
            SubModuleImage.name = "SubModuleImage";
            Label SubModuleLabel = new Label(labelName);
            ToolbarMenu SubModuleMenu = new ToolbarMenu();
            SubModuleMenu.name = "SubModuleMenu";
            VisualElement SubModuleProperties = new VisualElement();
            SubModuleProperties.name = "SubModuleProperties";
            
            SubModuleProperties.PGMargin(6, 3, 3, 0);
            
            SubModuleToolbar.style.justifyContent = Justify.SpaceBetween;
            SubModuleToolbar.style.width = StyleKeyword.Auto;
            SubModuleToolbar.style.height = StyleKeyword.Auto;
            SubModuleToolbar.PGBorderWidth(0,0,0,1);
            SubModuleToolbar.PGBorderRadius(3);
            SubModuleToolbar.style.backgroundColor = new StyleColor(PGColors.UnityEventBackground());

            LeftSideWrapper.style.flexDirection = FlexDirection.Row;
            
            SubModuleImage.style.width = 31;
            SubModuleImage.style.height = 31;
            SubModuleImage.style.backgroundImage = image;

            SubModuleLabel.style.alignSelf = Align.Center;
            SubModuleLabel.PGMargin(6,6,0,0);
            SubModuleLabel.PGBoldText();
            
            SubModule.PGMargin(0,0,3,0);
            SubModule.PGBorderWidth(1);
            SubModule.PGBorderColor(PGColors.CustomBorder());
            SubModule.style.backgroundColor = new StyleColor(PGColors.UnityEventBackground());

            SubModuleMenu.PGBorderWidth(1, 0, 0, 0);
            
            LeftSideWrapper.Add(SubModuleImage);
            LeftSideWrapper.Add(SubModuleLabel);
            SubModuleToolbar.Add(LeftSideWrapper);
            SubModuleToolbar.Add(SubModuleMenu);
            SubModule.Add(SubModuleToolbar);
            SubModule.Add(SubModuleProperties);

            return SubModule;
        }
        
        /********************************************************************************************************************************/
        
        public static GroupBox FindSubModule<T>(GoreSimulator goreSimulator, VisualElement GoreModuleParent)
        {
            GroupBox goreModule = null;
            for (var i = 0; i < goreSimulator.goreModules.Count; i++)
            {
                if(goreSimulator.goreModules[i].GetType() != typeof(T)) continue;
                goreModule = GoreModuleParent.Q<GroupBox>("GoreModule" + i);
                break;
            }

            return goreModule;
        }
        
        public static List<GroupBox> FindAllSubModules(GoreSimulator goreSimulator, VisualElement GoreModuleParent)
        {
            List<GroupBox> goreModules = new List<GroupBox>();
            for (var i = 0; i < goreSimulator.goreModules.Count; i++)
            {
                GroupBox goreModule = GoreModuleParent.Q<GroupBox>("GoreModule" + i);
                goreModules.Add(goreModule);
            }
            return goreModules;
        }
    }
}