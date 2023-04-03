using Acts.Characters;
using Managements.Managers;
using UnityEngine;
using Core;
using Actors.Characters.Player;

namespace Acts.Characters.Player
{
    [System.Serializable]
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
        }

        public override void Translate(Vector3 direction)
        {
            if (_playerActor.HasAnyState()) return;
            playerDir = direction;
            direction = InGame.CamDirCheck(direction);
            base.Translate(direction * distance);
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
                if((_playerActor.currentWeapon is OldSpear == false) || !((_playerActor.currentWeapon as OldSpear).IsDown))
                    ThisActor.SpriteTransform.localScale = new Vector3(-2, 1, 1);
                _playerAnimation.Play("VerticalMove");
            }
            else if(playerDir == Vector3.right)
            {
                if ((_playerActor.currentWeapon is OldSpear == false) || !((_playerActor.currentWeapon as OldSpear).IsDown))
                    ThisActor.SpriteTransform.localScale = new Vector3(2, 1, 1);
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
    }
}