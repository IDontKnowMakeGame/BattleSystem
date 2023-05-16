using Acts.Base;
using DG.Tweening;
using UnityEngine;

namespace Blocks.Acts
{
    public enum MovementType
    {
        None,
        Shake,
        Bounce,
        Roll
    }
    public class BlockMovement : Act
    {
        private Transform _modelTransform;
        private bool isMoving = false;
        public override void Awake()
        {
            _modelTransform = ThisActor.transform.Find("Anchor/Model");
            base.Awake();
        }


        public void Shake(float duration, float strength = 1f, int vibrato = 10, float randomness = 90f)
        {
            if (_modelTransform == null)
                return;
            if(isMoving) return;
            isMoving = true;
            var seq = DOTween.Sequence();
            seq.Append(_modelTransform.DOShakePosition(duration, strength, vibrato, randomness));
            seq.AppendCallback(() =>
            {
                if (_modelTransform == null)
                {
                    isMoving = false;
                    seq.Kill();
                    return;
                }
                _modelTransform.localPosition = Vector3.zero;
                isMoving = false;
                seq.Kill(true);
            });
        }

        public void Bounce(float duration, float strength = 0.5f)
        {
            if(isMoving) return;
            isMoving = true;
            var seq = DOTween.Sequence();
            seq.Append(_modelTransform.DOLocalJump(Vector3.zero, strength, 1, duration));
            seq.AppendCallback(() =>
            {
                _modelTransform.localPosition = Vector3.zero;
                isMoving = false;
                seq.Kill(true);
            });
        }
        
        public void Roll(float duration, float strength = 0.5f)
        {
            if(isMoving) return;
            isMoving = true;
            var seq = DOTween.Sequence();
            seq.Append(_modelTransform.DOLocalRotate(new Vector3(900, 900, 900), duration, RotateMode.LocalAxisAdd));
            seq.Join(_modelTransform.DOLocalJump(Vector3.zero, strength / 2f, 1, duration));
            seq.AppendCallback(() =>
            {
                _modelTransform.localRotation = Quaternion.identity;
                isMoving = false;
                seq.Kill(true);
            });
        }
    }
}