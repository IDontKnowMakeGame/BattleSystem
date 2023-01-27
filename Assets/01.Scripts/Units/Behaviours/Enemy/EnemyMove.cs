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
            if (InGame.GetUnit(pos) != null)
                return;
            var nextPos = pos;
            var originPos = ThisBase.Position;
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

            InGame.SetUnit(ThisBase, nextPos);
            _seq.Append(ThisBase.transform.DOMove(nextPos, spd).SetEase(Ease.Linear));
            _seq.InsertCallback(spd / 2, () =>
            {
                ThisBase.Position = nextPos;
            });
            _seq.AppendCallback(() =>
            {
                isMoving = false;
                ThisBase.Position = nextPos;
                InGame.SetUnit(null, originPos);
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