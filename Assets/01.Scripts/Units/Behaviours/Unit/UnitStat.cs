using System;
using Unit.Core;
using UnityEngine;
using Behaviour = Units.Behaviours.Base.Behaviour;

namespace Units.Behaviours.Unit
{
    [Serializable]
    public class UnitStat : UnitBehaviour,IDamaged
    {
        [SerializeField] private UnitStats originStats = null;
        [SerializeField] private UnitStats changeStats = null;

		public UnitStats NowStats {
			get
			{
				ChangeStats();
				return changeStats;
			}
		}

		public float Half { get; set; } = 0;

		private UnitEquiq _unitEquiq;
		public override void Start()
		{
			_unitEquiq = ThisBase.GetBehaviour<UnitEquiq>();
			base.Start();
			changeStats = originStats;
		}

		protected virtual void ChangeStats()
		{
			int Weight = 3;
			float Atk = originStats.Atk;

			if(_unitEquiq.CurrentWeapon != null)
			{
				Debug.Log(_unitEquiq.CurrentWeapon);
				Debug.Log(_unitEquiq);
				Debug.Log(_unitEquiq.CurrentWeapon.WeaponStat);
				Weight = _unitEquiq.CurrentWeapon.WeaponStat.Weight;
				Atk = _unitEquiq.CurrentWeapon.WeaponStat.Atk;
			}

			foreach(var a in _unitEquiq._helos)
			{
				//헤일로에 능력에 따라 무언가를 해준다.

			}

			changeStats.Agi = WeightToSpeed(Weight);
			changeStats.Atk = Atk;
		}

		private float WeightToSpeed(int a) => a switch
		{
			1 => 0.2f,
			2 => 0.25f,
			3 => 0.3f,
			4 => 0.35f,
			5 => 0.45f,
			6 => 0.5f,
			7 => 0.7f,
			8 => 0.8f,
			9 => 0.9f,
			_ => 0.1f
		};

		public void Damaged(float damage)
		{

			float half = Half / 100;

			changeStats.Hp -= damage - damage * half;
			Half = 0;
			if(changeStats.Hp <= 0)
			{
				Die();
				return;
			}
		}

		public virtual void Die()
		{

		}
	}
}