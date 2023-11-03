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
    public static class PGListViewUtility
    {
        
        
        
        /********************************************************************************************************************************/
        /********************************************************************************************************************************/
        // Obsolete, try new Layout with Toolbar.
        
        
        /// <summary>
        ///     Used for the ListView.makeItem function. Creates a VisualElement "itemWrapper" with two children: Label "itemLabel" and Button "itemRemove".
        ///     If you want to parent an Element to the item, the list should be reorderable and then use "itemWrapper.parent.Add(Visualization);"
        /// </summary>
        /// <param name="createItemCopy">Create an "itemCopy" button.</param>
        public static VisualElement MakeItemWrapperReorderable(bool createItemCopy = false)
        {
            var itemWrapper = new VisualElement();
            itemWrapper.name = "itemWrapper";
            itemWrapper.style.flexDirection = FlexDirection.Row;
            itemWrapper.style.justifyContent = Justify.SpaceBetween;
            
            var itemWrapperLeft = new VisualElement();
            itemWrapperLeft.name = "itemWrapperLeft";
            itemWrapperLeft.style.flexDirection = FlexDirection.Row;
            itemWrapperLeft.style.flexGrow = 1f;
            
            var itemWrapperRight = new VisualElement();
            itemWrapperRight.name = "itemWrapperRight";
            itemWrapperRight.style.flexDirection = FlexDirection.Row;

            var itemLabel = new Label();
            itemLabel.name = "itemLabel";
            itemLabel.style.width = 180;
            itemLabel.style.unityTextAlign = TextAnchor.MiddleLeft;
            itemLabel.style.color = PGColors.InspectorVariableText();
            itemLabel.style.unityTextAlign = TextAnchor.MiddleLeft;
            itemLabel.PGBackgroundColorHover(PGColors.ListViewHover());
            itemLabel.style.unityFontStyleAndWeight = new StyleEnum<FontStyle>(FontStyle.Bold);
            itemLabel.style.flexGrow = 1f;

            var itemRemove = CreateItemRemoveButton();
            
            if (createItemCopy)
            {
                var itemCopy = CreateItemCopyButton();
                itemWrapperRight.Add(itemCopy);   
            }
            
            itemWrapperLeft.Add(itemLabel);
            itemWrapperRight.Add(itemRemove);
            
            itemWrapper.Add(itemWrapperLeft);
            itemWrapper.Add(itemWrapperRight);

            return itemWrapper;
        }

        public static VisualElement MakeItemWrapperNotReorderable()
        {
            var itemWrapperParent = new VisualElement();
            itemWrapperParent.style.backgroundColor = PGColors.ListViewBackground();
            itemWrapperParent.style.marginLeft = 8f;

            var itemWrapper = new VisualElement();
            itemWrapper.name = "itemWrapper";
            itemWrapper.style.flexDirection = FlexDirection.Row;

            var itemLabel = new Label();
            itemLabel.name = "itemLabel";
            itemLabel.style.width = 180;
            itemLabel.style.unityTextAlign = TextAnchor.MiddleLeft;
            itemLabel.style.color = PGColors.InspectorVariableText();
            itemLabel.style.flexGrow = 1f;
            itemLabel.style.unityTextAlign = TextAnchor.MiddleLeft;
            itemLabel.PGBackgroundColorHover(PGColors.ListViewHover());
            itemLabel.style.unityFontStyleAndWeight = new StyleEnum<FontStyle>(FontStyle.Bold);

            var itemRemove = CreateItemRemoveButton();

            itemWrapper.Add(itemLabel);
            itemWrapper.Add(itemRemove);
            itemWrapperParent.Add(itemWrapper);
            return itemWrapperParent;
        }
        
        /********************************************************************************************************************************/
        
        /// <summary>
        ///     Used with listView.itemIndexChanged to solve the issue that wrong items are removed after manually reordering the list.
        /// </summary>
        public static void CreateItemRemoves<T>(ListView listView, List<T> itemList, Action<int> itemRemovedClicked) where T : class
        {
            for (int i = 0; i < itemList.Count; i++)
            {
                if (listView.itemsSource == null) continue;
                if (i < 0 || i >= listView.itemsSource.Count) continue;
        
                var listItem = listView.GetRootElementForIndex(i);
                if (listItem == null) continue;
        
                var itemWrapperRight = listItem.Q<VisualElement>("itemWrapperRight");
                Button itemRemove = itemWrapperRight.Q<Button>("itemRemove");
        
                if (itemRemove != null) itemWrapperRight.Remove(itemRemove);
                itemRemove = CreateItemRemoveButton();
                itemRemove.clicked += () =>
                {
                    itemRemovedClicked.Invoke(i);
                };

                itemWrapperRight.Add(itemRemove);
            }
        }
        
        /// <summary>
        ///     Used with listView.itemIndexChanged to solve the issue that wrong items are copied after manually reordering the list.
        /// </summary>
        public static void CreateItemCopies<T>(ListView listView, List<T> itemList, Action<int> itemCopyClicked) where T : class
        {
            for (int i = 0; i < itemList.Count; i++)
            {
                if (listView.itemsSource == null) continue;
                if (i < 0 || i >= listView.itemsSource.Count) continue;
        
                var listItem = listView.GetRootElementForIndex(i);
                if (listItem == null) continue;
        
                var itemWrapperRight = listItem.Q<VisualElement>("itemWrapperRight");
                Button itemCopy = itemWrapperRight.Q<Button>("itemCopy");
        
                if (itemCopy != null) itemWrapperRight.Remove(itemCopy);
                itemCopy = CreateItemCopyButton();
                itemCopy.clicked += () =>
                {
                    itemCopyClicked.Invoke(i);
                };

                itemWrapperRight.Add(itemCopy);
            }
        }
        
        /// <summary>
        ///     Used with listView.itemIndexChanged to solve the issue that wrong items are checked after manually reordering the list.
        /// </summary>
        public static void CreateItemToggle<T>(ListView listView, List<T> itemList, Action<int> itemToggleClicked) where T : class
        {
            for (int i = 0; i < itemList.Count; i++)
            {
                if (listView.itemsSource == null) continue;
                if (i < 0 || i >= listView.itemsSource.Count) continue;
        
                var listItem = listView.GetRootElementForIndex(i);
                if (listItem == null) continue;
        
                var itemWrapperRight = listItem.Q<VisualElement>("itemWrapperRight");
                Toggle itemToggle = itemWrapperRight.Q<Toggle>("itemToggle");
        
                if (itemToggle != null) itemWrapperRight.Remove(itemToggle);
                
                itemWrapperRight.Add(itemToggle);
            }
        }
        
        /********************************************************************************************************************************/
        /********************************************************************************************************************************/

        private static Button CreateItemRemoveButton()
        {
            var itemRemove = new Button();
            itemRemove.name = "itemRemove";
            itemRemove.text = "-";
            itemRemove.tooltip = "Remove";
            itemRemove.PGBackgroundColorHover(PGColors.HoverButtonRed());
            return itemRemove;
        }

        private static Button CreateItemCopyButton()
        {
            var itemCopy = new Button();
            itemCopy.name = "itemCopy";
            itemCopy.text = "≡";
            itemCopy.tooltip = "Duplicate";
            itemCopy.PGBackgroundColorHover(PGColors.UnitySceneHeaderBlue());
            return itemCopy;
        }
        
    }
}