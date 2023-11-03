// ----------------------------------------------------
// Copyright (c) Pampel Games e.K. All Rights Reserved.
// https://www.pampelgames.com
// ----------------------------------------------------

using UnityEngine;

namespace PampelGames.Shared.Utility
{
    public static class PGEnums
    {
        public enum AxisEnum
        {
            [InspectorName("X+")] XPlus,
            [InspectorName("Y+")] YPlus,
            [InspectorName("Z+")] ZPlus,
            [InspectorName("X-")] XMinus,
            [InspectorName("Y-")] YMinus,
            [InspectorName("Z-")] ZMinus
        }
        
        public static Vector3 GetAxis(AxisEnum axis)
        {
            return m_AlignAxisToVector[(int) axis];
        }
        
        private static readonly Vector3[] m_AlignAxisToVector =
        {
            Vector3.right, Vector3.up, Vector3.forward, Vector3.left, Vector3.down, Vector3.back
        };
        
        
        /********************************************************************************************************************************/

        public enum RenderPipelineEnum
        {
            BuiltIn,
            URP,
            HDRP
        }
        
        /********************************************************************************************************************************/
        public enum UpdateMode
        {
            Update,
            FixedUpdate,
            LateUpdate
        }
        
        public static YieldInstruction GetYieldInstruction(UpdateMode axis)
        {
            return yieldInstructions[(int) axis];
        }
        
        private static readonly YieldInstruction[] yieldInstructions =
        {
            null, // corresponds to Update
            new WaitForFixedUpdate(), // corresponds to FixedUpdate
            new WaitForEndOfFrame() // corresponds to LateUpdate
        };
        
        /********************************************************************************************************************************/

    }
}
