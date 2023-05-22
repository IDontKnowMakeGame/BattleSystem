using System;
using System.Runtime.CompilerServices;
using Actors.Bases;
using Actors.Characters;
using Actors.Characters.Enemy;
using Blocks;
using Core;
using Tools;
using UnityEngine;
using Acts.Characters.Player;
using DG.Tweening;
using Random = UnityEngine.Random;
using Data;
using Actors.Characters.Player;

namespace Acts.Characters.Enemy
{
    [Serializable]
    public class EnemyStatAct : CharacterStatAct
    {
        [SerializeField]
        private int _feahter;
        [SerializeField]
        private GetItemInfo info;

		private Actor attackActor;

        public bool isBoss = false;
        public override void Awake()
        {
            base.Awake();
		}

        public override void Damage(float damage, Actor actor)
        {
            attackActor = actor;
            base.Damage(damage, actor);
            if (actor is EmptyBlock)
            {
                Die();
                return;
            }

			GameObject obj = Define.GetManager<ResourceManager>().Instantiate("DamageText");
			obj.GetComponent<DamageText>().PopUp((int)damage, ThisActor.transform.position, (actor.transform.position - ThisActor.transform.position).GetDirection());

			if (ThisActor is BossActor)
            {
                UIManager.Instance.BossBar.ChangeBossBarValue(PercentHP());
			}

            if(actor is PlayerActor)
            {
                Debug.Log(actor.gameObject.name);
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
            ThisActor.gameObject.tag = "Untagged";

            if (attackActor != null)
            QuestManager.Instance.CheckKillMission((ThisActor as EnemyActor).CurrentType);

            GameObject addObject = Define.GetManager<ResourceManager>().Instantiate("EatEffect");
            addObject.transform.position = ThisActor.Position + Vector3.up;
			addObject.GetComponent<EatEffect>().Init(attackActor.gameObject);

            if(isBoss)
            {
				UnitAnimation unit = ThisActor.GetAct<UnitAnimation>();
				ClipBase clip = unit.GetClip("Die");
				clip?.SetEventOnFrame(clip.fps - 1, ObjectCreate);
				unit?.Play("Die");

                UIManager.Instance.BossBar.HideBossBar();
			}

            Define.GetManager<DataManager>().AddFeahter(_feahter);
            UIManager.Instance?.InGame.AddFeatherValue(_feahter);


            var enemy = ThisActor as EnemyActor;
            if (enemy != null) enemy.Alive = false;

		    ThisActor.RemoveAct<EnemyAI>();

            Debug.Log(isBoss);

            if (!isBoss)
            {
	            UnitAnimation unit = ThisActor.GetAct<UnitAnimation>();
	            CharacterRender render = ThisActor.GetAct<CharacterRender>();
	            var mat = render.Renderer.material;
	            ClipBase clip = unit.GetClip("Die");
				Arrow arrow = ThisActor.GetComponentInChildren<Arrow>();
				if (arrow != null)
				{
					arrow.StickReBlock();
					arrow.transform.parent = null;
				}
				clip.OnExit = () =>
	            {
					Arrow arrow = ThisActor.GetComponentInChildren<Arrow>();
					if (arrow != null)
					{
						arrow.StickReBlock();
						arrow.transform.parent = null;
					}
					ThisActor.gameObject.SetActive(false);
                };
	            var deathParticle = Define.GetManager<ResourceManager>().Instantiate("DeathParticle");
	            //DOTween.To(() => 4, x => mat.SetFloat("_Cutoff_Height", x), 0, 0.8f);
	            deathParticle.transform.position = ThisActor.Position;
	            unit.Play("Die");

                Debug.Log("Die");
                Debug.Log(unit);
            }

            if(info._id != ItemID.None)
            {
                GameObject obj = Define.GetManager<ResourceManager>().Instantiate("GetItemObject");
                obj.GetComponent<GetItemObject>().Init(info);
                obj.transform.position = this.ThisActor.Position + Vector3.up/2;
			}

            InGame.Player.GetAct<PlayerAttack>().RangeReset();
            InGame.Player.GetAct<PlayerAttack>().DeleteEnemy(ThisActor.gameObject);
		}

        private void ObjectCreate()
        {
			GameObject obj = Define.GetManager<ResourceManager>().Instantiate("DieObject");
			obj.GetComponent<DieAction>().InitDieObj(ThisActor.gameObject.name);
            obj.transform.gameObject.transform.GetChild(0).GetChild(0).localScale = ThisActor.gameObject.transform.GetChild(0).GetChild(0).localScale;
            obj.transform.gameObject.transform.GetChild(0).GetChild(0).localPosition = Vector3.up;
			obj.transform.position = ThisActor.Position + Vector3.up;

            InGame.Player.GetAct<PlayerAttack>().RangeReset();
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
            particle.uvIndex = Random.Range(0, 8);
            MeshParticle.Instance.AddParticle("Blood", particle);
        }
    }
}