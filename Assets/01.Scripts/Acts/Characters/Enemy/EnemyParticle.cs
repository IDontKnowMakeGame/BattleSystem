using System;
using Acts.Base;
using UnityEngine;

namespace Acts.Characters.Enemy
{
    [Serializable]
    public class EnemyParticle : Act
    {
        [SerializeField] private ParticleSystem landingParticle;
        [SerializeField] private ParticleSystem secondPhaseParticle;

        public void PlayLandingParticle()
        {
            landingParticle.Play();
        }

        public void PlaySecondPhaseParticle()
        {
            secondPhaseParticle.Play();
        }
    }
}