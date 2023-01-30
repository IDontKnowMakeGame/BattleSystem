using System.Collections.Generic;
using Unit.Core.Weapon;
using System;
using UnityEngine;

namespace Units.Behaviours.Unit
{
	[Serializable]
	public class UnitEquiq : UnitBehaviour
	{
		[SerializeField]
		protected string _currentWeapon;
		[SerializeField]
		protected string _secoundWeapon;

		public bool isEnemy;
		public Weapon CurrentWeapon
		{
			get
			{
				if (_currentWeapon == _beforeWeaponType)
					return weapons[_currentWeapon];
				else
				{
					_beforeWeaponType = _currentWeapon;

					if (!isEnemy)
					{
						weapons[_currentWeapon].ChangeKey();
						Debug.Log("Çה");
					}

					return weapons[_currentWeapon];
				}
			}
		}
		private string _beforeWeaponType;

		public Dictionary<string, Weapon> weapons = new Dictionary<string, Weapon>();

		public List<Helo> _helos = new List<Helo>();

		public override void Awake()
		{
			weapons.Add("oldSword", new OldStraightSword() { _thisBase = this.ThisBase });
			weapons.Add("oldGreatSword", new OldGreatSword() { _thisBase = this.ThisBase });
			weapons.Add("oldTwinSword", new OldTwinSword() { _thisBase = this.ThisBase });
			weapons.Add("oldSpear", new OldSpear() { _thisBase = this.ThisBase });
			weapons.Add("taintedSword", new TaintedSword() { _thisBase = this.ThisBase });

			foreach (var value in weapons)
			{
				value.Value?.Awake();
			}

			foreach (var value in _helos)
			{
				value?.Awake();
			}
		}

		public override void Start()
		{
			foreach (var value in weapons)
			{
				value.Value?.Start();
			}

			if (!isEnemy)
				weapons[_currentWeapon].ChangeKey();

			foreach (var value in _helos)
			{
				value?.Start();
			}
		}
		public override void Update()
		{
			CurrentWeapon?.Update();

			foreach (var value in _helos)
			{
				value?.Update();
			}
		}
	}
}
