using System;
using UnityEngine;
using Units;

namespace Unit.Core.Weapon
{
	public enum WeaponType
	{

	}

    [Serializable]
    public class Weapon:EquipmentItem
    {
        public Units.Base.Units _thisBase;

        protected WeaponStats weaponStats = null;

        //인풋 매니저
		
        //유닛 공격, 유닛 move
		

        protected float _currentTime;
        protected float _maxTime;

        protected bool _isCoolTime = false;

        public bool isSkill = false;

        public bool _isEnemy = true;


		public override void Start()
		{
            //여기서 다 받아주고
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
	}
}