using System;
using System.Numerics;
using Acts.Base;
using Core;
using UnityEngine;
using Quaternion = UnityEngine.Quaternion;
using Vector3 = UnityEngine.Vector3;

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
        
        public void PlayTaintedParticle(Vector3 dir)
        {
            var resource = Define.GetManager<ResourceManager>();
            var particleObj = resource.Instantiate("TaintedEffect", ThisActor.transform);
            particleObj.transform.localPosition = dir;
            particleObj.transform.rotation = Quaternion.Euler(-90f, 90f - dir.ToDegree(), 0);
            resource.Destroy(particleObj, 1f);
        }
    }
}