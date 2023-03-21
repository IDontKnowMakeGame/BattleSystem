using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Managements.Managers;
using Core;
using Actors.Characters.Player;
using Actors.Characters;

namespace Acts.Characters.Player
{
    public class PlayerAttack : CharacterAttack
    {
        private PlayerAnimation _playerAnimation;
        private PlayerActor _playerActor;

        private AttackCollider attackCol;

        private List<SampleControoler> enemys = new List<SampleControoler>();

        public override void Awake()
        {
            base.Awake();
        }

        public override void Start()
        {
            base.Start();
            Define.GetManager<EventManager>().StartListening(EventFlag.Attack, Attack);
            _playerAnimation = ThisActor.GetAct<PlayerAnimation>();
            _playerActor = InGame.Player.GetComponent<PlayerActor>();

            attackCol = ThisActor.GetComponentInChildren<AttackCollider>();

            // Test Code
            //attackInfo.SizeZ = 1;
        }

        public override void Update()
        {
            base.Update();
        }

        public override void AttackCheck(AttackInfo attackInfo)
        {
            enemys.Clear();

            attackCol.SetAttackCol(attackInfo);

            if(attackCol.CurrntDirNearEnemy() != null)
                enemys.Add(attackCol.CurrntDirNearEnemy());

            if(enemys.Count > 0)
            {
                _playerAnimation.curClip.SetEventOnFrame(attackInfo.ReachFrame, Attack);
            }

            attackCol.AllReset();
        }

        private void Attack()
        {
            foreach (SampleControoler enemy in enemys)
            {
                Debug.Log(enemy.name);
                GameObject obj = Define.GetManager<ResourceManager>().Instantiate("Damage");
                obj.GetComponent<DamagePopUp>().DamageText(5, enemy.transform.position);
            }
        }
        
        public override void ReadyAttackAnimation(AttackInfo attackInfo)
        {
            if (_playerActor.HasState(CharacterState.Everything & ~CharacterState.Hold)) return;
            _playerActor.AddState(CharacterState.Attack);

            if (attackInfo.PressInput == Vector3.left)
            {
                ThisActor.SpriteTransform.localScale = new Vector3(-1, 1, 1);
                _playerAnimation.Play("VerticalAttack");
            }
            else if (attackInfo.PressInput == Vector3.right)
            {
                ThisActor.SpriteTransform.localScale = new Vector3(1, 1, 1);
                _playerAnimation.Play("VerticalAttack");
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
            _playerAnimation.curClip.SetEventOnFrame(_playerAnimation.curClip.fps - 1 , () => _playerActor.RemoveState(Actors.Characters.CharacterState.Attack));
        }

        private void Attack(EventParam eventParam)
        {
            ReadyAttackAnimation(eventParam.attackParam);
		}

        public override void OnDisable()
        {
			Define.GetManager<EventManager>()?.StopListening(EventFlag.Attack, Attack);
		}
    }
}
