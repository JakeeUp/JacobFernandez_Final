// ----------------------------------------------------
// Copyright (c) Pampel Games e.K. All Rights Reserved.
// https://www.pampelgames.com
// ----------------------------------------------------

using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Runtime.Serialization;
using UnityEngine;

namespace PampelGames.Shared.Utility
{
    public static class PGClassUtility
    {
        
        /// <summary>
        ///     Generates a list of all inheritors of a class type.
        /// </summary>
        /// <param name="assemblies">var assemblies = AppDomain.CurrentDomain.GetAssemblies();</param>
        /// <typeparam name="T">Type of the base class.</typeparam>
        public static List<T> CreateInstances<T>(Assembly[] assemblies)
            where T : class
        {
            var instances = new List<T>();

            foreach (var type in assemblies.SelectMany(a => a.GetTypes())
                         .Where(t => typeof(T).IsAssignableFrom(t) && t.IsClass && !t.IsAbstract))
            {
                var instance = (T) FormatterServices.GetUninitializedObject(type);
                instances.Add(instance);
            }

            return instances;
        }
        
        /// <summary>
        ///     Creates a copy of a class instance, including all properties and fields.
        /// </summary>
        /// <param name="source">Instance of the class that should be copied.</param>
        /// <returns>Object that has to be casted to the class.</returns>
        public static object CopyClass(object source)
        {
            return CopyClassInternal(source);
        }

        /// <summary>
        ///     Copies the fields and properties from the source object to the target object.
        /// </summary>
        /// <param name="source">The source object to copy from.</param>
        /// <param name="target">The target object to copy to. Can also be a derived class.</param>
        public static void CopyClassValues(object source, object target)
        {
            CopyClassValuesInternal(source, target);
        }
        

        /********************************************************************************************************************************/
        /********************************************************************************************************************************/

        private static object CopyClassInternal(object source)
        {
            var sourceType = source.GetType();
            var targetType = sourceType.Assembly.GetType(sourceType.FullName);

            if (targetType != null)
            {
                var targetInstance = Activator.CreateInstance(targetType);

                CopyProperties(source, targetInstance);
                CopyFields(source, targetInstance);

                return targetInstance;
            }

            return targetType;
        }

        private static void CopyClassValuesInternal(object source, object target)
        {
            CopyProperties(source, target);
            CopyFields(source, target);
        }
        
        private static void CopyProperties(object source, object target)
        {
            var targetType = target.GetType();
            var properties = targetType.GetProperties();

            foreach (var property in properties)
            {
                if (property.CanRead && property.CanWrite)
                {
                    var value = property.GetValue(source);
                    if (value is AnimationCurve curve)
                    {
                        property.SetValue(target, CopyAnimationCurve(curve));
                    }
                    else
                    {
                        property.SetValue(target, value);
                    }
                }
            }
        }

        private static void CopyFields(object source, object target)
        {
            var targetType = target.GetType();

            var fields = targetType.GetFields();

            foreach (var field in fields)
            {
                var value = field.GetValue(source);
                if (value is AnimationCurve curve)
                {
                    field.SetValue(target, CopyAnimationCurve(curve));
                }
                else
                {
                    field.SetValue(target, value);
                }
            }
        }
        

        private static AnimationCurve CopyAnimationCurve(AnimationCurve curve)
        {
            var copiedCurve = new AnimationCurve(curve.keys)
            {
                postWrapMode = curve.postWrapMode,
                preWrapMode = curve.preWrapMode
            };
            return copiedCurve;
        }



    }
}