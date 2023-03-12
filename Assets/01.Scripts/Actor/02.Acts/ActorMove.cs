using System.Collections;
using DG.Tweening;
using UnityEngine;

namespace Actor.Acts
{
    public class ActorMove : Act
    {
        private bool _isMoving = false;
        public void Translate(Vector3 dir, Weapon weapon = null)
        {
            MoveTo(_actorController.Position + dir, weapon);
        }

        public void MoveTo(Vector3 pos, Weapon weapon = null)
        {
            if(_isMoving) return;
            Sequence seq = DOTween.Sequence();
            var nextPos = pos;
            var curPos = _actorController.Position;
            float speed = 0f;
            if(weapon == null)
            {
                speed = 0.5f;
            }
            else
            {
                speed = weapon.itemInfo.WeightToSpeed;
                Debug.Log(speed);
            }

            StartCoroutine(PositionUpdateCoroutine());
            seq.Append(transform.DOMove(nextPos, speed).SetEase(Ease.Linear));
            seq.AppendCallback(() =>
            {
                _isMoving = false;
                _actorController.Position = nextPos;
                seq.Kill();
            });
        }

        public IEnumerator PositionUpdateCoroutine()
        {
            _isMoving = true;
            while (_isMoving)
            {
                yield return new WaitForFixedUpdate();
                var pos = transform.position;
                if (Mathf.Round(pos.x) != pos.x)
                {
                    pos.x = Mathf.Round(pos.x);
                }

                if (Mathf.Round(pos.z) != pos.z)
                {
                    pos.z = Mathf.Round(pos.z);
                }

                _actorController.Position = pos;
            }
        }
    }
}