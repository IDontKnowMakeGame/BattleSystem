using System;
using System.Runtime.CompilerServices;
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
        private Actor attackActor;

        public override void Awake()
        {
            base.Awake();
		}

        public override void Damage(float damage, Actor actor)
        {
            attackActor = actor;
            base.Damage(damage, actor);

            GameObject obj = Define.GetManager<ResourceManager>().Instantiate("Damage");
            obj.GetComponent<DamagePopUp>().DamageText((int)damage, ThisActor.transform.position);


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

        public override void Die()
        {
            if (!ThisActor.gameObject.activeSelf)
                return;

            ThisActor.GetAct<EnemyAI>()?.ResetAllConditions();

			GameObject obj = Define.GetManager<ResourceManager>().Instantiate("DieObject");
            obj.GetComponent<DieAction>().InitDieObj(ThisActor.gameObject.name);
            obj.transform.position = ThisActor.Position + Vector3.up;

            //QuestManager.Instance.CheckKillMission((ThisActor as EnemyActor).CurrentType);
            GameObject addObject = Define.GetManager<ResourceManager>().Instantiate("EatEffect");
            //if(attackActor != null)
            addObject.transform.position = ThisActor.Position + Vector3.up;
			addObject.GetComponent<EatEffect>().Init(attackActor.gameObject);
            QuestManager.Instance.CheckKillMission((ThisActor as EnemyActor).CurrentType);

			var enemy = ThisActor as EnemyActor;
            if (enemy != null) enemy.Alive = false;
			ThisActor.gameObject.SetActive(false);
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