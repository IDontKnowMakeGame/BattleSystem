using Acts.Base;
using UnityEngine;

namespace Blocks.Acts
{
    public class BlockParticle : Act
    {
        private Transform _particleTransform; 
        private ParticleSystem _particleSystem;
        public override void Awake()
        {
            base.Awake();
            _particleTransform = Resources.Load<Transform>("Prefabs/BlockExplosion");
            var particle = GameObject.Instantiate(_particleTransform, ThisActor.transform);
            _particleSystem = particle.GetComponent<ParticleSystem>();
        }
        
        public void Play()
        {
            _particleSystem.Play();
        }
    }
}