// ----------------------------------------------------
// Copyright (c) Pampel Games e.K. All Rights Reserved.
// https://www.pampelgames.com
// ----------------------------------------------------

using UnityEngine;

namespace PampelGames.Shared.Tools
{
    /// <summary>
    ///     Can be attached to pooled particle systems to despawn them automatically when all particles have died.
    /// </summary>
    public class PGPoolableParticles : MonoBehaviour
    {
        public ParticleSystem _particleSystem;

        private void Start()
        {
            var main = _particleSystem.main;
            main.stopAction = ParticleSystemStopAction.Callback;
        }

        private void OnParticleSystemStopped()
        {
            PGPool.Release(gameObject);
        }
    }
}