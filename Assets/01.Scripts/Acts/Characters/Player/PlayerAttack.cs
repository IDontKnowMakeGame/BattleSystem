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

        public override void Awake()
        {
            InputManager.OnAttackPress += ReadyAttackAnimation;

            base.Awake();
        }

        public override void Start()
        {
            base.Start();
            _playerAnimation = ThisActor.GetAct<PlayerAnimation>();
            _playerActor = InGame.Player.GetComponent<PlayerActor>();
        }

        public override void ReadyAttackAnimation(Vector3 dir)
        {
            if (_playerActor.IsPlaying) return;

            _playerActor.IsPlaying = true;

            if (dir == Vector3.left)
            {
                ThisActor.SpriteTransform.localScale = new Vector3(-1, 1, 1);
                _playerAnimation.Play("VerticalAttack");
            }
            else if (dir == Vector3.right)
            {
                ThisActor.SpriteTransform.localScale = new Vector3(1, 1, 1);
                _playerAnimation.Play("VerticalAttack");
            }
            else if (dir == Vector3.forward)
            {
                _playerAnimation.Play("UpperAttack");
            }
            else if (dir == Vector3.back)
            {
                _playerAnimation.Play("LowerAttack");
            }

            _playerAnimation.curClip.SetEventOnFrame(_playerAnimation.curClip.fps - 1 , () => _playerActor.IsPlaying = false);
        }
    }
}
