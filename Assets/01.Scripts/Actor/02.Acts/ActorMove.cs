using DG.Tweening;
using UnityEngine;

namespace Actor.Acts
{
    public class ActorMove : Act
    {
        public void Translate(Vector3 dir, Weapon weapon = null)
        {
            MoveTo(_actorController.Position + dir, weapon);
        }

        public void MoveTo(Vector3 pos, Weapon weapon = null)
        {
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
            }
            seq.Append(transform.DOMove(nextPos, speed).SetEase(Ease.Linear));
        }
    }
}