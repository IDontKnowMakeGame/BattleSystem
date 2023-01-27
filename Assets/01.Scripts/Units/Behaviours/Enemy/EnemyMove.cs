using Core;
using DG.Tweening;
using Units.Behaviours.Unit;
using UnityEngine;

namespace Units.Base.Enemy
{
    public class EnemyMove : UnitMove
    {
        public override void MoveTo(Vector3 pos, float spd = 1)
        {
            Vector3 nextPos = pos;
            nextPos.y = 1;

            _seq = DOTween.Sequence();
            isMoving = true;

            var distance = Vector3.Distance(ThisBase.Position, nextPos);
            if (distance < 0.1f)
            {
                isMoving = false;
                _seq.Kill();
                return;
            }

            _seq.Append(ThisBase.transform.DOMove(nextPos, spd).SetEase(Ease.Linear));
            _seq.InsertCallback(spd / 2, () =>
            {
                ThisBase.Position = nextPos;
                InGame.SetUnit(ThisBase, ThisBase.Position);
            });
            _seq.AppendCallback(() =>
            {
                isMoving = false;
                ThisBase.Position = nextPos;
                onBehaviourEnd?.Invoke();
                _seq.Kill();
            });
        }
        
        public bool IsMoving()
        {
            return isMoving;
        }
    }
}