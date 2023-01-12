using DG.Tweening;
using Manager;
using UnityEngine;
using UnityEngine.Formats.Alembic.Timeline;

namespace Unit.Enemy.Base
{
    public class EnemyMove : UnitMove
    {
        private float speed;
        private Sequence _seq;
        public override void Translate(Vector3 dir, float speed = 0)
        {
            if (isMoving == true)
                return;

            _seq = DOTween.Sequence();
            isMoving = true;
            var originalPos = thisBase.transform.position;
            _originPosition = originalPos;
            dir.y = originalPos.y;
            var nextPos = dir;
            var distance = Vector3.Distance(originalPos, nextPos);
            if (distance < 0.1f)
            {
                isMoving = false;
                _seq.Kill();
                return;
            }

            float speeds = speed != 0 ? speed : 1;
            speeds = 0.2f;
            _seq.Append(thisBase.transform.DOMove(nextPos, speeds).SetEase(Ease.Linear));
            _seq.AppendCallback(() =>
            {
                Move(nextPos);
                onBehaviourEnd?.Invoke();
                onBehaviourEnd = null;
                isMoving = false;
                _seq.Kill();
            });
        }
    }
}