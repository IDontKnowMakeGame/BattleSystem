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

		public Weapon SecoundWeapon
		{
			get
			{
				if (_secoundWeapon != null && _secoundWeapon != "")
					return weapons[_secoundWeapon];
				else
					return null;
			}
		}

		public Dictionary<string, Weapon> weapons = new Dictionary<string, Weapon>();
		protected Dictionary<string, Halo> halos = new Dictionary<string, Halo>();

		private int _haloCount = 2;

		public Halo[] UseHalo => usingHalos;
		protected Halo[] usingHalos = new Halo[3];
		public override void Awake()
		{
			weapons.Add("oldSword", new OldStraightSword() { _thisBase = this.ThisBase });
			weapons.Add("oldGreatSword", new OldGreatSword() { _thisBase = this.ThisBase });
			weapons.Add("oldTwinSword", new OldTwinSword() { _thisBase = this.ThisBase });
			weapons.Add("oldSpear", new OldSpear() { _thisBase = this.ThisBase });
			weapons.Add("oldBow", new OldBow() { _thisBase = this.ThisBase });
			weapons.Add("taintedSword", new TaintedSword() { _thisBase = this.ThisBase });
			weapons.Add("brokenSword", new BrokenSword() { _thisBase = this.ThisBase });

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

			for(int i = 0; i < _haloCount; i++)
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
			if (_currentWeapon == "oldSword")
				return 0;
			else if (_currentWeapon == "oldTwinSword")
				return 1;
			else if (_currentWeapon == "oldGreatSword")
				return 2;
			else if (_currentWeapon == "oldSpear")
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
