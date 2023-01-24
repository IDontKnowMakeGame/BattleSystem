using System;
using UnityEngine;
using Units.Behaviours.Unit;
using Managements;
using Managements.Managers;

namespace Unit.Core.Weapon
{
	public enum WeaponType
	{
		OldStraightSword,
		OldGreatSword,
		OldTwinSword
	}

    [Serializable]
    public class Weapon:EquipmentItem
    {
        public Units.Base.Units _thisBase;

        protected WeaponStats _weaponStats = null;

		protected AttackCollider _attackCollider = null;
		public WeaponStats WeaponStat => _weaponStats;

		//유닛 공격, 유닛 move
		protected UnitAttack _unitAttack;
		protected UnitMove _unitMove;

        protected float _currentTime;
        protected float _maxTime;

        protected bool _isCoolTime = false;

        public bool isSkill = false;

        public bool _isEnemy = false;

		public override void Start()
		{
			//여기서 다 받아주고
			_unitAttack = _thisBase.GetBehaviour<UnitAttack>();
			_unitMove = _thisBase.GetBehaviour<UnitMove>();

			if(!_isEnemy)
			{
				_inputManager.ChangeInGameAction(InputTarget.UpMove, () => Move(Vector3.forward));
				_inputManager.ChangeInGameAction(InputTarget.DownMove, () => Move(Vector3.back));
				_inputManager.ChangeInGameAction(InputTarget.LeftMove, () => Move(Vector3.left));
				_inputManager.ChangeInGameAction(InputTarget.RightMove, () => Move(Vector3.right));

				_inputManager.ChangeInGameAction(InputTarget.UpAttack, () => Attack(Vector3.forward));
				_inputManager.ChangeInGameAction(InputTarget.DownAttack, () => Attack(Vector3.back));
				_inputManager.ChangeInGameAction(InputTarget.LeftAttack, () => Attack(Vector3.left));
				_inputManager.ChangeInGameAction(InputTarget.RightAttack, () => Attack(Vector3.right));
			}
		}

		public override void Update()
		{
			Timer();
		}

		protected void Timer()
		{
			if (_currentTime < _maxTime && _isCoolTime)
			{
				_currentTime += Time.deltaTime;
			}
			else
			{
				_isCoolTime = false;
				_currentTime = 0;
			}
		}

		protected void GetWeaponStateData(string name)
		{
			GameManagement.Instance.GetManager<DataManager>().GetWeaponStateData(name, GetWeaponStateData);
        }
		protected void GetWeaponStateData(WeaponStats data)
        {
			_weaponStats = data;
		}

		protected virtual void Move(Vector3 vec)
		{

		}

		protected virtual void Attack(Vector3 vec)
		{
			//_weaponStats = 
		}

		protected virtual void Skill(Vector3 vec)
		{

		}
	}
}