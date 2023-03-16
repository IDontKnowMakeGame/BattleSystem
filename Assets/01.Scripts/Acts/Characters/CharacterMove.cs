using System.Collections;
using System.Runtime.InteropServices.WindowsRuntime;
using Acts.Base;
using Core;
using DG.Tweening;
using Managements.Managers;
using UnityEngine;

namespace Acts.Characters
{
    public class CharacterMove : Act
    {
        private Transform _thisTransform;
        protected bool _isMoving = false;
        
        public override void Awake()
        {
            _thisTransform = ThisActor.transform;
        }

        protected void Translate(Vector3 direction)
        {
            var nextPos = ThisActor.Position + direction;
            Move(nextPos);
        }
        
        protected virtual void Move(Vector3 position)
        {
            if (_isMoving) return;
            var seq = DOTween.Sequence();
            var currentPos = ThisActor.Position;
            var nextPos = position;
            nextPos.y = 1;

            MoveAnimation(position - currentPos);

            ThisActor.StartCoroutine(PositionUpdateCoroutine());
            seq.Append(_thisTransform.DOMove(nextPos, 0.3f).SetEase(Ease.Linear));
            seq.AppendCallback(() =>
            {
                ThisActor.Position = nextPos;
                _isMoving = false;
                MoveStop();
                seq.Kill();
            });
        }
        
        public IEnumerator PositionUpdateCoroutine()
        {
            _isMoving = true;
            while (_isMoving)
            {
                yield return new WaitForFixedUpdate();
                var pos = _thisTransform.position;
                if (Mathf.Round(pos.x) != pos.x)
                {
                    pos.x = Mathf.Round(pos.x);
                }

                if (Mathf.Round(pos.z) != pos.z)
                {
                    pos.z = Mathf.Round(pos.z);
                }
                
                InGame.SetActorOnBlock(ThisActor, pos);
                ThisActor.Position = pos;
            }
        }

        protected virtual void MoveAnimation(Vector3 dir)
        {

        }

        protected virtual void MoveStop()
        {

        }
    }
}