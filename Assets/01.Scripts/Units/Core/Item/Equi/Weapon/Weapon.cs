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
		OldTwinSword,
		OldSpear
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
		protected UnitStat _unitStat;

        protected float _currentTime;
        protected float _maxTime;

        protected bool _isCoolTime = false;

        public bool isSkill = false;

		protected bool _isEnemy = true;
		public override void Start()
		{
			//여기서 다 받아주고
			_unitAttack = _thisBase.GetBehaviour<UnitAttack>();
			_unitMove = _thisBase.GetBehaviour<UnitMove>();
			_unitStat = _thisBase.GetBehaviour<UnitStat>();
		}

		public override void Update()
		{
			Timer();
		}

		public virtual void ChangeKey()
		{
			_inputManager.ClearInGameAction(InputTarget.UpMove);
			_inputManager.ClearInGameAction(InputTarget.DownMove);
			_inputManager.ClearInGameAction(InputTarget.LeftMove);
			_inputManager.ClearInGameAction(InputTarget.RightMove);

			_inputManager.ClearInGameAction(InputTarget.UpAttack);
			_inputManager.ClearInGameAction(InputTarget.DownAttack);
			_inputManager.ClearInGameAction(InputTarget.LeftAttack);
			_inputManager.ClearInGameAction(InputTarget.RightAttack);

			_inputManager.AddInGameAction(InputTarget.UpMove, InputStatus.Press, () => Move(Vector3.forward));
			_inputManager.AddInGameAction(InputTarget.DownMove, InputStatus.Press, () => Move(Vector3.back));
			_inputManager.AddInGameAction(InputTarget.LeftMove, InputStatus.Press, () => Move(Vector3.left));
			_inputManager.AddInGameAction(InputTarget.RightMove, InputStatus.Press, () => Move(Vector3.right));

			_inputManager.AddInGameAction(InputTarget.UpAttack, InputStatus.Press, () => Attack(Vector3.forward));
			_inputManager.AddInGameAction(InputTarget.DownAttack, InputStatus.Press, () => Attack(Vector3.back));
			_inputManager.AddInGameAction(InputTarget.LeftAttack, InputStatus.Press, () => Attack(Vector3.left));
			_inputManager.AddInGameAction(InputTarget.RightAttack, InputStatus.Press, () => Attack(Vector3.right));
			_isEnemy = false;
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