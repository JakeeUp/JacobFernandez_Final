// ---------------------------------------------------
// Copyright (c) Pampel Games e.K. All Rights Reserved.
// https://www.pampelgames.com
// ---------------------------------------------------

using UnityEngine.UIElements;

namespace PampelGames.Shared.Utility
{
    public static class PGListViewExtensions
    {

        public static void PGStandardListViewStyle(this ListView listView)
        {
            listView.virtualizationMethod = CollectionVirtualizationMethod.DynamicHeight;
            listView.reorderable = true;
            listView.reorderMode = ListViewReorderMode.Animated;
            listView.showBorder = true;
            listView.style.backgroundColor = PGColors.InspectorBackground();
        }

        public static void PGObjectListViewStyle(this ListView listView, string headerTitle = null)
        {
            listView.virtualizationMethod = CollectionVirtualizationMethod.DynamicHeight;
            listView.showAddRemoveFooter = true;
            listView.PGHideScrollView();
            listView.showBorder = true;
            if (headerTitle != null)
            {
                listView.showFoldoutHeader = true;
                listView.headerTitle = headerTitle;
                listView.showBoundCollectionSize = true;
            }
        }
        public static void PGHideScrollView(this ListView listView, bool hideVertical = true, bool hideHorizontal = true)
        {
            if(hideVertical) listView.Q<ScrollView>().verticalScrollerVisibility = ScrollerVisibility.Hidden;
            if(hideHorizontal) listView.Q<ScrollView>().horizontalScrollerVisibility = ScrollerVisibility.Hidden;
        }
        public static Button PGGetRemoveButton(this ListView listView)
        {
            var removeButton = listView.Q<Button>("unity-list-view__remove-button");
            return removeButton;
        }
        
        public static Button PGGetAddButton(this ListView listView)
        {
            var addButton = listView.Q<Button>("unity-list-view__add-button");
            return addButton;
        }
        
        public static void PGHideAddButton(this ListView listView)
        {
            listView.showAddRemoveFooter = true;
            var addButton = listView.Q<Button>("unity-list-view__add-button");
            addButton.style.display = DisplayStyle.None;
        }
        

        public static void PGRemoveTopBorder(this ListView listView)
        {
            var scrollView = listView.Q<ScrollView>();
            scrollView.style.borderTopLeftRadius = 0f;
            scrollView.style.borderTopRightRadius = 0f;
            scrollView.style.borderTopWidth = 0f;
        }
        
        /// <summary>
        /// Set plus green and minus red on mouse hover.
        /// </summary>
        /// <param name="originalColor">If default, current color of the buttons will be used when mouse leaves.</param>
        public static void PGButtonsRedGreenHover(this ListView listView, StyleColor originalColor = default)
        {
            // var contentView = listView.Q<VisualElement>("unity-content-viewport");
            // contentView.style.backgroundColor = (Color) new Color32(77, 77, 77, 255);
            
            var removeButton = listView.Q<Button>("unity-list-view__remove-button");
            if (originalColor == default) originalColor = removeButton.style.backgroundColor;
            removeButton.RegisterCallback<MouseEnterEvent>((evt) => removeButton.style.backgroundColor = PGColors.HoverButtonRed());
            removeButton.RegisterCallback<MouseLeaveEvent>((evt) => removeButton.style.backgroundColor = originalColor);
            
            var addButton = listView.Q<Button>("unity-list-view__add-button");
            if (originalColor == default) originalColor = addButton.style.backgroundColor;
            addButton.RegisterCallback<MouseEnterEvent>((evt) => addButton.style.backgroundColor = PGColors.HoverButtonGreen());
            addButton.RegisterCallback<MouseLeaveEvent>((evt) => addButton.style.backgroundColor = originalColor);
        }
        

        
    }
}
