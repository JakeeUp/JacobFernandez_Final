// ----------------------------------------------------
// Copyright (c) Pampel Games e.K. All Rights Reserved.
// https://www.pampelgames.com
// ----------------------------------------------------

#if UNITY_EDITOR
using System.Collections.Generic;
using PampelGames.Shared.Utility;
using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;

namespace PampelGames.Shared.Tools.PGInspector.Editor
{
    /// <summary>
    ///     Base class for components implementing <see cref="PGIHeader"/>.
    ///     Add HeaderUXML on top of the UXML file.
    /// </summary>
    public abstract class PGHeaderBaseInspector : UnityEditor.Editor
    {
        protected VisualElement container;
        public VisualTreeAsset _visualTree;

        protected VisualElement Header;
        protected VisualElement IconsLeft;

        protected VisualElement additionalIcon01;

        private VisualElement Logo;
        private VisualElement Icons;
        private VisualElement documentation;
        private VisualElement globalSettings;
        private VisualElement PlayIcons;
        private VisualElement playIcon;
        private VisualElement pauseIcon;
        private VisualElement stopIcon;
        private VisualElement reInitializeIcon;

        protected virtual void OnEnable()
        {
            container = new VisualElement();
            _visualTree.CloneTree(container);
            FindElements();
        }

        public override VisualElement CreateInspectorGUI()
        {
            IconDocumentation();
            IconGlobalSettings();
            AdditionalIcons();
            IconsPlay();
            SetInspectorLogo();

            DrawInspector();

            return container;
        }

        private void FindElements()
        {
            Header = container.Q<VisualElement>("Header");
            Logo = container.Q<VisualElement>("Logo");
            Icons = container.Q<VisualElement>("Icons");
            
            documentation = container.Q<VisualElement>("documentation");
            globalSettings = container.Q<VisualElement>("globalSettings");

            additionalIcon01 = Icons.Q<VisualElement>("additionalIcon01");

            IconsLeft = container.Q<VisualElement>(nameof(IconsLeft));
            PlayIcons = container.Q<VisualElement>("PlayIcons");
            playIcon = container.Q<VisualElement>("playIcon");
            pauseIcon = container.Q<VisualElement>("pauseIcon");
            stopIcon = container.Q<VisualElement>("stopIcon");
            reInitializeIcon = container.Q<VisualElement>("reInitializeIcon");
        }
        private void IconDocumentation()
        {
            documentation.RegisterCallback<ClickEvent>(evt => Application.OpenURL(DocumentationURL()));
            documentation.PGSetupClickableIcon();
        }
        private void IconGlobalSettings()
        {
            if (!UseGlobalSettings())
            {
                globalSettings.style.display = DisplayStyle.None;
                return;
            }
            globalSettings.RegisterCallback<ClickEvent>(evt => OpenGlobalSettingsWindow());
            globalSettings.PGSetupClickableIcon();
        }

        private void IconsPlay()
        {
            playIcon.PGSetupClickableIcon();
            pauseIcon.PGSetupClickableIcon();
            stopIcon.PGSetupClickableIcon();
            reInitializeIcon.PGSetupClickableIcon();
            
            
            playIcon.RegisterCallback<ClickEvent>(evt =>
            {
                if (!EditorApplication.isPlaying) return;
                Execute();
            });
            pauseIcon.RegisterCallback<ClickEvent>(evt =>
            {
                if (!EditorApplication.isPlaying) return;
                if (!IsPaused()) Pause();
                else Resume();
            });
            stopIcon.RegisterCallback<ClickEvent>(evt =>
            {
                if (!EditorApplication.isPlaying) return;
                Stop();
            });
            reInitializeIcon.RegisterCallback<ClickEvent>(evt =>
            {
                if (!EditorApplication.isPlaying) return;
                ReInitialize();
            });
            
            IconsPlayVisibility();
            EditorApplication.playModeStateChanged += change => { IconsPlayVisibility(); };
        }

        private void IconsPlayVisibility()
        {
            if (EditorApplication.isPlaying)
                PlayIcons.style.opacity = 1f;
            else
                PlayIcons.style.opacity = 0.25f;
        }
        
        private void SetInspectorLogo()
        {
            var logo = InspectorLogo();
            logo.style.flexGrow = 1f;
            Icons.Insert(1, logo);
        }

        /********************************************************************************************************************************/

        /// <summary>
        ///     URL link of the documentation provided.
        /// </summary>
        protected abstract string DocumentationURL();
        
        /// <summary>
        ///     Returning false will set the Global Setting icon to DisplayStyle.None.
        /// </summary>
        protected abstract bool UseGlobalSettings();
        
        /// <summary>
        ///     If global settings are used, open the settings Editor Window with this method.
        /// </summary>
        protected virtual void OpenGlobalSettingsWindow(){}

        /// <summary>
        ///     Optional. Set Display.Flex and assign background image.
        /// </summary>
        protected virtual void AdditionalIcons()
        {
            additionalIcon01.PGSetupClickableIcon();
        }
        
        /// <summary>
        /// Logo on top of the inspector.
        /// </summary>
        protected abstract VisualElement InspectorLogo();
        
        /// <summary>
        ///     Draw the inspector using the container Visual Element.
        /// </summary>
        protected abstract void DrawInspector();
        
        /********************************************************************************************************************************/
        
        /// <summary>
        ///     Execute the underlying script. Only visible in Playmode.
        /// </summary>
        private void Execute()
        {
            ((PGIHeader) target).Execute();
        }

        /// <summary>
        ///     Pause the underlying script. Only visible in Playmode.
        /// </summary>
        private void Pause()
        {
            ((PGIHeader) target).Pause();
        }

        /// <summary>
        ///     Resume pause of the underlying script. Only visible in Playmode.
        /// </summary>
        private void Resume()
        {
            ((PGIHeader) target).Resume();
        }

        /// <summary>
        ///     Stop the underlying script. Only visible in Playmode.
        /// </summary>
        private void Stop()
        {
            ((PGIHeader) target).Stop();
        }
        
        /// <summary>
        ///     Re-Initialize the underlying script. Only visible in Playmode.
        /// </summary>
        private void ReInitialize()
        {
            ((PGIHeader) target).Stop();
            ((PGIHeader) target).ReInitialize();
        }

        /// <summary>
        ///     Is the underlying script currently executing?
        /// </summary>
        private bool IsExecuting()
        {
            return ((PGIHeader) target).IsExecuting();
        }

        /// <summary>
        ///     Is the underlying script currently in Pause mode?
        /// </summary>
        private bool IsPaused()
        {
            return ((PGIHeader) target).IsPaused();
        }
    }
}
#endif