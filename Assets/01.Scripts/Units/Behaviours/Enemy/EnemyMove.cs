using Core;
using DG.Tweening;
using Managements.Managers;
using Units.Behaviours.Unit;
using UnityEngine;

namespace Units.Base.Enemy
{
    public class EnemyMove : UnitMove
    {
        protected bool IsFloating = false;
        public override void MoveTo(Vector3 pos, float spd = 1)
        {
            var map = Define.GetManager<MapManager>();
            if (map.GetBlock(pos) == null)
                return;
            if (InGame.GetUnit(pos) != null)
                return;
            if (isMoving)
            {
                return;
            }
            var nextPos = pos;
            var originPos = ThisBase.Position;
            nextPos.y = 1;

            
            var distance = Vector3.Distance(ThisBase.Position, nextPos);
            if (distance < 0.1f)
            {
                isMoving = false;
                _seq.Kill();
                return;
            }

            InGame.SetUnit(ThisBase, nextPos);
            _seq = DOTween.Sequence();
            isMoving = true;
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

        public void JumpTo(Vector3 pos, float pow = 1, float spd = 1)
        {
            var map = Define.GetManager<MapManager>();
            if (map.GetBlock(pos) == null)
                return;
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

            IsFloating = true;
            InGame.SetUnit(ThisBase, nextPos);
            _seq.Append(ThisBase.transform.DOJump(nextPos, pow, 1, spd).SetEase(Ease.InQuad));
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
                IsFloating = false;
                _seq.Kill();
            });
        }
        
        public bool IsMoving()
        {
            return isMoving;
        }
    }
}