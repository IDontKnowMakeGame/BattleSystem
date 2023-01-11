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
        private bool isMoving = false;
        private Vector3 _originPosition;
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
            if (GameManagement.Instance.GetManager<MapManager>().IsMovablePosition(nextPos) == false)
            {
                isMoving = false;
                return;
            }
            if (distance < 0.1f)
            {
                isMoving = false;
                _seq.Kill();
                return;
            }

            float speeds = speed != 0 ? speed : 1;
            _seq.Append(thisBase.transform.DOMove(nextPos, speeds).SetEase(Ease.Linear));
            _seq.AppendCallback(() =>
            {
                position = nextPos;
                GameManagement.Instance.GetManager<MapManager>().GetBlock(thisBase.transform.position).MoveUnitOnBlock(thisBase);
                GameManagement.Instance.GetManager<MapManager>().GetBlock(_originPosition).RemoveUnitOnBlock();
                onBehaviourEnd?.Invoke();
                onBehaviourEnd = null;
                isMoving = false;
                _seq.Kill();
            });
        }

        public bool IsMoving() => isMoving;
    }
}