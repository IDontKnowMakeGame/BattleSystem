using System.Collections.Generic;
using Unit.Core.Weapon;
using System;
using UnityEngine;
using Unit.Core;

namespace Units.Behaviours.Unit
{
	[Serializable]
	public class UnitEquiq : UnitItem
	{
		//All Info of Item
		#region Weapon
		//Now CurrentWeapon & SecoundWeapon
		[SerializeField]
		protected ItemID _currentWeapon;
		[SerializeField]
		protected ItemID _secoundWeapon;

		//Select Enemy or Player
		public bool isEnemy = true;

		//Null or CurrentWeapon Return, if this UnitEquiq isEnemy true & Null Create current Weapon return currentWeapon
		public Weapon CurrentWeapon
		{
			get
			{
				if (_currentWeapon != ItemID.None)
				{
					Item item;
					Weapon weapon = null;
					items.TryGetValue(_currentWeapon, out item);
					weapon = item as Weapon;
					return weapon;
				}
				else
					return null;
			}
		}
		public Weapon SecoundWeapon
		{
			get
			{
				if (_secoundWeapon != ItemID.None)
				{
					Item item;
					Weapon weapon = null;
					items.TryGetValue(_secoundWeapon, out item);
					weapon = item as Weapon;
					return weapon;
				}
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
			base.Awake();
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
		//public virtual void InsertHelo(ItemID name, int idx)
		//{
		//	EraseHelo(idx);

		//	usingHalos[idx] = halos[name];
		//	usingHalos[idx].Init();
		//}

		//public virtual void EraseHelo(int idx)
		//{
		//	if (usingHalos[idx] != null)
		//		usingHalos[idx].Exit();

		//	usingHalos[idx] = null;
		//}

		public int WeaponAnimation()
		{
			if (_currentWeapon == ItemID.OldGreatSword)
				return 0;
			else if (_currentWeapon == ItemID.OldTwinSword)
				return 1;
			else if (_currentWeapon == ItemID.OldGreatSword)
				return 2;
			else if (_currentWeapon == ItemID.OldSpear)
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
