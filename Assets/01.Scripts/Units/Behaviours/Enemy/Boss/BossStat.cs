using System;
using Core;
using Units.Base.Unit;
using Units.Behaviours.Unit;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Units.Base.Enemy.Boss
{
    [Serializable]
    public class BossStat : UnitStat
    {
        public override void Damaged(float damage, UnitBase giveUnit)
        {
            base.Damaged(damage, giveUnit);
            EventParam param = new EventParam();
            param.floatParam = changeStats.Hp / originStats.Hp;
            if(Core.InGame.BossBase == this.ThisBase)
            {
                Core.Define.GetManager<EventManager>().TriggerEvent(EventFlag.AddBossHP, param);
				
                var particle = new MeshParticle.Particle();
                particle.randomProperties = new MeshParticle.ParticleRandomProperties()
                {
                    minPosition = new Vector3(-0.3f, -0.3f),
                    maxPosition = new Vector3(0.3f, 0.3f),
                    minRotation = 0,
                    maxRotation = 360,
                    minQuadSize = new Vector3(0.1f, 0.1f),
                    maxQuadSize = new Vector3(0.4f, 0.4f),
                };
                var pos = ThisBase.transform.position;
                particle.position = new Vector3(pos.x, pos.z) + particle.randomProperties.RandomPos;
                particle.rotation = particle.randomProperties.RandomRot;
                particle.quadSize = particle.randomProperties.RandomQuadSize;
                particle.skewed = true;
                particle.uvIndex = Random.Range(0, 4);
                InGame.MeshParticle.AddParticle("BossDamage", particle);

                //GameObject ob = Core.Define.GetManager<ResourceManagers>().Instantiate("ChargingPop");
                //ob.transform.position = ThisBase.transform.position;

            }
        }

        public override void Die()
        {
            base.Die();
            ThisBase.gameObject.SetActive(false);
        }
    }
}