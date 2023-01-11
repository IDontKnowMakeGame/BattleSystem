using DG.Tweening;
using Manager;
using Unit;
using UnityEngine;
using System;

namespace Unit.Player
{
    [System.Serializable]
    public class PlayerMove : UnitMove
    {
        private float speed;

        private Vector3 _moveDirection = Vector3.zero;
        private bool isMoving = false;
        private Vector3 _originPosition;
        private Sequence _seq;

        PlayerWeapon _weapon;
		public override void Start()
        {
            _weapon = thisBase.GetBehaviour<PlayerWeapon>();
            speed = thisBase.GetBehaviour<PlayerStats>().GetCurrentStat().agi;
            base.Start();  
        }

        public override void Update()
        {
            if (_weapon.currentWeapon.isSkill)
			{
                Debug.Log("ㅋ");
                return;
			}
        }

        public override void Translate(Vector3 dir, float s = 0)
        {

            if (isMoving == true)
                return;

            var originalPos = thisBase.transform.position;
            _originPosition = originalPos;
            var nextPos = originalPos + dir;

            if (GameManagement.Instance.GetManager<MapManager>().IsMovablePosition(nextPos) == false)
                return;

            _seq = DOTween.Sequence();
            isMoving = true;
            var distance = Vector3.Distance(originalPos, nextPos);
            if (distance < 0.1f)
            {
                isMoving = false;
                _seq.Kill();
                return;
            }

            float speeds = s != 0 ? s : speed;
            _seq.Append(thisBase.transform.DOMove(nextPos, speeds).SetEase(Ease.Linear));
            _seq.AppendCallback(() =>
            {
                position = nextPos;
                GameManagement.Instance.GetManager<MapManager>().GetBlock(thisBase.transform.position).MoveUnitOnBlock(thisBase);
                GameManagement.Instance.GetManager<MapManager>().GetBlock(_originPosition).RemoveUnitOnBlock();
                onBehaviourEnd?.Invoke();
                onBehaviourEnd = null;
                _moveDirection = Vector2.zero;
                isMoving = false;
                _seq.Kill();
            });
        }
    }
}