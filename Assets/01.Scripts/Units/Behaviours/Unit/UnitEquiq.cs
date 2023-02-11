using System.Collections.Generic;
using Unit.Core.Weapon;
using System;
using UnityEngine;
using Unit.Core;

namespace Units.Behaviours.Unit
{
	[Serializable]
	public class UnitEquiq : UnitBehaviour
	{
		[SerializeField]
		protected string _currentWeapon;

		[SerializeField]
		protected string _secoundWeapon;

		[SerializeField]
		protected string _currentHalo;

		public bool isEnemy;
		public Weapon CurrentWeapon
		{
			get
			{
				if (_currentWeapon != null && _currentWeapon != "")
					return weapons[_currentWeapon];
				else
					return null;
			}
		}

		public Dictionary<string, Weapon> weapons = new Dictionary<string, Weapon>();
		protected Dictionary<string, Halo> halos = new Dictionary<string, Halo>();

		public override void Awake()
		{
			weapons.Add("oldSword", new OldStraightSword() { _thisBase = this.ThisBase });
			weapons.Add("oldGreatSword", new OldGreatSword() { _thisBase = this.ThisBase });
			weapons.Add("oldTwinSword", new OldTwinSword() { _thisBase = this.ThisBase });
			weapons.Add("oldSpear", new OldSpear() { _thisBase = this.ThisBase });
			weapons.Add("oldBow", new OldBow() { _thisBase = this.ThisBase });
			weapons.Add("taintedSword", new TaintedSword() { _thisBase = this.ThisBase });

			halos.Add("DirtyHalo", new DirtyHalo());
			halos.Add("EvilSpiritHalo", new EvilSpiritHalo());


			foreach (var value in weapons)
			{
				value.Value?.Awake();
			}
			foreach (var value in halos)
			{
				value.Value?.Awake();
			}
		}

		public override void Start()
		{
			foreach (var value in weapons)
			{
				value.Value?.Start();
			}
			foreach (var value in halos)
			{
				value.Value?.Start();
			}

			if (!isEnemy)
				CurrentWeapon?.ChangeKey();

			_currentHalo = "EvilSpiritHalo";
		}
		public override void Update()
		{
			CurrentWeapon?.Update();
			halos[_currentHalo].Update();
		}

		public int WeaponAnimation()
		{
			if (_currentWeapon == "oldSword")
				return 0;
			else if (_currentWeapon == "oldTwinSword")
				return 1;
			else
				return 0;
		}
	}
}
