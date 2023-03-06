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
		protected Weapons _currentWeapon;
		[SerializeField]
		protected Weapons _secoundWeapon;

		public bool isEnemy;
		public Weapon CurrentWeapon
		{
			get
			{
				if (Weapons.Empty != _currentWeapon)
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
				if (_secoundWeapon != Weapons.Empty)
					return weapons[_secoundWeapon];
				else
					return null;
			}
		}

		public Dictionary<Weapons, Weapon> weapons = new Dictionary<Weapons, Weapon>();
		protected Dictionary<string, Halo> halos = new Dictionary<string, Halo>();

		private int _haloCount = 2;

		public Halo[] UseHalo => usingHalos;
		protected Halo[] usingHalos = new Halo[3];
		public override void Awake()
		{
			weapons.Add(Weapons.OldStraightSword, new OldStraightSword() { _thisBase = this.ThisBase });
			weapons.Add(Weapons.OldTwinSword, new OldTwinSword() { _thisBase = this.ThisBase });
			weapons.Add(Weapons.OldBow, new OldBow() { _thisBase = this.ThisBase });
			weapons.Add(Weapons.TaintedSword, new TaintedSword() { _thisBase = this.ThisBase });
			weapons.Add(Weapons.OldGreatSword, new OldGreatSword() { _thisBase = this.ThisBase });

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
		public virtual void InsertWeapon(Weapons name, Weapon type)
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
			if (_currentWeapon == Weapons.OldStraightSword)
				return 0;
			else if (_currentWeapon == Weapons.OldTwinSword)
				return 1;
			else if (_currentWeapon == Weapons.OldGreatSword)
				return 2;
			else if (_currentWeapon == Weapons.OldSpear)
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
