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
			base.Start();
			_unitEquiq = ThisBase.GetBehaviour<UnitEquiq>();
			changeStats = originStats;
		}

		protected virtual void ChangeStats()
		{
			int Weight = 0;
			float Atk = 0;

			Weight = _unitEquiq.CurrentWeapon.WeaponStat.Weight;
			Atk = _unitEquiq.CurrentWeapon.WeaponStat.Atk;

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
			2 => 0.4f,
			3 => 0.5f,
			4 => 0.7f,
			5 => 1f,
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