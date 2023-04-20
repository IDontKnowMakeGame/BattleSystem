using System;
using Acts.Base;
using UnityEngine;

namespace Acts.Characters.Enemy
{
    [Serializable]
    public class EnemyParticle : Act
    {
        [SerializeField] private ParticleSystem landingParticle;
        
        public void PlayLandingParticle()
        {
            landingParticle.Play();
        }
    }
}