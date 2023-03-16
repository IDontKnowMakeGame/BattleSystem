using Acts.Characters;
using Managements.Managers;
using UnityEngine;

namespace Acts.Characters.Player
{
    public class PlayerMove : CharacterMove
    {
        private PlayerAnimation _playerAnimation;
        private Transform spriteTransform;

        public override void Awake()
        {         
            base.Awake();
            InputManager.OnMovePress += Translate;
        }

        public override void Start()
        {
            base.Start();

            spriteTransform = ThisActor.GetComponentInChildren<MeshRenderer>().transform;

            _playerAnimation = ThisActor.GetAct<PlayerAnimation>();
        }

        /// <summary>
        /// Player Animation Setting
        /// </summary>
        protected override void MoveAnimation(Vector3 dir)
        {
            if (_isMoving) return;

            if(dir == Vector3.left)
            {
                spriteTransform.localScale = new Vector3(-1, 1, 1);
                _playerAnimation.Play("VerticalMove");
            }
            else if(dir == Vector3.right)
            {
                spriteTransform.localScale = new Vector3(1, 1, 1);
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
        }
    }
}