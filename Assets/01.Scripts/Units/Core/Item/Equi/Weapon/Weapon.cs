﻿using System;
using UnityEngine;
using Units.Behaviours.Unit;
using Managements.Managers;
using Managements;

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

		//인풋 매니저
		protected InputManager _inputManager;
		//유닛 공격, 유닛 move
		protected UnitAttack _unitAttack;
		protected UnitMove _unitMove;

        protected float _currentTime;
        protected float _maxTime;

        protected bool _isCoolTime = false;

        public bool isSkill = false;

        public bool _isEnemy = true;

		public override void Start()
		{
			//여기서 다 받아주고
			_inputManager = GameManagement.Instance.GetManager<InputManager>();
			_unitAttack = _thisBase.GetBehaviour<UnitAttack>();
			_unitMove = _thisBase.GetBehaviour<UnitMove>();

			_inputManager.AddInGameAction(InputTarget.Skill, Skill);
		}

		public override void Update()
		{
			if (!_isEnemy)
			{
				Skill();
				Move(Vector3.zero);
				Attack(Vector3.zero);
			}
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
			GameManagement.Instance.StartCoroutine((string)GameManagement.Instance.GetManager<DataManager>().GetWeaponStateData(name, GetWeaponStateData));
			Debug.Log("이게 된다고?!?!?!?");
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

		protected virtual void Skill()
		{

		}
	}
}