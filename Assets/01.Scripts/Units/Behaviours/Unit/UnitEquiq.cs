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
		//All Info of Item
		public static Dictionary<int, Item> items = new Dictionary<int, Item>()
		{
			{0, new OldStraightSword()},
			{1, new OldTwinSword()}
		};
		#region Weapon
		//Now CurrentWeapon & SecoundWeapon
		[SerializeField]
		protected int _currentWeapon;
		[SerializeField]
		protected int _secoundWeapon;

		//Select Enemy or Player
		public bool isEnemy = true;

		//Null or CurrentWeapon Return, if this UnitEquiq isEnemy true & Null Create current Weapon return currentWeapon
		public Weapon CurrentWeapon
		{
			get
			{
				if ("" != _currentWeapon && null != _currentWeapon)
				{
					Weapon weapon = null;
					if (weapons.TryGetValue(_currentWeapon, out weapon))
					{
						return weapons[_currentWeapon];
				}
				else
				{
					if (isEnemy)
					{
						Type type = Type.GetType(_currentWeapon);
							if (type == null)
								return weapon;
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
				if ("" != _currentWeapon && null != _currentWeapon)
					return weapons[_secoundWeapon];
				else
					return null;
			}
		}
		#endregion

		#region Halo
		protected Dictionary<string, Halo> halos = new Dictionary<string, Halo>();
		private int _haloCount = 2;
		public Halo[] UseHalo => usingHalos;
		protected Halo[] usingHalos = new Halo[3];
		#endregion
		public override void Awake()
		{
			//weapons. 

			//halos.Add("DirtyHalo", new DirtyHalo());
			//halos.Add("EvilSpiritHalo", new EvilSpiritHalo());


			foreach (var value in items)
			{
				value.Value?.Awake();
			}
			//foreach (var value in halos)
			//{
			//	value.Value?.Awake();
			//}
		}

		public override void Start()
		{
			foreach (var value in items)
			{
				value.Value?.Start();
			}
			//foreach (var value in halos)
			//{
			//	value.Value?.Start();
			//}
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
		public virtual void InsertWeapon(string name, Weapon type)
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
			if (_currentWeapon == "OldStraightSword")
				return 0;
			else if (_currentWeapon == "OldTwinSword")
				return 1;
			else if (_currentWeapon == "OldGreatSword")
				return 2;
			else if (_currentWeapon == "OldSpear")
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
