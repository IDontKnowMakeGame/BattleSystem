using Acts.Base;
using UnityEngine;

namespace Blocks.Acts
{
    public class BlockParticle : Act
    {
        private ParticleSystem _explosionParticleSystem;
        private ParticleSystem _smokeParticleSystem;
        public override void Awake()
        {
            base.Awake();
            _explosionParticleSystem = CreateParticle("BlockExplosion");
            _smokeParticleSystem = CreateParticle("BlockSmoke");
        }
        
        public void PlayExplosionParticle()
        {
            _explosionParticleSystem.Play();
        }

        public void PlaySmokeParticle()
        {
            _smokeParticleSystem.Play();
        }
        
        public ParticleSystem CreateParticle(string name)
        {
            var particleTrm = Resources.Load<Transform>("Prefabs/" + name);
            var particle = GameObject.Instantiate(particleTrm, ThisActor.transform);
            return particle.GetComponent<ParticleSystem>();
        }
    }
}