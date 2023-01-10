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
        public Action onMoveEnd;

        private Vector3 _moveDirection = Vector3.zero;
        private bool isMoving = false;
        private Vector3 _originPosition;
        private Sequence _seq;
        InputManager _inputManager;

        PlayerWeapon _weapon;

        public override void Start()
        {
            _inputManager = GameManagement.Instance.GetManager<InputManager>();
            _weapon = thisBase.GetBehaviour<PlayerWeapon>();
        }

        public override void Update()
        {
            if (_weapon.isSkill)
			{
                return;
			}

            speed = thisBase.GetBehaviour<PlayerStats>().GetCurrentStat().agi;
            
            if (_inputManager.GetKeyInput(InputManager.InputSignal.MoveForward))
                _moveDirection.z = 1;
            if (_inputManager.GetKeyInput(InputManager.InputSignal.MoveBackward))
                _moveDirection.z = -1;
            if (_inputManager.GetKeyInput(InputManager.InputSignal.MoveRight))
                _moveDirection.x = 1;
            if (_inputManager.GetKeyInput(InputManager.InputSignal.MoveLeft))
                _moveDirection.x = -1;
            Translate(_moveDirection);
        }
        
        protected override void Translate(Vector3 dir)
        {
            if (isMoving == true)
                return;

            _seq = DOTween.Sequence();
            isMoving = true;
            var originalPos = thisBase.transform.position;
            _originPosition = originalPos;
            var nextPos = originalPos + dir;
            var distance = Vector3.Distance(originalPos, nextPos);
            if (distance < 0.1f)
            {
                isMoving = false;
                _seq.Kill();
                return;
            }

            var duration = distance / speed;
            _seq.Append(thisBase.transform.DOMove(nextPos, duration).SetEase(Ease.Linear));
            _seq.AppendCallback(() =>
            {
                
                GameManagement.Instance.GetManager<MapManager>().GetBlock(thisBase.transform.position).MoveUnitOnBlock(thisBase);
                GameManagement.Instance.GetManager<MapManager>().GetBlock(_originPosition).RemoveUnitOnBlock();
                onMoveEnd?.Invoke();
                onMoveEnd = null;
                _moveDirection = Vector2.zero;
                isMoving = false;
                _seq.Kill();
            });
        }

        public void Translation(Vector3 dir)
		{
            Translate(dir);
        }
    }
}