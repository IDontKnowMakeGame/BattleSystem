using DG.Tweening;
using Manager;
using Unit;
using UnityEngine;
using System.Collections.Generic;
using System;

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

namespace Unit.Player
{
    [System.Serializable]
    public class PlayerMove : UnitMove
    {
        private float speed;

        private Vector3 _moveDirection = Vector3.zero;
        private Sequence _seq;

        Queue<MoveNode> moveDir = new Queue<MoveNode>();

        PlayerWeapon _weapon;
		public override void Start()
        {
            _weapon = thisBase.GetBehaviour<PlayerWeapon>();
            speed = thisBase.GetBehaviour<PlayerStats>().GetCurrentStat().agi;
            base.Start();  
        }

        public override void Update()
        {
            if (_weapon.currentWeapon.isSkill)
			{
                return;
			}
            PopMove();
        }

        public void InputMovement(Vector3 dir, float speed = 0)
        {
            if (moveDir.Count > 2) return;
            moveDir.Enqueue(new MoveNode(dir, speed));
        }

        public void PopMove()
        {
            if(moveDir.Count > 0 && !isMoving)
            {
                MoveNode nextNode = moveDir.Dequeue();
                Translate(nextNode.dir, nextNode.speed);
            }
        }

        public override void Translate(Vector3 dir, float s = 0)
        {
            if (isMoving == true)
                return;

            var originalPos = thisBase.transform.position;
            _originPosition = originalPos;
            var nextPos = originalPos + dir;

            if (GameManagement.Instance.GetManager<MapManager>().IsMovablePosition(nextPos) == false)
                return;

            _seq = DOTween.Sequence();
            isMoving = true;
            thisBase.GetBehaviour<PlayerAnimation>().SetMovement(isMoving);

            thisBase.transform.localScale = new Vector3(dir == Vector3.left ? -1 : dir == Vector3.right ? 1 : thisBase.transform.localScale.x, 1, 1);

            var distance = Vector3.Distance(originalPos, nextPos);
            if (distance < 0.1f)
            {
                isMoving = false;
                thisBase.GetBehaviour<PlayerAnimation>().SetMovement(isMoving);
                _seq.Kill();
                return;
            }

            float speeds = s != 0 ? s : speed;
            _seq.Append(thisBase.transform.DOMove(nextPos, speeds).SetEase(Ease.Linear));
            _seq.AppendCallback(() =>
            {
                position = nextPos;
                GameManagement.Instance.GetManager<MapManager>().GetBlock(thisBase.transform.position).MoveUnitOnBlock(thisBase);
                GameManagement.Instance.GetManager<MapManager>().GetBlock(_originPosition).RemoveUnitOnBlock();
                onBehaviourEnd?.Invoke();
                onBehaviourEnd = null;
                _moveDirection = Vector2.zero;
                isMoving = false;
                thisBase.GetBehaviour<PlayerAnimation>().SetMovement(isMoving);
                _seq.Kill();
            });
        }
    }
}