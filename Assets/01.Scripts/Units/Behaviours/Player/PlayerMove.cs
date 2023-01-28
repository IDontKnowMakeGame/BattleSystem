using System;
using System.Collections;
using System.Collections.Generic;
using Core;
using UnityEngine;
using Units.Behaviours.Unit;
using DG.Tweening;
using Managements.Managers;
using Units.Base.Unit;

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
        private Vector3 spawnPos;

        public override void Awake()
        {
            base.Awake();
        }

        public override void Start()
        {
            Define.GetManager<InputManager>().ChangeInGameAction(InputTarget.UpMove, InputStatus.Press, () => Translate(Vector3.forward));
            Define.GetManager<InputManager>().ChangeInGameAction(InputTarget.DownMove, InputStatus.Press, () => Translate(Vector3.back));
            Define.GetManager<InputManager>().ChangeInGameAction(InputTarget.LeftMove, InputStatus.Press, () => Translate(Vector3.left));
            Define.GetManager<InputManager>().ChangeInGameAction(InputTarget.RightMove, InputStatus.Press, () => Translate(Vector3.right));

            SpawnSetting();
        }

        public override void Update()
        {
            ChangeDir();
            PopMove();
        }

        public void SpawnSetting()
        {
            // SpawnPostion Setting
            _seq.Kill();
            isMoving = false;
            onBehaviourEnd?.Invoke();
            ClearMove();

            ThisBase.Position = spawnPos;
            ThisBase.transform.position = spawnPos;
        }

        public void EnqueueMove(Vector3 dir)
        {
            if (moveDir.Count > 1) return;
            // 현재 스피드를 계산하는 식 필요
            var speed = ThisBase.GetBehaviour<UnitStat>().NowStats.Agi;
            moveDir.Enqueue(new MoveNode(dir, speed));
        }

        public override void Translate(Vector3 dir, float spd = 1)
        {
            if (isMoving || ThisBase.State.HasFlag(BaseState.StopMove))
                return;

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
            
            MoveTo(ThisBase.Position + dir* distance, spd);
        }

        public override void MoveTo(Vector3 pos, float spd = 1)
        {
            if (InGame.GetUnit(pos) != null)
                return;
            var nextPos = pos;
            var orignalPos = ThisBase.Position;
            nextPos.y = 1;
            
            _seq = DOTween.Sequence();
            isMoving = true;

            var distance = Vector3.Distance(ThisBase.Position, nextPos);
            if (distance < 0.1f)
            {
                isMoving = false;
                PlayerStop();
                _seq.Kill();
                return;
            }
            
            InGame.SetUnit(ThisBase, nextPos);
            _seq.Append(ThisBase.transform.DOMove(nextPos, spd).SetEase(Ease.Linear));
            _seq.InsertCallback(spd / 2, () =>
            {
                ThisBase.Position = nextPos;
            });
            _seq.AppendCallback(() =>
            {
                isMoving = false;
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
            if (moveDir.Count > 0 && !isMoving)
            {
                MoveNode nextNode = moveDir.Dequeue();
                Translate(nextNode.dir, nextNode.speed);
                Debug.Log(3);
            }
        }
        public void PlayerStop()
        {
            if (moveDir.Count == 0)
            {
                
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
    }
}
