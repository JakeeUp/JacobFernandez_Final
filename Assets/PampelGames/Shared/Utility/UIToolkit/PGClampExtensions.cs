// ---------------------------------------------------
// Copyright (c) Pampel Games e.K. All Rights Reserved.
// https://www.pampelgames.com
// ---------------------------------------------------

// UnityEditor.UIElements; is needed for Unity 2021. 
// Should be removed when deprecated.
#if UNITY_EDITOR
using UnityEditor.UIElements;
using UnityEngine;
using UnityEngine.UIElements;

namespace PampelGames.Shared
{
    public static class PGClampExtensions
    {

        /// <summary>
        ///     Register a ValueChangeCallback to clamp the values.
        /// </summary>
        public static void PGClampValue(this IntegerField field, int minValue = 0, int maxValue = int.MaxValue)
        {
            field.RegisterValueChangedCallback(evt =>
            {
                field.value = Mathf.Clamp(field.value, minValue, maxValue);
            });
        }

        public static void PGClampValue(this FloatField field, float minValue = 0f, float maxValue = Mathf.Infinity)
        {
            field.RegisterValueChangedCallback(evt =>
            {
                field.value = Mathf.Clamp(field.value, minValue, maxValue);
            });
        }
        
        
        /// <param name="yMinEqualsX">Minimum Y-value is the X-value.</param>
        public static void PGClampValue(this Vector2IntField field, int minValue = 0, int maxValue = int.MaxValue, bool yMinEqualsX = false)
        {
            field.RegisterValueChangedCallback(evt =>
            {
                var instanceAmountValue = field.value;
                instanceAmountValue.x = Mathf.Clamp(instanceAmountValue.x, minValue, maxValue);
                if(yMinEqualsX) instanceAmountValue.y = Mathf.Clamp(instanceAmountValue.y, field.value.x, maxValue);
                else instanceAmountValue.y = Mathf.Clamp(instanceAmountValue.y, minValue, maxValue);
                field.value = instanceAmountValue;
            });
        }
        
        /// <param name="yMinEqualsX">Minimum Y-value is the X-value.</param>
        public static void PGClampValue(this Vector2Field field, float minValue = 0f, float maxValue = Mathf.Infinity, bool yMinEqualsX = false)
        {
            field.RegisterValueChangedCallback(evt =>
            {
                var instanceAmountValue = field.value;
                instanceAmountValue.x = Mathf.Clamp(instanceAmountValue.x, minValue, maxValue);
                if(yMinEqualsX) instanceAmountValue.y = Mathf.Clamp(instanceAmountValue.y, field.value.x, maxValue);
                else instanceAmountValue.y = Mathf.Clamp(instanceAmountValue.y, minValue, maxValue);
                field.value = instanceAmountValue;
            });
        }
        
        public static void PGClampValue(this Vector3IntField field, int minValue = 0, int maxValue = int.MaxValue)
        {
            field.RegisterValueChangedCallback(evt =>
            {
                var instanceAmountValue = field.value;
                instanceAmountValue.x = Mathf.Clamp(instanceAmountValue.x, minValue, maxValue);
                instanceAmountValue.y = Mathf.Clamp(instanceAmountValue.y, minValue, maxValue);
                instanceAmountValue.z = Mathf.Clamp(instanceAmountValue.z, minValue, maxValue);
                field.value = instanceAmountValue;
            });
        }
        
        public static void PGClampValue(this Vector3Field field, float minValue = 0f, float maxValue = Mathf.Infinity)
        {
            field.RegisterValueChangedCallback(evt =>
            {
                var instanceAmountValue = field.value;
                instanceAmountValue.x = Mathf.Clamp(instanceAmountValue.x, minValue, maxValue);
                instanceAmountValue.y = Mathf.Clamp(instanceAmountValue.y, minValue, maxValue);
                instanceAmountValue.z = Mathf.Clamp(instanceAmountValue.z, minValue, maxValue);
                field.value = instanceAmountValue;
            });
        }
        
    }
}
#endif