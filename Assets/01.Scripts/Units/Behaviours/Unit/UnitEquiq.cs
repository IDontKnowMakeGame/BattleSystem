using System.Collections.Generic;
using Unit.Core.Weapon;
using System;
using UnityEngine;
using Unit.Core;

namespace Units.Behaviours.Unit
{
	public enum WeaponEnum
	{
		Empty,
		OldStraightSword,
		OldTwinSword,
		OldGreatSword,
		OldSpear,
		OldBow,
		TaintedSword,
		End
	}
	[Serializable]
	public class UnitEquiq : UnitBehaviour
	{
		[SerializeField]
		protected WeaponEnum _currentWeapon;
		[SerializeField]
		protected WeaponEnum _secoundWeapon;

		public bool isEnemy;
		public Weapon CurrentWeapon
		{
			get
			{
				if (WeaponEnum.Empty != _currentWeapon)
				{
					Weapon weapon;
					if (weapons.TryGetValue(_currentWeapon, out weapon))
					{
						return weapons[_currentWeapon];
				}
				else
				{
					if (isEnemy)
					{
						Type type = Type.GetType(_currentWeapon.ToString());
						Weapon weaponClass = Activator.CreateInstance(type) as Weapon;
						weaponClass._thisBase = ThisBase;
						InsertWeapon(_currentWeapon, weaponClass);
						weapon = weapons[_currentWeapon];
					}

					return weapon;
				}
			}
				else
					return null;
			}
		}
		public Weapon SecoundWeapon
		{
			get
			{
				if (_secoundWeapon != WeaponEnum.Empty)
					return weapons[_secoundWeapon];
				else
					return null;
			}
		}

		public Dictionary<WeaponEnum, Weapon> weapons = new Dictionary<WeaponEnum, Weapon>();
		protected Dictionary<string, Halo> halos = new Dictionary<string, Halo>();

		private int _haloCount = 2;

		public Halo[] UseHalo => usingHalos;
		protected Halo[] usingHalos = new Halo[3];
		public override void Awake()
		{
			weapons.Add(WeaponEnum.OldStraightSword, new OldStraightSword() { _thisBase = this.ThisBase });
			weapons.Add(WeaponEnum.OldTwinSword, new OldTwinSword() { _thisBase = this.ThisBase });
			weapons.Add(WeaponEnum.OldBow, new OldBow() { _thisBase = this.ThisBase });
			weapons.Add(WeaponEnum.TaintedSword, new TaintedSword() { _thisBase = this.ThisBase });
			weapons.Add(WeaponEnum.OldGreatSword, new OldGreatSword() { _thisBase = this.ThisBase });

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
		}
		public override void Update()
		{
			CurrentWeapon?.Update();

			for (int i = 0; i < _haloCount; i++)
			{
				if (usingHalos[i] != null)
					usingHalos[i].Update();
			}
		}

		public override void OnDestroy()
		{
			foreach (var value in halos)
			{
				value.Value?.OnDestroy();
			}
		}

		public override void OnApplicationQuit()
		{
			foreach (var value in halos)
			{
				value.Value?.OnApplicationQuit();
			}
		}
		public virtual void InsertWeapon(WeaponEnum name, Weapon type)
		{
			weapons.Add(name, type);
			weapons[name].Awake();
			weapons[name].Start();
		}
		public virtual void InsertHelo(string name, int idx)
		{
			EraseHelo(idx);

			usingHalos[idx] = halos[name];
			usingHalos[idx].Init();
		}

		public virtual void EraseHelo(int idx)
		{
			if (usingHalos[idx] != null)
				usingHalos[idx].Exit();

			usingHalos[idx] = null;
		}

		public int WeaponAnimation()
		{
			if (_currentWeapon == WeaponEnum.OldStraightSword)
				return 0;
			else if (_currentWeapon == WeaponEnum.OldTwinSword)
				return 1;
			else if (_currentWeapon == WeaponEnum.OldGreatSword)
				return 2;
			else if (_currentWeapon == WeaponEnum.OldSpear)
				return 3;
			else
				return 0;
		}

		public void KillCount()
		{
			CurrentWeapon.KillEnemy();
		}
	}
}
