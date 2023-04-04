using System;
using Actors.Bases;
using Actors.Characters;
using Actors.Characters.Enemy;
using Core;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Acts.Characters.Enemy
{
    [Serializable]
    public class EnemyStatAct : CharacterStatAct
    {
        public override void Damage(float damage, Actor actor)
        {
            base.Damage(damage, actor);

            GameObject obj = Define.GetManager<ResourceManager>().Instantiate("Damage");
            obj.GetComponent<DamagePopUp>().DamageText(damage, ThisActor.transform.position);


			if (ThisActor is BossActor)
            {
                UIManager.Instance.BossBar.ChangeBossBarValue(PercentHP());
                EventParam eventParam = new EventParam();
                eventParam.intParam = 1;
                eventParam.stringParam = (actor as CharacterActor).currentWeapon.info.Class;
                Define.GetManager<EventManager>().TriggerEvent(EventFlag.PlayTimeLine, eventParam);
            }
            DamageEffect();
        }

        private void DamageEffect()
        {
            var particle = new MeshParticle.Particle
            {
                randomProperties = new MeshParticle.ParticleRandomProperties()
                {
                    minPosition = new Vector3(-0.3f, -0.3f),
                    maxPosition = new Vector3(0.3f, 0.3f),
                    minRotation = 0,
                    maxRotation = 360,
                    minQuadSize = new Vector3(1f, 1f),
                    maxQuadSize = new Vector3(1f, 1f),
                }
            };
            var pos = ThisActor.transform.position;
            particle.position = new Vector3(pos.x, pos.z) + particle.randomProperties.RandomPos;
            particle.rotation = particle.randomProperties.RandomRot;
            particle.quadSize = particle.randomProperties.RandomQuadSize;
            particle.skewed = true;
            particle.uvIndex = Random.Range(0, 4);
            MeshParticle.Instance.AddParticle("Blood", particle);
        }
    }
}