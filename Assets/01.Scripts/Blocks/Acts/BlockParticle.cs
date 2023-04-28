using System.Collections;
using Acts.Base;
using Core;
using UnityEngine;

namespace Blocks.Acts
{
    public class BlockParticle : Act
    {
        private GameObject _explosionParticleObject;
        private GameObject _smokeParticleObject;
        PoolManager _poolManager;
        public override void Awake()
        {
            _explosionParticleObject = Resources.Load<GameObject>("Prefabs/BlockExplosion");
            _smokeParticleObject = Resources.Load<GameObject>("Prefabs/BlockSmoke");
            _poolManager = Define.GetManager<PoolManager>();
            base.Awake();
        }
        
        public void PlayExplosionParticle()
        {
            ThisActor.StartCoroutine(PlayParticleCoroutine(_explosionParticleObject));
        }

        public void PlaySmokeParticle()
        {
            ThisActor.StartCoroutine(PlayParticleCoroutine(_smokeParticleObject));
        }
        
        public IEnumerator PlayParticleCoroutine(GameObject obj)
        {
            var particleSystem = CreateParticle(obj, out var poolable);
            particleSystem.Play();
            yield return new WaitForSeconds(particleSystem.main.duration);
            particleSystem.Stop();
            _poolManager.Push(poolable);
        }
        
        public ParticleSystem CreateParticle(GameObject obj, out Poolable poolable)
        {
             poolable =_poolManager.Pop(obj);
            var particleSystem = poolable.GetComponent<ParticleSystem>();
            particleSystem.transform.position = ThisActor.Position + Vector3.up * 0.5f;
            particleSystem.transform.rotation = Quaternion.identity;
            return particleSystem;
        }
    }
}