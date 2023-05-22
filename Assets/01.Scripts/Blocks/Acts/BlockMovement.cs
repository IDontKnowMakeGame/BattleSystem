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
        private Transform _anchorTransform;
        private Transform _modelTransform;
        private bool isMoving = false;
        public override void Awake()
        {
            _anchorTransform = ThisActor.transform.Find("Anchor");
            _modelTransform = _anchorTransform.Find("Model");
            base.Awake();
        }


        public void Shake(float duration, float strength = 1f, int vibrato = 10, float randomness = 90f)
        {
            if (_anchorTransform == null)
                return;
            if(isMoving) return;
            isMoving = true;
            _modelTransform.gameObject.SetActive(true);
            var seq = DOTween.Sequence();
            seq.Append(_anchorTransform.DOShakePosition(duration, strength, vibrato, randomness));
            seq.AppendCallback(() =>
            {
                if (_anchorTransform == null)
                {
                    isMoving = false;
                    seq.Kill();
                    return;
                }
                _anchorTransform.localPosition = Vector3.zero;
                isMoving = false;
                _modelTransform.gameObject.SetActive(false);
                seq.Kill(true);
            });
        }

        public void Bounce(float duration, float strength = 0.5f)
        {
            if(isMoving) return;
            isMoving = true;
            _modelTransform.gameObject.SetActive(true);
            var seq = DOTween.Sequence();
            seq.Append(_anchorTransform.DOLocalJump(Vector3.zero, strength, 1, duration));
            seq.AppendCallback(() =>
            {
                _anchorTransform.localPosition = Vector3.zero;
                isMoving = false;
                _modelTransform.gameObject.SetActive(false);
                seq.Kill(true);
            });
        }
        
        public void Roll(float duration, float strength = 0.5f)
        {
            if(isMoving) return;
            isMoving = true;
            _modelTransform.gameObject.SetActive(true);
            var seq = DOTween.Sequence();
            seq.Append(_anchorTransform.DOLocalRotate(new Vector3(900, 900, 900), duration, RotateMode.LocalAxisAdd));
            seq.Join(_anchorTransform.DOLocalJump(Vector3.zero, strength / 2f, 1, duration));
            seq.AppendCallback(() =>
            {
                _anchorTransform.localRotation = Quaternion.identity;
                isMoving = false;
                _modelTransform.gameObject.SetActive(false);
                seq.Kill(true);
            });
        }
    }
}