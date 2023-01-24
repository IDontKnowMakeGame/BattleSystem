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

        public override void Start()
        {
        }

        public override void Update()
        {
            PopMove();
        }

        public void EnqueueMove(Vector3 dir)
        {
            if (moveDir.Count > 1) return;
            // ���� ���ǵ带 ����ϴ� �� �ʿ�
            moveDir.Enqueue(new MoveNode(dir, 0.5f));
        }

        public override void Translate(Vector3 dir)
        {
            if (isMoving)
                return;

            Vector3 originalPos = ThisBase.transform.position;
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
    }
}
