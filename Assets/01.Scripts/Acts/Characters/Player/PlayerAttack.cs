using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Managements.Managers;
using Core;
using Actors.Characters.Player;

namespace Acts.Characters.Player
{
    public class PlayerAttack : CharacterAttack
    {
        private PlayerAnimation _playerAnimation;
        private PlayerActor _playerActor;

        private AttackCollider attackCol;

        private List<SampleControoler> enemys = new List<SampleControoler>();


        AttackInfo attackInfo = new AttackInfo();
        AttackInfo attackInfo2 = new AttackInfo();

        private AttackInfo temp;

        public override void Awake()
        {
            InputManager<GreatSword>.OnAttackPress += ReadyAttackAnimation;
            base.Awake();
        }

        public override void Start()
        {
            base.Start();
            _playerAnimation = ThisActor.GetAct<PlayerAnimation>();
            _playerActor = InGame.Player.GetComponent<PlayerActor>();

            attackCol = ThisActor.GetComponentInChildren<AttackCollider>();

            // Test Code
            //attackInfo.SizeZ = 1;

            attackInfo.ReachFrame = 5;
            attackInfo.SizeZ = 1;

            attackInfo2.ReachFrame = 5;
            attackInfo2.SizeZ = 2;

            temp = attackInfo;

            attackCol.SetAttackCol(ref temp);
        }

        public override void Update()
        {
            base.Update();

            if (Input.GetKeyDown(KeyCode.Alpha1))
            {
                temp = attackInfo;
                attackCol.SetAttackCol(ref temp);
            }
            if (Input.GetKeyDown(KeyCode.Alpha2))
            {
                temp = attackInfo2;
                attackCol.SetAttackCol(ref temp);
            }
        }

        public override void AttackCheck(AttackInfo attackInfo)
        {
            enemys.Clear();

            if(attackCol.CurrntDirNearEnemy() != null)
                enemys.Add(attackCol.CurrntDirNearEnemy());

            if(enemys.Count > 0)
                _playerAnimation.curClip.SetEventOnFrame(attackInfo.ReachFrame, Attack);

            attackCol.AllReset();
        }

        private void Attack()
        {
            foreach (SampleControoler enemy in enemys)
            {
                Debug.Log(enemy.name);
            }

        }

        public override void ReadyAttackAnimation(Vector3 dir)
        {
            if (_playerActor.IsPlaying) return;

            _playerActor.IsPlaying = true;

            temp.ResetDir();

            if (dir == Vector3.left)
            {
                ThisActor.SpriteTransform.localScale = new Vector3(-1, 1, 1);
                temp.AddDir(DirType.Left);
                _playerAnimation.Play("VerticalAttack");
            }
            else if (dir == Vector3.right)
            {
                ThisActor.SpriteTransform.localScale = new Vector3(1, 1, 1);
                temp.AddDir(DirType.Right);
                _playerAnimation.Play("VerticalAttack");
            }
            else if (dir == Vector3.forward)
            {
                temp.AddDir(DirType.Up);
                _playerAnimation.Play("UpperAttack");
            }
            else if (dir == Vector3.back)
            {
                temp.AddDir(DirType.Down);
                _playerAnimation.Play("LowerAttack");
            }

            AttackCheck(temp);

            _playerAnimation.curClip.SetEventOnFrame(_playerAnimation.curClip.fps - 1 , () => _playerActor.IsPlaying = false);
        }
    }
}
