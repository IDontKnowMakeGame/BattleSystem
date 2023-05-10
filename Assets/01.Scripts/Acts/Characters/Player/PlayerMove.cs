using Acts.Characters;
using Managements.Managers;
using UnityEngine;
using Core;
using Actors.Characters.Player;
using Actors.Characters;
using System.Collections.Generic;

namespace Acts.Characters.Player
{
    [System.Serializable]
    public class PlayerMove : CharacterMove
    {
        private PlayerAnimation _playerAnimation;
        private PlayerActor _playerActor;

        private Vector3 cameraDir;
        private Vector3 playerDir;
        public Vector3 SkillDir { get; set; }

        public float distance = 1;

        private bool isSkill = false;
        public bool IsSKill
        {
            get => isSkill;
            set => isSkill = value;
        }

        private Queue<Vector3> moveDir = new Queue<Vector3>();

        [SerializeField]
        private ParticleSystem dust;

        public override void Awake()
        {         
            base.Awake();
            InputManager<Weapon>.OnMovePress += EnqueMove;
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
            PopMove();
        }

        public override void Translate(Vector3 direction)
        {
            if (_playerActor.HasAnyState()) return;
            playerDir = direction;
            direction = InGame.CamDirCheck(direction);

           Debug.Log(distance);
            base.Translate(direction * distance);
        }

        public override void Move(Vector3 position)
        {
            if (isSkill)
                _isMoving = false;
            base.Move(position);
        }
        #region Test Code
        private void EnqueMove(Vector3 direction)
        {
            if (moveDir.Count > 1 || enableQ) return;
            moveDir.Enqueue(direction);
        }
        private void PopMove()
        {
            if(moveDir.Count > 0 && !_playerActor.HasAnyState() && !_isMoving)
            {
                enableQ = true;
                playerDir = moveDir.Dequeue();
                Vector3 dir = InGame.CamDirCheck(playerDir);
                dust.Play();
                base.Translate(dir * distance);
            }
        }
        #endregion

        public void BowBackStep(Vector3 position)
        {
            playerDir = (position - ThisActor.Position);
            Debug.Log(playerDir);
            base.Move(position);
        }

        /// <summary>
        /// Player Animation Setting
        /// </summary>
        protected override void AnimationCheck()
        {
            if(isSkill)
            {
                SkillAnimation();
            }
            else
            {
                OrginalAnimation();
            }    
        }

        private void OrginalAnimation()
        {
            if (playerDir == Vector3.left)
            {
                if (_playerActor.currentWeapon is OldSpear == false || (_playerActor.currentWeapon as OldSpear).NonDir == false)
                    ThisActor.SpriteTransform.localScale = new Vector3(-2, 1, 1);
                _playerAnimation.Play("HorizontalMove");
            }
            else if (playerDir == Vector3.right)
            {
                if (_playerActor.currentWeapon is OldSpear == false || (_playerActor.currentWeapon as OldSpear).NonDir == false)
                {
                    ThisActor.SpriteTransform.localScale = new Vector3(2, 1, 1);
                }
                _playerAnimation.Play("HorizontalMove");
            }
            else if (playerDir == Vector3.forward)
            {
                _playerAnimation.Play("UpperMove");
            }
            else if (playerDir == Vector3.back)
            {
                _playerAnimation.Play("LowerMove");
            }
        }

       private void SkillAnimation()
        {
            Debug.Log(SkillDir);
            if (SkillDir == Vector3.left)
            {
                ThisActor.SpriteTransform.localScale = new Vector3(-2, 1, 1);
                _playerAnimation.Play("HorizontalSkill");
            }
            else if (SkillDir == Vector3.right)
            {
                ThisActor.SpriteTransform.localScale = new Vector3(2, 1, 1);
                _playerAnimation.Play("HorizontalSkill");
            }
            else if (SkillDir == Vector3.forward)
            {
                _playerAnimation.Play("UpperSkill");
            }
            else if (SkillDir == Vector3.back)
            {
                _playerAnimation.Play("LowerSkill");
            }
        }

        protected override void MoveStop()
        {
            if (ThisActor.GetAct<CharacterStatAct>().ChangeStat.hp <= 0) return;

            if(isSkill)
            {
                isSkill = false;
            }
            else 
            {
                _playerAnimation.Play("Idle");
            }

            dust.Stop();
            base.MoveStop();
            //QuestManager.Instance.CheckRoomMission(ThisActor.Position);
        }
    }
}