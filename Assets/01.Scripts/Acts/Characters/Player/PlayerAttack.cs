using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Managements.Managers;
using Core;
using Actors.Characters.Player;
using Actors.Characters;
using Actors.Characters.Enemy;

namespace Acts.Characters.Player
{
    [System.Serializable]
    public class PlayerAttack : CharacterAttack
    {
        private PlayerAnimation _playerAnimation;
        private PlayerActor _playerActor;
        private PlayerEquipment _playerEquipment;

        private AttackCollider attackCol;

        private List<EnemyActor> enemys = new List<EnemyActor>();

        private Vector3 currentDir;
        private float degree;
        private Vector3 offset;
        private CharacterActor ThisCharacter => ThisActor as CharacterActor;

        private CharacterState _tempState;
        private CCInfo _ccInfo;

        public override void Awake()
        {
            base.Awake();

            OnAttackEnd = null;
        }

        public override void OnEnable()
        {
            base.OnEnable();
			Define.GetManager<EventManager>().StartListening(EventFlag.Attack, Attack);
			Define.GetManager<EventManager>().StartListening(EventFlag.NoneAniAttack, NoneAniAttack);
			Define.GetManager<EventManager>().StartListening(EventFlag.FureAttack, FureAttack);
		}

        public override void Start()
        {
            base.Start();
            _playerAnimation = ThisActor.GetAct<PlayerAnimation>();
            _playerActor = InGame.Player.GetComponent<PlayerActor>();
            _playerEquipment = _playerActor.GetAct<PlayerEquipment>();

            attackCol = ThisActor.GetComponentInChildren<AttackCollider>();

            // Test Code
            //attackInfo.SizeZ = 1;
        }

        public override void Update()
        {
            base.Update();
            ColParentRotate();
        }

        public override void AttackCheck(AttackInfo attackInfo)
        {
            enemys.Clear();

            attackCol.SetAttackCol(attackInfo);

            if(attackCol.CurrntDirNearEnemy() != null)
            {
                //attackCol.CurrntDirNearEnemy
                enemys.Add(attackCol.CurrntDirNearEnemy());
            }

            if(enemys.Count > 0)
            {
                _tempState = attackInfo.State;
                _ccInfo = attackInfo.CCInfo;
				ParticleRot(attackInfo.PressInput);
                _playerAnimation.curClip.SetEventOnFrame(attackInfo.ReachFrame, Attack);
            }

			OnAttackEnd?.Invoke(_playerActor.UUID);
			attackCol.AllReset();
        }

        private void Attack()
        {
            var character = ThisActor.GetAct<CharacterStatAct>();

			foreach (EnemyActor enemy in enemys)
            {
                enemy.GetAct<CharacterStatAct>().Damage(character.ChangeStat.atk, ThisActor);

				if (enemy.Alive)
                {
                    if (_tempState.HasFlag(CharacterState.KnockBack))
                    {
						enemy.OnKnockBack.Invoke(_ccInfo.knockRange, ThisActor);
                        _tempState = CharacterState.None;
                    }

					GameObject spark = Define.GetManager<ResourceManager>().Instantiate("Spark");

                    Vector3 particleRot = spark.transform.localEulerAngles;
                    particleRot.y = degree;

                    spark.transform.localPosition = enemy.transform.position.SetY(0.7f) + offset;
                    spark.transform.localRotation = Quaternion.Euler(particleRot);
                    spark.GetComponent<ParticleSystem>().Play();
                }
            }
            _playerActor.GetAct<PlayerBuff>().ChangeAdneraline(1);
		}

        private void ParticleRot(Vector3 pressInput)
        {
            if (pressInput == Vector3.forward)
            {
                degree = 180;
                offset = new Vector3(0, 0, 0.3f);
            }
            else if (pressInput == Vector3.back)
            {
                degree = 0;
                offset = new Vector3(0, 0, -0.3f);
            }
            else if (pressInput == Vector3.left)
            {
                degree = 90;
                offset = new Vector3(-0.3f, 0, 0);
            }
            else if (pressInput == Vector3.right)
            {
                degree = -90;
                offset = new Vector3(0.3f, 0, 0);
            }
        }
        
        public override void ReadyAttackAnimation(AttackInfo attackInfo)
        {
            if (_playerActor.HasState(CharacterState.Everything & ~CharacterState.Hold)) return;
            _playerActor.AddState(CharacterState.Attack);

            if (attackInfo.PressInput == Vector3.left)
            {
                ThisActor.SpriteTransform.localScale =  new Vector3(-2, 1, 1);
                _playerAnimation.Play("HorizontalAttack");
            }
            else if (attackInfo.PressInput == Vector3.right)
            {
                ThisActor.SpriteTransform.localScale = new Vector3(2, 1, 1);
                _playerAnimation.Play("HorizontalAttack");
            }
            else if (attackInfo.PressInput == Vector3.forward)
            {
                _playerAnimation.Play("UpperAttack");
            }
            else if (attackInfo.PressInput == Vector3.back)
            {
                _playerAnimation.Play("LowerAttack");
            }

            AttackCheck(attackInfo);

            // 마지막 프레임에 종료 넣기
            _playerAnimation.curClip.SetEventOnFrame(_playerAnimation.curClip.fps - 1 , FinishAttack);
        }

        private void FinishAttack()
        {
            _playerActor.RemoveState(Actors.Characters.CharacterState.Attack);
            _playerAnimation.curClip.events.Clear();
        }

        private void Attack(EventParam eventParam)
        {
            if (ThisCharacter.HasCCState()) return;
            ReadyAttackAnimation(eventParam.attackParam);
		}

        private void NoneAniAttack(EventParam eventParam)
        {
            if (ThisCharacter.HasCCState()) return;
            if(eventParam.boolParam == false)
            {
				enemys.Clear();
				attackCol.SetAttackCol(eventParam.attackParam);
				_playerActor.AddState(CharacterState.Attack);
			}
            else
            {
				if (attackCol.CurrntDirNearEnemy() != null)
					enemys.Add(attackCol.CurrntDirNearEnemy());

				if (enemys.Count > 0)
				{
					Attack();
				}
				OnAttackEnd?.Invoke(_playerActor.UUID);
				attackCol.AllReset();
				_playerActor.RemoveState(CharacterState.Attack);
			}
		}

        private void FureAttack(EventParam eventParam)
        {
            if (ThisCharacter.HasCCState()) return;
            enemys.Clear();

            attackCol.SetAttackCol(eventParam.attackParam);
			if (attackCol.CurrntDirNearEnemy() != null)
				enemys.Add(attackCol.CurrntDirNearEnemy());

            if (enemys.Count > 0)
            {
                Attack();
            }
			OnAttackEnd?.Invoke(_playerActor.UUID);
			attackCol.AllReset();
		}
        private void ColParentRotate()
        {
            Vector3 cameraDir = InGame.CameraDir();

            if (currentDir == cameraDir) return;

            currentDir = cameraDir;

            if(cameraDir == Vector3.right)
            {
                attackCol.transform.rotation = Quaternion.Euler(0, 90, 0);
            }
            else if(cameraDir == Vector3.left)
            {
                attackCol.transform.rotation = Quaternion.Euler(0, -90, 0);
            }
            else if(cameraDir == Vector3.forward)
            {
                attackCol.transform.rotation = Quaternion.Euler(0, 0, 0);
            }
            else if (cameraDir == Vector3.back)
            {
                attackCol.transform.rotation = Quaternion.Euler(0, 180, 0);
            }
        }

        public override void OnDisable()
        {
			Define.GetManager<EventManager>()?.StopListening(EventFlag.Attack, Attack);
            Define.GetManager<EventManager>()?.StopListening(EventFlag.NoneAniAttack, NoneAniAttack);
			Define.GetManager<EventManager>()?.StartListening(EventFlag.FureAttack, FureAttack);
		}

        public void RangeReset()
        {
            attackCol.AllEnemysClear();
        }
    }
}
