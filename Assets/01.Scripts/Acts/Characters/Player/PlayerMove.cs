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

        private Vector3 cameraDir;
        private Vector3 playerDir;

        public float distance = 1;
        public override void Awake()
        {         
            base.Awake();
            InputManager<Weapon>.OnMovePress += Translate;
        }

        public override void Start()
        {
            base.Start();
            _playerAnimation = ThisActor.GetAct<PlayerAnimation>();
            _playerActor = InGame.Player.GetComponent<PlayerActor>();
        }

        public override void Update()
        {
            base.Update();
            cameraDir = InGame.CameraDir();
        }

        public override void Translate(Vector3 direction)
        {
            if (_playerActor.HasAnyState()) return;
            playerDir = direction;
            direction = CamDirCheck(direction);
            base.Translate(direction* distance);
        }

        public override void Move(Vector3 position)
        {
            base.Move(position);
        }

        /// <summary>
        /// Player Animation Setting
        /// </summary>
        protected override void MoveAnimation()
        {
            if (playerDir == Vector3.left)
            {
                ThisActor.SpriteTransform.localScale = new Vector3(-1, 1, 1);
                _playerAnimation.Play("VerticalMove");
            }
            else if(playerDir == Vector3.right)
            {
                ThisActor.SpriteTransform.localScale = new Vector3(1, 1, 1);
                _playerAnimation.Play("VerticalMove");
            }
            else if(playerDir == Vector3.forward)
            {
                _playerAnimation.Play("UpperMove");
            }
            else if (playerDir == Vector3.back)
            {
                _playerAnimation.Play("LowerMove");
            }
        }

        protected override void MoveStop()
        {
            _playerAnimation.Play("Idle");
            base.MoveStop();
        }

        private Vector3 CamDirCheck(Vector3 direction)
        {
            if (cameraDir.x != 0)
            {
                float swap = direction.x;
                direction.x = direction.z * cameraDir.x;
                direction.z = cameraDir.x * -swap;
            }
            else if (cameraDir.z != 0)
            {
                direction.x = direction.x * cameraDir.z;
                direction.z = direction.z * cameraDir.z;
            }

            return direction;
        }
    }
}