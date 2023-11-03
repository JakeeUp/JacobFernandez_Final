// ----------------------------------------------------
// Gore Simulator
// Copyright (c) Pampel Games e.K. All Rights Reserved.
// https://www.pampelgames.com
// ----------------------------------------------------

using System.Collections.Generic;
using UnityEngine;

namespace PampelGames.GoreSimulator
{
    /// <summary>
    ///     Child GameObjects that are not part of the Skinned Mesh Renderer (items etc.).
    ///     Also used for skinned children, just to recognize the child in the sub-modules.
    /// </summary>
    public class DetachedChild : MonoBehaviour
    {
        public int layer;
        
        private readonly List<Component> addedComponents = new();

        public void RegisterComponent(Component component)
        {
            addedComponents.Add(component);
        }

        public void RemoveAddedComponents()
        {
            foreach (var component in addedComponents)
            {
                Destroy(component);
            }
            addedComponents.Clear();
        }
    }
}