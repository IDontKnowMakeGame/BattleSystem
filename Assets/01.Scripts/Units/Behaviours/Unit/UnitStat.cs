using System;
using Unit.Core;
using UnityEngine;
using Behaviour = Units.Behaviours.Base.Behaviour;

namespace Units.Behaviours.Unit
{
    [Serializable]
    public class UnitStat : UnitBehaviour
    {
        [SerializeField] private UnitStats originStats = null;
        [SerializeField] private UnitStats changeStats = null;

		public UnitStats NowStats => changeStats;

		private UnitEquiq _unitEquiq;
		public override void Start()
		{
			base.Start();
			_unitEquiq = ThisBase.GetBehaviour<UnitEquiq>();
		}

		protected virtual void ChangeStats()
		{
			changeStats.Agi = WeightToSpeed(_unitEquiq.CurrentWeapon.WeaponStat.Weight);
			changeStats.Atk = _unitEquiq.CurrentWeapon.WeaponStat.Atk;
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
	}
}