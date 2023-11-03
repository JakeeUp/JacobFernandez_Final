// ----------------------------------------------------
// Copyright (c) Pampel Games e.K. All Rights Reserved.
// https://www.pampelgames.com
// ----------------------------------------------------

// UnityEditor.UIElements; is needed for Unity 2021. 
// Should be removed when deprecated.
#if UNITY_EDITOR
using UnityEditor.UIElements;
using UnityEngine;
using UnityEngine.UIElements;

namespace PampelGames.Shared
{
    public static class PGVectorFieldExtensions
    {
        /// <summary>
        ///     Changes the Vector2Field X and Y label (default is "X" and "Y").
        /// </summary>
        /// <param name="labelFlexGrow">FlexGrow default is 0 for the label and 1 for the text input. Must be increased if label text is bigger than 1 letter.</param>
        /// <param name="textAnchor">Default TextAnchor of the label is Left.</param>
        public static void PGVector2ComponentLabel(this Vector2Field vector2Field, string labelX, string labelY, float labelFlexGrow = 0.25f,
            TextAnchor textAnchor = TextAnchor.MiddleCenter)
        {
            var labelXFloatField = vector2Field.Q<FloatField>("unity-x-input");
            labelXFloatField.label = labelX;
            var labelYFloatField = vector2Field.Q<FloatField>("unity-y-input");
            labelYFloatField.label = labelY;

            var LabelX = labelXFloatField.Q<Label>();
            var LabelY = labelYFloatField.Q<Label>();

            LabelX.style.flexGrow = labelFlexGrow;
            LabelY.style.flexGrow = labelFlexGrow;
            LabelX.style.marginRight = labelFlexGrow * 4;
            LabelY.style.marginRight = labelFlexGrow * 4;

            labelXFloatField.style.unityTextAlign = textAnchor;
            labelYFloatField.style.unityTextAlign = textAnchor;
        }

        /// <summary>
        ///     Changes the Vector3Field X, Y and Z label (default is "X", "Y" and "Z").
        /// </summary>
        /// <param name="labelFlexGrow">FlexGrow default is 0 for the label and 1 for the text input. Must be increased if label text is bigger than 1 letter.</param>
        /// <param name="textAnchor">Default TextAnchor of the label is Left.</param>
        public static void PGVector3ComponentLabel(this Vector3Field vector3Field, string labelX, string labelY, string labelZ,
            float labelFlexGrow = 0.25f, TextAnchor textAnchor = TextAnchor.MiddleCenter)
        {
            var labelXFloatField = vector3Field.Q<FloatField>("unity-x-input");
            labelXFloatField.label = labelX;
            var labelYFloatField = vector3Field.Q<FloatField>("unity-y-input");
            labelYFloatField.label = labelY;
            var labelZFloatField = vector3Field.Q<FloatField>("unity-z-input");
            labelZFloatField.label = labelZ;

            var LabelX = labelXFloatField.Q<Label>();
            LabelX.style.flexGrow = labelFlexGrow;
            var LabelY = labelYFloatField.Q<Label>();
            LabelY.style.flexGrow = labelFlexGrow;
            var LabelZ = labelZFloatField.Q<Label>();
            LabelZ.style.flexGrow = labelFlexGrow;

            labelXFloatField.style.unityTextAlign = textAnchor;
            labelYFloatField.style.unityTextAlign = textAnchor;
            labelZFloatField.style.unityTextAlign = textAnchor;
        }


        /// <summary>
        ///     Changes the Vector2Field X and Y label (default is "X" and "Y").
        /// </summary>
        /// <param name="labelFlexGrow">FlexGrow default is 0 for the label and 1 for the text input. Must be increased if label text is bigger than 1 letter.</param>
        /// <param name="textAnchor">Default TextAnchor of the label is Left.</param>
        public static void PGVector2ComponentLabel(this Vector2IntField vector2Field, string labelX, string labelY, float labelFlexGrow = 0.25f,
            TextAnchor textAnchor = TextAnchor.MiddleCenter)
        {
            var labelXFloatField = vector2Field.Q<IntegerField>("unity-x-input");
            labelXFloatField.label = labelX;
            var labelYFloatField = vector2Field.Q<IntegerField>("unity-y-input");
            labelYFloatField.label = labelY;

            var LabelX = labelXFloatField.Q<Label>();
            LabelX.style.flexGrow = labelFlexGrow;
            var LabelY = labelYFloatField.Q<Label>();
            LabelY.style.flexGrow = labelFlexGrow;

            labelXFloatField.style.unityTextAlign = textAnchor;
            labelYFloatField.style.unityTextAlign = textAnchor;
        }

        /// <summary>
        ///     Changes the Vector3Field X, Y and Z label (default is "X", "Y" and "Z").
        /// </summary>
        /// <param name="labelFlexGrow">FlexGrow default is 0 for the label and 1 for the text input. Must be increased if label text is bigger than 1 letter.</param>
        /// <param name="textAnchor">Default TextAnchor of the label is Left.</param>
        public static void PGVector3ComponentLabel(this Vector3IntField vector3Field, string labelX, string labelY, string labelZ,
            float labelFlexGrow = 0.25f, TextAnchor textAnchor = TextAnchor.MiddleCenter)
        {
            var labelXFloatField = vector3Field.Q<IntegerField>("unity-x-input");
            labelXFloatField.label = labelX;
            var labelYFloatField = vector3Field.Q<IntegerField>("unity-y-input");
            labelYFloatField.label = labelY;
            var labelZFloatField = vector3Field.Q<IntegerField>("unity-z-input");
            labelZFloatField.label = labelZ;

            var LabelX = labelXFloatField.Q<Label>();
            LabelX.style.flexGrow = labelFlexGrow;
            var LabelY = labelYFloatField.Q<Label>();
            LabelY.style.flexGrow = labelFlexGrow;
            var LabelZ = labelZFloatField.Q<Label>();
            LabelZ.style.flexGrow = labelFlexGrow;

            labelXFloatField.style.unityTextAlign = textAnchor;
            labelYFloatField.style.unityTextAlign = textAnchor;
            labelZFloatField.style.unityTextAlign = textAnchor;
        }
    }
}
#endif