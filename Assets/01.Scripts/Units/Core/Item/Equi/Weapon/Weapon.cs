using System;
using UnityEngine;
using Units.Behaviours.Unit;
using Units.Base.Unit;
using Managements;
using Managements.Managers;
using Units.Base.Player;
using Core;

namespace Unit.Core.Weapon
{
	public enum WeaponType
	{
		OldStraightSword,
		OldGreatSword,
		OldTwinSword,
		OldSpear,
		TaintedSword,
		End
	}

    [Serializable]
    public class Weapon:EquipmentItem
    {
        public UnitBase _thisBase;

        protected WeaponStats _weaponStats = null;

		protected AttackCollider _attackCollider = null;
		public WeaponStats WeaponStat => _weaponStats;

		//유닛 공격, 유닛 move
		protected UnitAttack _unitAttack;
		protected UnitMove _unitMove;
		protected UnitStat _unitStat;

		protected PlayerAttack _playerAttack;

        protected float _currentTime;
        protected float _maxTime;

        protected bool _isCoolTime = false;

        public bool isSkill = false;

		protected bool _isEnemy = true;

		protected SliderObject _sliderObject;
		public override void Start()
		{
			//여기서 다 받아주고
			_unitAttack = _thisBase.GetBehaviour<UnitAttack>();
			_unitMove = _thisBase.GetBehaviour<UnitMove>();
			_unitStat = _thisBase.GetBehaviour<UnitStat>();

			_playerAttack = _unitAttack as PlayerAttack;
		}

		public override void Update()
		{
			Timer();
		}

		public virtual void ChangeKey()
		{
			_inputManager.AddInGameAction(InputTarget.UpAttack, InputStatus.Press, UpAttack);
			_inputManager.AddInGameAction(InputTarget.DownAttack, InputStatus.Press, DownAttack);
			_inputManager.AddInGameAction(InputTarget.LeftAttack, InputStatus.Press, LeftAttack);
			_inputManager.AddInGameAction(InputTarget.RightAttack, InputStatus.Press, RightAttack);
			_inputManager.AddInGameAction(InputTarget.Skill, InputStatus.Press, Skill);
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
			WeaponStateDataList weaponStateDataList = DataJson.LoadJsonFile<WeaponStateDataList>(Application.dataPath + "/SAVE/Weapon", "WeaponStatus");
			foreach (WeaponStateData data in weaponStateDataList.weaponList)
			{
				if (data.name == name)
				{
					_weaponStats = WeaponSerializable(data);
					break;
				}
			}
		}
		public WeaponStats WeaponSerializable(WeaponStateData data)
		{
			WeaponStats state = new WeaponStats();

			state.Afs = data.attackAfterDelay;
			state.Atk = data.damage;
			state.Ats = data.attackSpeed;
			state.Weight = data.weaponWeight;

			return state;
		}

		protected virtual void Attack(Vector3 vec)
		{

		}

		protected virtual void UpAttack()
		{
			_thisBase.StartCoroutines(_weaponStats.Ats, () => Attack(Vector3.forward));
		}
		protected virtual void DownAttack()
		{
			_thisBase.StartCoroutines(_weaponStats.Ats, () => Attack(Vector3.back));
		}
		protected virtual void LeftAttack()
		{
			_thisBase.StartCoroutines(_weaponStats.Ats, () => Attack(Vector3.left));
		}
		protected virtual void RightAttack()
		{
			_thisBase.StartCoroutines(_weaponStats.Ats, () => Attack(Vector3.right));
		}

		protected virtual void Skill()
		{

		}

		public virtual void Reset()
		{
			_inputManager.RemoveInGameAction(InputTarget.UpAttack, InputStatus.Press, UpAttack);
			_inputManager.RemoveInGameAction(InputTarget.DownAttack, InputStatus.Press, DownAttack);
			_inputManager.RemoveInGameAction(InputTarget.LeftAttack, InputStatus.Press, LeftAttack);
			_inputManager.RemoveInGameAction(InputTarget.RightAttack, InputStatus.Press, RightAttack);
			_inputManager.RemoveInGameAction(InputTarget.Skill, InputStatus.Press, Skill);
		}
	}
}