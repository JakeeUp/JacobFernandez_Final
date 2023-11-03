// ----------------------------------------------------
// Gore Simulator
// Copyright (c) Pampel Games e.K. All Rights Reserved.
// https://www.pampelgames.com
// ----------------------------------------------------

using System;
using System.Collections.Generic;
using PampelGames.Shared.Tools;
using PampelGames.Shared.Utility;
using UnityEngine;
using UnityEngine.Pool;
using Object = UnityEngine.Object;

namespace PampelGames.GoreSimulator
{
    public class SubModuleParticleEffects : SubModuleBase
    {
        public override string ModuleName()
        {
            return "Particle Effects";
        }

        public override string ModuleInfo()
        {
            return "Particle spawner using an automated, shared pool.";
        }

        public override int imageIndex()
        {
            return 7;
        }

        public override void ModuleAdded(Type type)
        {
            base.ModuleAdded(type);
            addedType = type.Name;
            particleClasses ??= new List<ParticleWrapperClass>();
            particleClasses.Add(new ParticleWrapperClass());
#if UNITY_EDITOR
            if (type == typeof(GoreModuleCut))
                particleClasses[^1].particle = _goreSimulator._defaultReferences.cutParticle;
            else if (type == typeof(GoreModuleExplosion)) particleClasses[^1].particle = _goreSimulator._defaultReferences.explosionParticle;
#endif
        }

        public override bool CompatibleRagdoll()
        {
            return false;
        }

        /********************************************************************************************************************************/

        public List<ParticleWrapperClass> particleClasses = new();
        public string addedType;
        
        internal List<GameObject> activeSystems = new();


        [Serializable]
        public class ParticleWrapperClass
        {
            public ParticleSystem particle;
            public Enums.ParticlePositionExpl positionExpl = Enums.ParticlePositionExpl.Center;
            public int maxPosition = 5;
            public Enums.ParticleRotationExpl rotationExpl = Enums.ParticleRotationExpl.Method;
            public Enums.ParticleRotationCut rotationCut = Enums.ParticleRotationCut.CutDirection;
            public Enums.ParticleSetParent setParent = Enums.ParticleSetParent.Character;
        }

        /********************************************************************************************************************************/

        public override void Initialize()
        {
#if UNITY_EDITOR
            for (int i = 0; i < particleClasses.Count; i++)
            {
                if (particleClasses[i].particle == null)
                {
                    Debug.LogError("Particle System missing on Gore Simulator: " + _goreSimulator.gameObject.name);
                    return;
                }
            }
#endif
            
            base.Initialize();
            for (var i = 0; i < particleClasses.Count; i++)
                InitializePool(particleClasses[i].particle.gameObject, _goreSimulator._globalSettings.particlePreload,
                    _goreSimulator._globalSettings.particleLimited);
        }

        /********************************************************************************************************************************/

        public override void ExecuteModuleCut(SubModuleClass subModuleClass)
        {
            var position = subModuleClass.cutPosition;
            
            for (var i = 0; i < particleClasses.Count; i++)
            {
                var rotation = GetRotationCut(particleClasses[i], subModuleClass);

                if (particleClasses[i].rotationCut == Enums.ParticleRotationCut.CutDirection)
                {
                    if(subModuleClass.multiCut) continue;

                    if (particleClasses[i].setParent == Enums.ParticleSetParent.None)
                    {
                        var particle = PGPool.Get(particleClasses[i].particle.gameObject);
                        particle.transform.SetPositionAndRotation(position, rotation);
                    }
                    else if (particleClasses[i].setParent == Enums.ParticleSetParent.Both ||
                        particleClasses[i].setParent == Enums.ParticleSetParent.Character)
                    {
                        var particle = PGPool.Get(particleClasses[i].particle.gameObject);
                        particle.transform.SetPositionAndRotation(position, rotation);
                        particle.transform.SetParent(subModuleClass.centerBone);    
                    }
                    if (particleClasses[i].setParent == Enums.ParticleSetParent.Both ||
                        particleClasses[i].setParent == Enums.ParticleSetParent.DetachedPart)
                    {
                        Quaternion rotation2 = rotation;
                        if (subModuleClass.cutDirection != Vector3.zero) rotation2 = Quaternion.LookRotation(subModuleClass.cutDirection * -1);
                        var particle2 = PGPool.Get(particleClasses[i].particle.gameObject);
                        particle2.transform.SetPositionAndRotation(position, rotation2);
                    
                        particle2.transform.SetParent(subModuleClass.subRagdoll
                            ? ((SkinnedMeshRenderer) subModuleClass.subModuleObjectClasses[0].renderer).rootBone
                            : subModuleClass.parent.transform);
                    }
                }
                else
                {
                    var particle = PGPool.Get(particleClasses[i].particle.gameObject);
                    particle.transform.SetPositionAndRotation(position, rotation);
                }
            }
        }

        public override void ExecuteModuleExplosion(SubModuleClass subModuleClass)
        {
            for (var i = 0; i < particleClasses.Count; i++)
                if (particleClasses[i].positionExpl == Enums.ParticlePositionExpl.Center)
                {
                    var rotation = GetRotationExplosion(particleClasses[i], subModuleClass.subModuleObjectClasses[0]);
                    var particle = PGPool.Get(particleClasses[i].particle.gameObject);
                    var position = subModuleClass.centerPosition;
                    particle.transform.SetPositionAndRotation(position, rotation);
                }
                else if (particleClasses[i].positionExpl == Enums.ParticlePositionExpl.Method)
                {
                    var rotation = GetRotationExplosion(particleClasses[i], subModuleClass.subModuleObjectClasses[0]);
                    var particle = PGPool.Get(particleClasses[i].particle.gameObject);
                    var position = subModuleClass.position;
                    particle.transform.SetPositionAndRotation(position, rotation);
                }
                else
                {
                    int maxAmount = particleClasses[i].maxPosition;
                    int stepSize = Math.Max(1, subModuleClass.subModuleObjectClasses.Count / maxAmount);

                    for (int j = 0; j < maxAmount && j * stepSize < subModuleClass.subModuleObjectClasses.Count; j++)
                    {
                        var subModuleObjClass = subModuleClass.subModuleObjectClasses[j * stepSize];
                        var rotation = GetRotationExplosion(particleClasses[i], subModuleObjClass);

                        var particle = PGPool.Get(particleClasses[i].particle.gameObject);
                        var position = subModuleObjClass.centerPosition;
                        particle.transform.SetPositionAndRotation(position, rotation);
                    }
                }
        }

        public override void ExecuteModuleRagdoll(List<GoreBone> goreBones)
        {
        }

        public override void Reset()
        {
            base.Reset();
            for (var i = activeSystems.Count - 1; i >= 0; i--)
            {
                PGPool.Release(activeSystems[i]);
            }
            activeSystems.Clear();
        }

        /********************************************************************************************************************************/

        private Quaternion GetRotationCut(ParticleWrapperClass particleClass, SubModuleClass subModuleClass)
        {
            if (particleClass.rotationCut == Enums.ParticleRotationCut.CutDirection)
            {
                if (subModuleClass.cutDirection == Vector3.zero) return Quaternion.identity; 
                return Quaternion.LookRotation(subModuleClass.cutDirection);
            }
            if (particleClass.rotationCut == Enums.ParticleRotationCut.Method)
            {
                if (subModuleClass.force == Vector3.zero) return Quaternion.identity; 
                return Quaternion.LookRotation(subModuleClass.force);
            }           
            if (particleClass.rotationCut == Enums.ParticleRotationCut.Default) return Quaternion.identity;
            return PGMathUtility.GetRandomRotation();
        }
        
        private Quaternion GetRotationExplosion(ParticleWrapperClass particleClass, SubModuleObjectClass subModuleObjectClass)
        {
            if (particleClass.rotationExpl == Enums.ParticleRotationExpl.Method)
            {
                if (subModuleObjectClass.force == Vector3.zero) return Quaternion.identity; 
                return Quaternion.LookRotation(subModuleObjectClass.force);
            }
            if (particleClass.rotationExpl == Enums.ParticleRotationExpl.Default) return Quaternion.identity;
            return PGMathUtility.GetRandomRotation();
        }


        /********************************************************************************************************************************/
        // Pool

        internal GameObject[] InitializePool(GameObject prefab, int preloadAmount, bool limited)
        {
            var pool = PGPool.TryGetExistingPool(prefab) ?? new ObjectPool<GameObject>(
                () => CreateSetup(prefab),
                GetSetup,
                ReleaseSetup,
                DestroySetup,
                true,
                preloadAmount);
            return PGPool.Preload(prefab, pool, preloadAmount, limited);
        }

        private GameObject CreateSetup(GameObject prefab)
        {
            var obj = Object.Instantiate(prefab);
            return obj;
        }

        private void GetSetup(GameObject obj)
        {
#if UNITY_EDITOR
            if (SO_GlobalSettings.Instance.hidePooledObjects) obj.hideFlags = HideFlags.None;
#endif
            if (!obj.TryGetComponent<PGPoolableParticles>(out var pgPoolableParticles))
            {
                pgPoolableParticles = obj.AddComponent<PGPoolableParticles>();
                if (pgPoolableParticles.TryGetComponent<ParticleSystem>(out var particleSystem))
                    pgPoolableParticles._particleSystem = particleSystem;
            }

            obj.SetActive(true);
            activeSystems.Add(obj);
            pgPoolableParticles._particleSystem.Play();
        }

        private void ReleaseSetup(GameObject obj)
        {
#if UNITY_EDITOR
            if (SO_GlobalSettings.Instance.hidePooledObjects) obj.hideFlags = HideFlags.HideInHierarchy;
#endif
            obj.transform.SetParent(null);
            if (activeSystems.Contains(obj)) activeSystems.Remove(obj);
            obj.SetActive(false);
        }

        private void DestroySetup(GameObject obj)
        {
            if (activeSystems.Contains(obj)) activeSystems.Remove(obj);
            Object.Destroy(obj);
        }
    }
}