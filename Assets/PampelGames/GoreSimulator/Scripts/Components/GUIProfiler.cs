// ----------------------------------------------------
// Gore Simulator
// Copyright (c) Pampel Games e.K. All Rights Reserved.
// https://www.pampelgames.com
// ----------------------------------------------------

using System.Collections.Generic;
using PampelGames.Shared.Tools;
using UnityEngine;

namespace PampelGames.GoreSimulator
{
    /// <summary>
    ///     Draws runtime information about active <see cref="GoreSimulator"/>s to the GUI.
    /// </summary>
    public class GUIProfiler : MonoBehaviour
    {
        public Color color = Color.black;
        public Vector2 offset = Vector2.zero;
        public bool dontDestroyOnLoad = true;

        private int activeGoreSimulators;
        private int poolCountActive;
        private int poolCountInactive;
        private int particlePoolCountActive;
        private int particlePoolCountInactive;
        
        private int calculateCounter;
        private void Awake()  
        {  
            if(dontDestroyOnLoad) DontDestroyOnLoad(gameObject);
        }
        
        private void OnGUI()
        {
            
            var labelWidth = 175;
            var labelHeight = 20;

            var style = new GUIStyle(GUI.skin.label);
            style.normal.textColor = color;

            GUI.Label(new Rect(10 + offset.x, 10 + offset.y, labelWidth, labelHeight), activeGoreSimulators.ToString(), style);
            GUI.Label(new Rect(50 + offset.x, 10 + offset.y, labelWidth, labelHeight), "<b>Gore Simulators</b>", style);
            
            GUI.Label(new Rect(10 + offset.x, 30 + offset.y, labelWidth, labelHeight), poolCountActive.ToString(), style);
            GUI.Label(new Rect(50 + offset.x, 30 + offset.y, labelWidth, labelHeight), "Pool Count Active", style);
            
            GUI.Label(new Rect(10 + offset.x, 50 + offset.y, labelWidth, labelHeight), poolCountInactive.ToString(), style);
            GUI.Label(new Rect(50 + offset.x, 50 + offset.y, labelWidth, labelHeight), "Pool Count Inactive", style);

            GUI.Label(new Rect(10 + offset.x, 70 + offset.y, labelWidth, labelHeight), particlePoolCountActive.ToString(), style);
            GUI.Label(new Rect(50 + offset.x, 70 + offset.y, labelWidth, labelHeight), "Particle Pool Count Active", style);
            
            GUI.Label(new Rect(10 + offset.x, 90 + offset.y, labelWidth, labelHeight), particlePoolCountInactive.ToString(), style);
            GUI.Label(new Rect(50 + offset.x, 90 + offset.y, labelWidth, labelHeight), "Particle Pool Count Inactive", style);
            
            calculateCounter++;
            if (calculateCounter >= 60) CalculateNewLists();
        }
        
        
        private void CalculateNewLists()
        {
            calculateCounter = 0;
            
            var activeGS = GoreSimulatorAPI.GetActiveGoreSimulators();
            activeGoreSimulators = activeGS.Count;
            
            if (activeGS.Count > 0)
            {
                poolCountActive = PGPool.GetCountActive(activeGS[0]._defaultReferences.pooledMesh);
                poolCountInactive = PGPool.GetCountInactive(activeGS[0]._defaultReferences.pooledMesh);

                particlePoolCountActive = 0;
                particlePoolCountInactive = 0;
                HashSet<GameObject> particlePrefabs = new();
                for (int i = 0; i < activeGS.Count; i++)
                {
                    var particleCut = activeGS[i].GetSubModuleCut<SubModuleParticleEffects>(); 
                    if (particleCut != null)
                    {
                        foreach (var particleClass in particleCut.particleClasses)
                        {
                            if(particleClass.particle != null) particlePrefabs.Add(particleClass.particle.gameObject);
                        }
                    }
                    var particleExplosion = activeGS[i].GetSubModuleExplosion<SubModuleParticleEffects>(); 
                    if (particleExplosion != null)
                    {
                        foreach (var particleClass in particleExplosion.particleClasses)
                        {
                            if(particleClass.particle != null) particlePrefabs.Add(particleClass.particle.gameObject);
                        }
                    }
                }

                foreach (var particlePrefab in particlePrefabs)
                {
                    particlePoolCountActive += PGPool.GetCountActive(particlePrefab);
                    particlePoolCountInactive += PGPool.GetCountInactive(particlePrefab);
                }
                
            }

        }
    }
}