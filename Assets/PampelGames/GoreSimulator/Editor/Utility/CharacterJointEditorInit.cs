// ----------------------------------------------------
// Gore Simulator
// Copyright (c) Pampel Games e.K. All Rights Reserved.
// https://www.pampelgames.com
// ----------------------------------------------------

using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using PampelGames.Shared.Utility;

namespace PampelGames.GoreSimulator.Editor
{
    public static class CharacterJointEditorInit
    {
        public static void CreateCharacterJoints(GoreSimulator goreSimulator)
        {
            var smr = goreSimulator.smr;
            
            float totalMass = goreSimulator.ragdollTotalMass;
            int centerCount = goreSimulator.bonesClasses.Count(b => b.centralBone);
            int otherCount = goreSimulator.bonesClasses.Count - centerCount;

            float massPerCenter = totalMass / (2 * centerCount + otherCount);
            float massPerOther = massPerCenter / 2;
            
            
            for (int i = 0; i < goreSimulator.bonesClasses.Count; i++)
            {
                var bonesClass = goreSimulator.bonesClasses[i];
                var goreBone = bonesClass.bone.GetComponent<GoreBone>();
                
                
                var rigid = bonesClass.bone.GetComponent<Rigidbody>();
                if (rigid == null) rigid = bonesClass.bone.gameObject.AddComponent<Rigidbody>();
                goreBone._rigidbody = rigid;
                rigid.mass = bonesClass.centralBone ? massPerCenter : massPerOther;
                
                if(bonesClass.bone == goreSimulator.center) continue;
                var characterJoint = bonesClass.bone.GetComponent<CharacterJoint>();
                if (characterJoint == null) characterJoint = bonesClass.bone.gameObject.AddComponent<CharacterJoint>();
                else continue;
                goreBone._characterJoint = characterJoint;
             
                /********************************************************************************************************************************/
                // Joint values
                characterJoint.axis = PGEnums.GetAxis(goreSimulator.jointOrientation);

                if (Constants.ContainsName(bonesClass.bone.name, Constants.CenterBones()))
                {
                    if(!goreSimulator.inverseDirection)
                        SetCharacterJointValues(characterJoint, -10, 80, 15, 0);
                    else   
                        SetCharacterJointValues(characterJoint, -80, 10, 15, 0);

                }
                else if (Constants.ContainsName(bonesClass.bone.name, Constants.HeadBones()))
                {
                    if(!goreSimulator.inverseDirection)
                        SetCharacterJointValues(characterJoint, -30, 70, 25, 0);
                    else
                        SetCharacterJointValues(characterJoint, -70, 30, 25, 0);
                }
                else if (Constants.ContainsName(bonesClass.bone.name, Constants.ArmBones()))
                {
                    if(!goreSimulator.inverseDirection)
                        SetCharacterJointValues(characterJoint, -25, 120, 50, 0);
                    else
                        SetCharacterJointValues(characterJoint, -120, 25, 50, 0);
                }
                else if (Constants.ContainsName(bonesClass.bone.name, Constants.UpperLegBones()))
                {
                    if(!goreSimulator.inverseDirection)
                        SetCharacterJointValues(characterJoint, -25, 65, 20, 0);
                    else
                        SetCharacterJointValues(characterJoint, -65, 25, 20, 0);
                }
                else if (Constants.ContainsName(bonesClass.bone.name, Constants.LowerLegBones()))
                {
                    if(!goreSimulator.inverseDirection)
                        SetCharacterJointValues(characterJoint, -110, 0, 5, 0);
                    else
                        SetCharacterJointValues(characterJoint, 0, 110, 5, 0);
                }
                else
                {
                    if(!goreSimulator.inverseDirection)
                        SetCharacterJointValues(characterJoint, -10, 25, 5, 0);
                    else
                        SetCharacterJointValues(characterJoint, -25, 10, 5, 0);
                }
                
            }

            /********************************************************************************************************************************/
            
            for (int i = 0; i < goreSimulator.bonesClasses.Count; i++)
            {
                var bonesClass = goreSimulator.bonesClasses[i];
                if(bonesClass.bone == goreSimulator.center) continue;
                
                var characterJoint = bonesClass.bone.GetComponent<CharacterJoint>();

                if (bonesClass.parentExists)
                {
                    characterJoint.connectedBody = bonesClass.firstParent.GetComponent<Rigidbody>();
                }
            }
            
        }

        private static void SetCharacterJointValues(CharacterJoint characterJoint, 
            float lowTwistLimit, float highTwistLimit, float swing1Limit, float swing2Limit)
        {
            var characterJointLowTwistLimit = characterJoint.lowTwistLimit;
            characterJointLowTwistLimit.limit = lowTwistLimit;
            characterJoint.lowTwistLimit = characterJointLowTwistLimit;
                    
            var characterJointHighTwistLimit = characterJoint.highTwistLimit;
            characterJointHighTwistLimit.limit = highTwistLimit;
            characterJoint.highTwistLimit = characterJointHighTwistLimit;
                    
            var characterJointSwing1Limit = characterJoint.swing1Limit;
            characterJointSwing1Limit.limit = swing1Limit;
            characterJoint.swing1Limit = characterJointSwing1Limit;
                    
            var characterJointSwing2Limit = characterJoint.swing2Limit;
            characterJointSwing2Limit.limit = swing2Limit;
            characterJoint.swing2Limit = characterJointSwing2Limit;
        }
    }
}