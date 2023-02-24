using System;
using Core;
using Unit.Core;
using UnityEngine;
using Behaviour = Units.Behaviours.Base.Behaviour;
using Units.Base.Player;
using Units.Base.Unit;
using Random = UnityEngine.Random;

namespace Units.Behaviours.Unit
{
    [Serializable]
    public class UnitStat : UnitBehaviour,IDamaged
    {
        [SerializeField] protected UnitStats originStats = null;
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

		public UnitStats addstat = new UnitStats { Agi = 0, Atk = 0, Hp = 0 };
		public UnitStats multistat = new UnitStats { Agi = 1, Atk = 1, Hp = 1 };

		protected UnitEquiq _unitEquiq;
		public override void Awake()
		{
			_unitEquiq = ThisBase.GetBehaviour<UnitEquiq>();
			changeStats.Set(OriginStats);
			base.Awake();
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

			Weight += (int)addstat.Agi;
			Atk += addstat.Atk;

			Weight *= (int)multistat.Agi;
			Atk *= multistat.Atk;

			changeStats.Agi = WeightToSpeed(Weight);
			changeStats.Atk = Atk;
		}

		protected float WeightToSpeed(int a) => a switch
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

		protected float SpeedToWeight(float a) => a switch
		{
			0.1f => 1,
			0.2f => 2,
			0.23f =>3,
			0.25f =>4,
			0.3f =>5 ,
			0.5f =>6 ,
			0.7f =>7 ,
			0.8f =>8 ,
			0.9f =>9,
			_ => 0.1f
		};

		public virtual void Damaged(float damage, UnitBase giveUnit)
		{
			ThisBase.GetBehaviour<CharacterRender>().DamageRender();
			float half = Half / 100;

			changeStats.Hp -= damage - damage * half;
			if(changeStats.Hp <= 0)
			{
				Die();
				return;
			}


			if (Core.InGame.BossBase == giveUnit)
            {
				EventParam haloParam = new EventParam();
				haloParam.unitParam = giveUnit;
				Core.Define.GetManager<EventManager>().TriggerEvent(EventFlag.DirtyHalo, haloParam);
			}

			onBehaviourEnd?.Invoke();
		}

		public void ChangeHP()
        {
			//EventParam param = new EventParam();
			//param.floatParam = changeStats.Hp / originStats.Hp;

			//EventParam param2 = new EventParam();
			//param2.floatParam = originStats.Hp;

			//Define.GetManager<EventManager>().TriggerEvent(EventFlag.AddPlayerHP, param);
			//Define.GetManager<EventManager>().TriggerEvent(EventFlag.HPWidth, param2);
		}

		public virtual void Die()
		{

		}
	}
}