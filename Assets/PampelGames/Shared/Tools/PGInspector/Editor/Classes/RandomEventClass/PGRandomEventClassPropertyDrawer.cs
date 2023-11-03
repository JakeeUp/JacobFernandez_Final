// ---------------------------------------------------
// Copyright (c) Pampel Games e.K. All Rights Reserved.
// https://www.pampelgames.com
// ---------------------------------------------------

#if UNITY_EDITOR
using PampelGames.Shared.Utility;
using UnityEditor;
using UnityEditor.UIElements;
using UnityEngine.UIElements;

namespace PampelGames.Shared.Tools.PGInspector.Editor
{
    [CustomPropertyDrawer(typeof(PGRandomEventClass))]
    public class PGRandomEventClassPropertyDrawer : PropertyDrawer
    {
        private PGRandomEventClass baseClass;

        private SerializedProperty randomModeProperty;
        private readonly EnumField randomMode = new("Random Mode");

        private SerializedProperty eventWrappersProperty;
        private readonly ListView eventWrappers = new();


        public override VisualElement CreatePropertyGUI(SerializedProperty property)
        {
            var container = new VisualElement();

            FindAndBindProperties(property);
            DrawRandomEventClass(property);

            container.Add(randomMode);
            container.Add(eventWrappers);

            return container;
        }

        private void FindAndBindProperties(SerializedProperty property)
        {
            baseClass = fieldInfo.GetValue(property.serializedObject.targetObject) as PGRandomEventClass;
            if(baseClass.randomEvents.Count == 0) baseClass.randomEvents.Add(new PGRandomEventClass.RandomEventWrapperClass());
            property.serializedObject.ApplyModifiedProperties();

            randomModeProperty = property.FindPropertyRelative(nameof(PGRandomEventClass.randomMode));
            randomMode.BindProperty(randomModeProperty);

            eventWrappersProperty = property.FindPropertyRelative(nameof(PGRandomEventClass.randomEvents));
            eventWrappers.itemsSource = baseClass.randomEvents;
        }


        /*******************************************************************************************************************************/

        private void DrawRandomEventClass(SerializedProperty property)
        {
            eventWrappers.PGStandardListViewStyle();
            eventWrappers.showAddRemoveFooter = true;

            DrawUnityEventsList(property);
        }

        private void DrawUnityEventsList(SerializedProperty property)
        {
            VisualElement MakeItem()
            {
                var wrapper = new VisualElement();
                wrapper.style.flexDirection = FlexDirection.Row;

                Slider itemSlider = new();
                itemSlider.name = "itemSlider";
                itemSlider.tooltip = "Probability weight of this event either in relation to the others or total, depending on the Random Mode.";
                itemSlider.direction = SliderDirection.Vertical;
                itemSlider.showInputField = true;
                itemSlider.highValue = 1f;
                var textField = itemSlider.Q<TextField>("unity-text-field");
                itemSlider.style.height = 90f;
                textField.style.width = 33f;
                itemSlider.style.flexGrow = 0.1f;

                PropertyField itemEvent = new();
                itemEvent.name = "itemEvent";
                itemEvent.style.flexGrow = 1f;
                itemEvent.tooltip = "Call InvokeRandomEvent() to start the random event selection.";

                wrapper.Add(itemSlider);
                wrapper.Add(itemEvent);

                return wrapper;
            }

            void BindItem(VisualElement _itemWrapper, int i)
            {
                var itemSlider = _itemWrapper.Q<Slider>("itemSlider");
                var itemEvent = _itemWrapper.Q<PropertyField>("itemEvent");

                if (i < 0 || eventWrappersProperty.arraySize <= i) return;

                property.serializedObject.Update();
                property.serializedObject.ApplyModifiedProperties();

                var unityEventsProperty = eventWrappersProperty.GetArrayElementAtIndex(i)
                    .FindPropertyRelative(nameof(PGRandomEventClass.RandomEventWrapperClass.unityEvent));

                var instancesWeightsProperty = eventWrappersProperty.GetArrayElementAtIndex(i)
                    .FindPropertyRelative(nameof(PGRandomEventClass.RandomEventWrapperClass.instanceWeight));

                itemEvent.BindProperty(unityEventsProperty);
                itemSlider.BindProperty(instancesWeightsProperty);
            }

            eventWrappers.makeItem = MakeItem;
            eventWrappers.bindItem = BindItem;
        }
    }
}
#endif