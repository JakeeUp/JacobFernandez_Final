// ----------------------------------------------------
// Gore Simulator
// Copyright (c) Pampel Games e.K. All Rights Reserved.
// https://www.pampelgames.com
// ----------------------------------------------------

using System.Collections.Generic;
using PampelGames.Shared;
using PampelGames.Shared.Editor;
using PampelGames.Shared.Utility;
using UnityEditor;
using UnityEditor.UIElements;
using UnityEngine;
using UnityEngine.UIElements;

namespace PampelGames.GoreSimulator.Editor
{
    [CustomPropertyDrawer(typeof(SubModuleParticleEffects))]
    public class SubModuleParticleEffectsDrawer : PropertyDrawer
    {
        private SubModuleParticleEffects _subModuleParticleEffects;
        
        
        private SerializedProperty particleClassesProperty;
        private readonly ListView particleClasses = new();
        


        public override VisualElement CreatePropertyGUI(SerializedProperty property)
        {
            var container = new VisualElement();

            var listIndex = PGPropertyDrawerUtility.GetDrawingListIndex(property);
            var obj = fieldInfo.GetValue(property.serializedObject.targetObject);
            var objList = obj as List<SubModuleBase>;
            _subModuleParticleEffects = (SubModuleParticleEffects) objList[listIndex];

            FindAndBindProperties(property);
            VisualizeProperties();

            CreateParticleClasses();

            container.Add(particleClasses);


            return container;
        }

        private void FindAndBindProperties(SerializedProperty property)
        {
            particleClassesProperty = property.FindPropertyRelative(nameof(SubModuleParticleEffects.particleClasses));
        }

        private void VisualizeProperties()
        {
            particleClasses.tooltip = "Particle Effects.";
        }

        private void CreateParticleClasses()
        {
            particleClasses.showBoundCollectionSize = false; // Important to avoid OutOfRangeException in BindItem.
            particleClasses.name = "particleClasses";
            particleClasses.itemsSource = _subModuleParticleEffects.particleClasses;
            
            particleClasses.PGObjectListViewStyle();
            
            particleClasses.makeItem = MakeItem;
            particleClasses.bindItem = BindItem;
        }
        
        private VisualElement MakeItem()
        {
            var item = new VisualElement();
            var particle = new ObjectField("Particle");
            var PositionWrapper = new VisualElement();
            PositionWrapper.name = "PositionWrapper";
            PositionWrapper.style.flexDirection = FlexDirection.Row;
            var position = new EnumField("Position");
            position.name = "position";
            var maxPosition = new IntegerField("Max. Amount");
            var setParent = new EnumField("Set Parent");
            setParent.name = "setParent";
            
            maxPosition.PGClampValue();
            maxPosition.PGRemoveLabelSpace();
            maxPosition.style.flexGrow = 0f;
            position.style.flexGrow = 1f;

            if (_subModuleParticleEffects.addedType == nameof(GoreModuleCut))
                PositionWrapper.style.display = DisplayStyle.None;   
            
            position.tooltip = "Spawn Position of the particle.\n" + "\n" +
                               "Center: Character mesh center.\n" +
                               "Method: Position specified in the explosion method.\n" +
                               "Parts: One spawn in the center of each explosion part.";

            maxPosition.tooltip = "Maximum amount of particles spawned per execution. Can not be higher than the amount of explosion parts.";
            
            var rotation = new EnumField("Rotation");
            rotation.name = "rotation";
            rotation.tooltip = "Rotation of the Particle System (Z+ is forward).";

            setParent.tooltip = "Sets the parent of the particle after spawning. If Both is selected, two particles will be instantiated.";

            if (_subModuleParticleEffects.addedType == nameof(GoreModuleCut))
            {
                rotation.tooltip += "\n" + "Note that Cut Direction does not apply to multi-cutted mesh.";
            }

            
            particle.objectType = typeof(ParticleSystem);
            
            item.Add(particle);
            PositionWrapper.Add(position);
            PositionWrapper.Add(maxPosition);
            item.Add(PositionWrapper);
            item.Add(rotation);
            item.Add(setParent);
            
            return item;
        }
        
        private void BindItem(VisualElement item, int index)
        {
            particleClassesProperty.serializedObject.Update();
                
            var particle = item.Q<ObjectField>();
            var PositionWrapper = item.Q<VisualElement>("PositionWrapper");
            var position = PositionWrapper.Q<EnumField>("position");
            var maxPosition = PositionWrapper.Q<IntegerField>();
            var rotation = item.Q<EnumField>("rotation");
            var setParent = item.Q<EnumField>("setParent");

            var listClassProperty = particleClassesProperty.GetArrayElementAtIndex(index);
            
            var particleProperty = listClassProperty.FindPropertyRelative(nameof(SubModuleParticleEffects.ParticleWrapperClass.particle));
            particle.BindProperty(particleProperty);
            var positionProperty = listClassProperty.FindPropertyRelative(nameof(SubModuleParticleEffects.ParticleWrapperClass.positionExpl));
            position.BindProperty(positionProperty);
            var maxPositionProperty = listClassProperty.FindPropertyRelative(nameof(SubModuleParticleEffects.ParticleWrapperClass.maxPosition));
            maxPosition.BindProperty(maxPositionProperty);
            var rotationProperty = listClassProperty.FindPropertyRelative(nameof(SubModuleParticleEffects.ParticleWrapperClass.rotationExpl));
            if (_subModuleParticleEffects.addedType == nameof(GoreModuleCut))
                rotationProperty = listClassProperty.FindPropertyRelative(nameof(SubModuleParticleEffects.ParticleWrapperClass.rotationCut));
            rotation.BindProperty(rotationProperty);
            setParent.PGSetupBindPropertyRelative(listClassProperty, nameof(SubModuleParticleEffects.ParticleWrapperClass.setParent));
            
            
            PositionDisplay();
            position.RegisterValueChangedCallback(evt => PositionDisplay());
            void PositionDisplay()
            {
                maxPosition.PGDisplayStyleFlex(_subModuleParticleEffects.particleClasses[index].positionExpl == Enums.ParticlePositionExpl.Parts);
            }

            rotation.RegisterValueChangedCallback(evt => SetParentDisplay());
            SetParentDisplay();
            void SetParentDisplay()
            {
                if (_subModuleParticleEffects.addedType != nameof(GoreModuleCut))
                {
                    setParent.style.display = DisplayStyle.None;
                }
                else
                {
                    setParent.PGDisplayStyleFlex(_subModuleParticleEffects.particleClasses[index].rotationCut == Enums.ParticleRotationCut.CutDirection);
                }

            }
            
        }
    }
}