using Acts.Characters;
using Managements.Managers;
using UnityEngine;
using Core;
using Actors.Characters.Player;

namespace Acts.Characters.Player
{
    public class PlayerMove : CharacterMove
    {
        private PlayerAnimation _playerAnimation;
        private PlayerActor _playerActor;

        public override void Awake()
        {         
            base.Awake();
            InputManager<GreatSword>.OnMovePress += Translate;
        }

        public override void Start()
        {
            base.Start();
            _playerAnimation = ThisActor.GetAct<PlayerAnimation>();
            _playerActor = InGame.Player.GetComponent<PlayerActor>();
        }

        protected override void Move(Vector3 position)
        {
            if (_playerActor.HasAnyState()) return;
            _playerActor.AddState(Actors.Characters.CharacterState.Move);
            base.Move(position);
        }

        /// <summary>
        /// Player Animation Setting
        /// </summary>
        protected override void MoveAnimation(Vector3 dir)
        {
            if (dir == Vector3.left)
            {
                ThisActor.SpriteTransform.localScale = new Vector3(-1, 1, 1);
                _playerAnimation.Play("VerticalMove");
            }
            else if(dir == Vector3.right)
            {
                ThisActor.SpriteTransform.localScale = new Vector3(1, 1, 1);
                _playerAnimation.Play("VerticalMove");
            }
            else if(dir == Vector3.forward)
            {
                _playerAnimation.Play("UpperMove");
            }
            else if (dir == Vector3.back)
            {
                _playerAnimation.Play("LowerMove");
            }
        }

        protected override void MoveStop()
        {
            _playerAnimation.Play("Idle");
            _playerActor.RemoveState(Actors.Characters.CharacterState.Move);
        }
    }
}