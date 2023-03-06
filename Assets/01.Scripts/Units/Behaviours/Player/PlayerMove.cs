using System;
using System.Collections;
using System.Collections.Generic;
using Core;
using UnityEngine;
using Units.Behaviours.Unit;
using DG.Tweening;
using Managements.Managers;
using Units.Base.Unit;
using Unity.Burst.Intrinsics;

struct MoveNode
{
    public Vector3 dir;
    public float speed;

    public MoveNode(Vector3 dir, float speed)
    {
        this.dir = dir;
        this.speed = speed;
    }
}

namespace Units.Base.Player
{
    [System.Serializable]
    public class PlayerMove : UnitMove
    {
        private Queue<MoveNode> moveDir = new Queue<MoveNode>();

        private Vector3 playerDir;

        [SerializeField]
        private float smoothMoving;

        private Transform sprite;

        private PlayerAnimation playerAnimation;

        private bool twoAnimation = true;

        public bool stop = false;

        public override void Awake()
        {
            base.Awake();
        }

        public override void Start()
        {
            InputManager.OnMovePress += EnqueueMove;

            sprite = ThisBase.GetComponentInChildren<MeshRenderer>().transform;
            playerAnimation = ThisBase.GetBehaviour<PlayerAnimation>();

            ResetMove();
        }

        public void ResetMove()
        {
            // SpawnPostion Setting
            _seq.Kill();
            isMoving = false;
            onBehaviourEnd?.Invoke();
            ClearMove();
        }
        
        public override void Update()
        {
            ChangeDir();
            PopMove();
        }

        public void EnqueueMove(Vector3 dir)
        {
            if (moveDir.Count > 1 || ThisBase.State.HasFlag(BaseState.Moving) || stop) return;
            var speed = ThisBase.GetBehaviour<UnitStat>().NowStats.Agi;
            moveDir.Enqueue(new MoveNode(dir, speed));
		}

		public override void Translate(Vector3 dir, float spd = 1)
        {
            if (isMoving || ThisBase.State.HasFlag(BaseState.StopMove) || 
                !playerAnimation.CurWeaponAnimator.LastChange || 
                ThisBase.GetBehaviour<PlayerItem>().PlayerPortion.UsePortion || stop)
            {
                ThisBase.RemoveState(BaseState.Moving);
                return;
            }

            if (playerDir.x != 0)
            {
                float swap = dir.x;
                dir.x = dir.z * playerDir.x;
                dir.z = playerDir.x * -swap;
            }
            else if (playerDir.z != 0)
            {
                dir.x = dir.x * playerDir.z;
                dir.z = dir.z * playerDir.z;
            }
            
            MoveTo(ThisBase.Position + dir * distance, spd);
        }

        public override void MoveTo(Vector3 pos, float spd = 1)
        {
            var attack = ThisBase.GetBehaviour<PlayerAttack>();
            PlayerEquiq playerEqiq = ThisBase.GetBehaviour<PlayerEquiq>();
            if (attack.IsAttack && playerEqiq.WeaponAnimation() != 1)
            {
                ThisBase.RemoveState(BaseState.Moving);
                return;
            }

            var map = Define.GetManager<MapManager>();

            var nextPos = pos;
            var orignalPos = ThisBase.Position;
            nextPos.y = 1;

            if (InGame.GetUnit(pos) != null)
            {
                if (playerEqiq.WeaponAnimation() != 1)
                {
                    playerAnimation.CurWeaponAnimator.ResetParameter();
                    playerAnimation.SetAnmation();
                }
                else
                    MoveAnimation(nextPos - orignalPos);

                ThisBase.RemoveState(BaseState.Moving);
                return;
            }

            if (map.GetBlock(pos) == null || !map.GetBlock(pos).isWalkable)
            {
                if(playerEqiq.WeaponAnimation() != 1)
                {
                    playerAnimation.CurWeaponAnimator.ResetParameter();
                    playerAnimation.SetAnmation();
                }
                else
                    MoveAnimation(nextPos - orignalPos);

                ThisBase.RemoveState(BaseState.Moving);
                return;
            }
            else
            {
                LockTile lockTile = map.GetBlock(orignalPos)?.GetComponent<LockTile>();

                if (lockTile != null)
                    lockTile.CheckDir(nextPos);
            }

            
            _seq = DOTween.Sequence();
            isMoving = true;
            MoveAnimation(nextPos - orignalPos);
                

            var distance = Vector3.Distance(ThisBase.Position, nextPos);
            if (distance < 0.1f)
            {
                isMoving = false;
                ThisBase.RemoveState(BaseState.Moving);
                PlayerStop();
                _seq.Kill();
                return;
            }
            
            InGame.SetUnit(ThisBase, nextPos);

            _seq.Append(ThisBase.transform.DOMove(nextPos, spd).SetEase(Ease.OutCubic));
            _seq.InsertCallback(spd / 2, () =>
            {
                ThisBase.Position = nextPos;
            });
            _seq.InsertCallback(spd / smoothMoving, () =>
            {
                ClearMove();
                ThisBase.RemoveState(BaseState.Moving);
            });
            _seq.AppendCallback(() =>
            {
                isMoving = false;
                if(!attack.IsAttack || playerEqiq.WeaponAnimation() == 1)
                    PlayerStop();
                ThisBase.Position = nextPos;
                InGame.SetUnit(null, orignalPos);
                onBehaviourEnd?.Invoke();
                _seq.Kill();
            });
        }

        public void ClearMove()
        {
            moveDir.Clear();
        }

        public void PopMove()
        {
            if (moveDir.Count > 0 && !isMoving && !ThisBase.State.HasFlag(BaseState.Moving))
            {
                MoveNode nextNode = moveDir.Dequeue();
                ThisBase.AddState(BaseState.Moving);
                Translate(nextNode.dir, nextNode.speed);
            }
        }
        public void PlayerStop()
        {
            if (moveDir.Count == 0 && !ThisBase.State.HasFlag(BaseState.Skill))
            {
                playerAnimation.CurWeaponAnimator.ResetParameter();
                playerAnimation.SetAnmation();
            }
        }

        private void ChangeDir()
        {
            Vector3 heading = Define.MainCam.transform.localRotation * Vector3.forward;
            heading.y = 0;
            heading = heading.normalized;

            if(Math.Abs(heading.x) >= Math.Abs(heading.z))
            {
                if (heading.x >= 0) playerDir = Vector3.right;
                else playerDir = Vector3.left;
            }
            else
            {
                if (heading.z >= 0) playerDir = Vector3.forward;
                else playerDir = Vector3.back;
            }
        }

        private void MoveAnimation(Vector3 dir)
        {
            dir.y = 0;
            dir.Normalize();

  /*          if (sprite == null)
                Debug.LogError("Sprite is null.");*/
            if(playerAnimation == null)
                Debug.LogError("PlayerAnimation is null.");

            playerAnimation.CurWeaponAnimator.SetDir = dir;
            playerAnimation.CurWeaponAnimator.Moving = true;
            playerAnimation.SetAnmation();

            /*            if (ThisBase.GetBehaviour<PlayerEqiq>().WeaponAnimation() == 1)
                        {
                            twoAnimation = !twoAnimation;
                            if (twoAnimation)
                            {
                                unitAnimation.ChangeState(7);
                            }
                        }*/
        }

        public override void OnDisable()
        {
            InputManager.OnMovePress -= EnqueueMove;
            base.OnDisable();
        }
    }
}
