using System;
using System.Collections;
using System.Collections.Generic;
using Core;
using UnityEngine;
using Units.Behaviours.Unit;
using DG.Tweening;
using Managements.Managers;
using Input = Managements.Managers.Input;

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

        public override void Update()
        {
            base.Update();

            PopMove();
        }

        public override void Translate(Vector3 dir)
        {
            if (moveDir.Count > 1) return;
            // 현재 스피드를 계산하는 식 필요
            moveDir.Enqueue(new MoveNode(dir, 0.5f));
        }

        public void ClearMove()
        {
            moveDir.Clear();
        }

        public void PopMove()
        {
            if(moveDir.Count > 0 && !isMoving)
            {
                MoveNode nextNode = moveDir.Dequeue();
                MoveTo(nextNode.dir, nextNode.speed);
            }
        }

        public override void MoveTo(Vector3 pos, float spd = 1)
        {
            if (isMoving)
                return;

            Vector3 originalPos = ThisBase.transform.position;
            Vector3 nextPos = originalPos + pos;

            _seq = DOTween.Sequence();
            isMoving = true;

            var distance = Vector3.Distance(originalPos, nextPos);
            if (distance < 0.1f)
            {
                isMoving = false;
                _seq.Kill();
                return;
            }

            _seq.Append(ThisBase.transform.DOMove(nextPos, spd).SetEase(Ease.Linear));
            _seq.AppendCallback(() =>
            {
                isMoving = false;
                _seq.Kill();
            });
        }
    }
}
