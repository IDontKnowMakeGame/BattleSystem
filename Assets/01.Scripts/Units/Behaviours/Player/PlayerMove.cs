using System;
using System.Collections;
using System.Collections.Generic;
using Core;
using UnityEngine;
using Units.Behaviours.Unit;
using DG.Tweening;

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
        private float spd = 0.5f;

        private GameObject mainCam;

        private Vector3 playerDir;

        public override void Start()
        {
            mainCam = Camera.main.gameObject;
        }

        public override void Update()
        {
            ChangeRotate();
            ChangeDir();
            PopMove();
        }

        public void EnqueueMove(Vector3 dir)
        {
            if (moveDir.Count > 1) return;
            // 현재 스피드를 계산하는 식 필요
            moveDir.Enqueue(new MoveNode(dir, 0.5f));
        }

        public override void Translate(Vector3 dir)
        {
            if (isMoving)
                return;

            Vector3 originalPos = ThisBase.transform.position;

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
            Vector3 nextPos = originalPos + dir;

            _seq = DOTween.Sequence();
            isMoving = true;

            var distance = Vector3.Distance(originalPos, nextPos);
            if (distance < 0.1f)
            {
                isMoving = false;
                PlayerStop();
                _seq.Kill();
                return;
            }

            _seq.Append(ThisBase.transform.DOMove(nextPos, spd).SetEase(Ease.Linear));
            _seq.InsertCallback(spd / 2, () => ThisBase.Position = nextPos);
            _seq.AppendCallback(() =>
            {
                isMoving = false;
                PlayerStop();
                ThisBase.Position = nextPos;
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
                Translate(nextNode.dir);
                Debug.Log(3);
            }
        }
        public void PlayerStop()
        {
            if (moveDir.Count == 0)
            {
                
            }
        }

        private void ChangeRotate()
        {
            Vector3 playerRotate = ThisBase.transform.rotation.eulerAngles;
            playerRotate.y = mainCam.transform.rotation.eulerAngles.y;

            ThisBase.transform.rotation = Quaternion.Euler(playerRotate);
        }

        private void ChangeDir()
        {
            Vector3 heading = mainCam.transform.localRotation * Vector3.forward;
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
