// ---------------------------------------------------
// Copyright (c) Pampel Games e.K. All Rights Reserved.
// https://www.pampelgames.com
// ---------------------------------------------------

#if UNITY_EDITOR
using PampelGames.Shared.Utility;
using UnityEditor;
using UnityEditor.UIElements;
using UnityEngine;
using UnityEngine.UIElements;

namespace PampelGames.Shared.Tools.Editor
{

    [CustomPropertyDrawer(typeof(PGTweenSharedClass))]
    public class PGTweenSharedClassPropertyDrawer : PropertyDrawer
    {
        
        private SerializedProperty easeProperty;
        private readonly EnumField ease = new();
        private SerializedProperty animationCurveProperty;
        private readonly CurveField animationCurve = new("Animation Curve");
        private readonly Button easeTypesButton = new();
        
        private SerializedProperty amplitudeProperty;
        private FloatField amplitude = new();
        
        // Curve Creator
        private readonly Button showPresetsButton = new();
        private readonly VisualElement Preset = new();
        private SerializedProperty shakeFrequencyProperty;
        private readonly IntegerField shakeFrequency = new();
        private SerializedProperty maxPositiveProperty;
        private readonly Slider maxPositive = new();
        private SerializedProperty maxNegativeProperty;
        private readonly Slider maxNegative = new();
        private SerializedProperty multiplierProperty;
        private readonly FloatField multiplier = new();
        private SerializedProperty flipProperty;
        private readonly Toggle flip = new();

        private readonly VisualElement ButtonWrapper = new();
        private readonly Button setPresetButton = new();
        private readonly Button hideButton = new();

        

        public override VisualElement CreatePropertyGUI(SerializedProperty property)
        {
            var container = new VisualElement();
            if (property == null) return container;
            
            FindAndBindProperties(property);
            DrawPGTweenSharedClass(property);

            container.Add(ease);
            container.Add(animationCurve);
            container.Add(amplitude);
            container.Add(Preset);

            return container;
        }

        private void FindAndBindProperties(SerializedProperty property)
        {
            easeProperty = property.FindPropertyRelative(nameof(PGTweenSharedClass.ease));
            ease.BindProperty(easeProperty);
            animationCurveProperty = property.FindPropertyRelative(nameof(PGTweenSharedClass.animationCurve));
            animationCurve.BindProperty(animationCurveProperty);

            amplitudeProperty = property.FindPropertyRelative(nameof(PGTweenSharedClass.amplitude));
            amplitude.BindProperty(amplitudeProperty);
            
            shakeFrequencyProperty = property.FindPropertyRelative(nameof(PGTweenSharedClass.shakeFrequency));
            shakeFrequency.BindProperty(shakeFrequencyProperty);
            maxPositiveProperty = property.FindPropertyRelative(nameof(PGTweenSharedClass.maxPositive));
            maxPositive.BindProperty(maxPositiveProperty);
            maxNegativeProperty = property.FindPropertyRelative(nameof(PGTweenSharedClass.maxNegative));
            maxNegative.BindProperty(maxNegativeProperty);
            multiplierProperty = property.FindPropertyRelative(nameof(PGTweenSharedClass.multiplier));
            multiplier.BindProperty(multiplierProperty);
            flipProperty = property.FindPropertyRelative(nameof(PGTweenSharedClass.flip));
            flip.BindProperty(flipProperty);
            
        }

        private void DrawPGTweenSharedClass(SerializedProperty property)
        {
            animationCurve.tooltip = "The time axis does not influence the duration.\n" +
                                     "The value axis is normalized from 0 to 1 and influences the value.";
            ease.label = "Ease Type";
            ease.tooltip = "Click on the question mark for a visualization of the different types.";
            animationCurve.label = "Animation Curve";
            easeTypesButton.text = "?";
            easeTypesButton.tooltip = "Sheet for Easing Functions.";
            showPresetsButton.text = "Create";
            showPresetsButton.tooltip = "Open the Curve Creator";
            animationCurve.Add(showPresetsButton);

            amplitude.tooltip =
                "Click on the question mark to get a visualization on how the amplitude works for specific ease types.";
            amplitude.label = "Amplitude";

            AnimatioCurveVisibility();
            AmplitudeVisibility();
            ease.RegisterValueChangedCallback((evt =>
            {
                AnimatioCurveVisibility();
                AmplitudeVisibility();
            }));

            var existingEaseTypesButton = ease.Q<Button>();
            if (existingEaseTypesButton == null)
            {
                easeTypesButton.clicked += PGTweenDocumentationEditor.OpenEasingTypes;
                ease.style.flexDirection = FlexDirection.Row;
                ease.Add(easeTypesButton);
                
            }
            

            Preset.style.display = DisplayStyle.None;
            showPresetsButton.clicked += () =>
            {
                Preset.style.display = DisplayStyle.Flex;
            };

            shakeFrequency.label = "Frequency";

            maxPositive.label = "Max. Positive";
            maxPositive.lowValue = 0f;
            maxPositive.highValue = 1f;
            maxPositive.showInputField = true;
            maxNegative.label = "Max. Negative";
            maxNegative.lowValue = -1f;
            maxNegative.highValue = 0f;
            maxNegative.showInputField = true;

            multiplier.label = "Multiplier";
 
            multiplier.RegisterValueChangedCallback(evt =>
            {
                multiplier.value = Mathf.Clamp(multiplier.value, 0, Mathf.Infinity);
            });
            amplitude.RegisterValueChangedCallback(evt =>
            {
                amplitude.value = Mathf.Clamp(amplitude.value, 0, 100);
            });

            flip.label = "Flip";

            setPresetButton.text = "Apply Curve";
            setPresetButton.tooltip = "Apply the animation curve with the specified settings.";
            setPresetButton.RegisterCallback<MouseEnterEvent>( (evt) => setPresetButton.style.backgroundColor = (Color) new Color32(38, 252, 88, 125));
            setPresetButton.RegisterCallback<MouseLeaveEvent>( (evt) => setPresetButton.style.backgroundColor = (Color) new Color32(88, 88, 88, 255));
            setPresetButton.clicked += SetAnimationCurvePreset;
            setPresetButton.style.flexGrow = 2f;
            
            hideButton.text = "Hide";
            hideButton.tooltip = "Hide the Curve Creator.";
            hideButton.clicked += () =>
            {
                Preset.style.display = DisplayStyle.None;
            };

            ButtonWrapper.style.flexDirection = FlexDirection.Row;
            ButtonWrapper.Add(setPresetButton);
            ButtonWrapper.Add(hideButton);
            
            Preset.Add(shakeFrequency);
            Preset.Add(maxPositive);
            Preset.Add(maxNegative);
            Preset.Add(multiplier);
            Preset.Add(flip);
            Preset.Add(ButtonWrapper);

            Preset.style.borderTopWidth = 5;
            Preset.style.borderRightWidth = 5;
            Preset.style.borderBottomWidth = 5;
            Preset.style.borderLeftWidth = 5;

            Preset.style.borderTopLeftRadius = 3;
            Preset.style.borderTopRightRadius = 3;
            Preset.style.borderBottomLeftRadius = 3;
            Preset.style.borderBottomRightRadius = 3;

            Preset.style.borderTopColor = (Color) new Color32(36, 36, 36, 255);
            Preset.style.borderBottomColor = (Color) new Color32(36, 36, 36, 255);
            Preset.style.borderLeftColor = (Color) new Color32(36, 36, 36, 255);
            Preset.style.borderRightColor = (Color) new Color32(36, 36, 36, 255);
            
        }

        private void AnimatioCurveVisibility()
        {
            if (easeProperty.enumValueFlag == (int) PGTweenEase.Ease.AnimationCurve)
            {
                animationCurve.style.display = DisplayStyle.Flex;
            }
            else
            {
                animationCurve.style.display = DisplayStyle.None;
            }
        }

        private void AmplitudeVisibility()
        {
            if (easeProperty.enumValueFlag == (int) PGTweenEase.Ease.InBack ||
                easeProperty.enumValueFlag == (int) PGTweenEase.Ease.OutBack ||
                easeProperty.enumValueFlag == (int) PGTweenEase.Ease.InOutBack ||
                easeProperty.enumValueFlag == (int) PGTweenEase.Ease.InElastic ||
                easeProperty.enumValueFlag == (int) PGTweenEase.Ease.OutElastic ||
                easeProperty.enumValueFlag == (int) PGTweenEase.Ease.InOutElastic)
                amplitude.style.display = DisplayStyle.Flex;
            else
                amplitude.style.display = DisplayStyle.None;
        }

        private void SetAnimationCurvePreset()
        {
            animationCurve.value = PGAnimationCurvesUtility.CreateShakeCurve(maxPositive.value, maxNegative.value, shakeFrequency.value, multiplier.value, flip.value);
        }

    }
}
#endif