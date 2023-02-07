using System;
using Unit.Core;
using UnityEngine;
using Behaviour = Units.Behaviours.Base.Behaviour;
using Units.Base.Player;

namespace Units.Behaviours.Unit
{
    [Serializable]
    public class UnitStat : UnitBehaviour,IDamaged
    {
        [SerializeField] private UnitStats originStats = null;
        [SerializeField] protected UnitStats changeStats = null;
		public UnitStats OriginStats => originStats;
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
			changeStats.Set(OriginStats);
			base.Start();
		}

		protected virtual void ChangeStats()
		{
			int Weight = 3;
			float Atk = originStats.Atk;

			if(_unitEquiq.CurrentWeapon != null)
			{
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
			1 => 0.1f,
			2 => 0.2f,
			3 => 0.23f,
			4 => 0.25f,
			5 => 0.3f,
			6 => 0.5f,
			7 => 0.7f,
			8 => 0.8f,
			9 => 0.9f,
			_ => 0.1f
		};

		public virtual void Damaged(float damage)
		{

			float half = Half / 100;

			changeStats.Hp -= damage - damage * half;
			if(changeStats.Hp <= 0)
			{
				Die();
				return;
			}

			EventParam param = new EventParam();
			param.floatParam = changeStats.Hp / originStats.Hp;
			if(Core.InGame.BossBase == this.ThisBase)
            {
				Core.Define.GetManager<EventManager>().TriggerEvent(EventFlag.AddBossHP, param);
            }
			if(Core.InGame.PlayerBase == ThisBase)
            {
				ThisBase.GetBehaviour<PlayerBuff>().ChangeAnger(2);
				Core.Define.GetManager<EventManager>().TriggerEvent(EventFlag.AddPlayerHP, param);

			}

		}

		public virtual void Die()
		{

		}
	}
}