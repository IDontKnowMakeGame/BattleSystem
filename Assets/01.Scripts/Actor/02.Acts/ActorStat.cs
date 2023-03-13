using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unit.Core;
using System;

namespace Actor.Acts
{
	public class ActorStat : Act
	{
		[SerializeField] protected ActorStats originStats = new ActorStats();
		[SerializeField] protected ActorStats changeStats = new ActorStats();
		public ActorStats OriginStats => originStats;
		public ActorStats NowStats
		{
			get
			{
				ChangeStats();
				return changeStats;
			}
		}
		public float Half { get; set; } = 0;

		public Action afterDieAction;

		public ActorStats addstat = new ActorStats { Agi = 0, Atk = 0, Hp = 0 };
		public ActorStats multistat = new ActorStats { Agi = 1, Atk = 1, Hp = 1 };

		protected override void Awake()
		{
			changeStats.Set(OriginStats);
			base.Awake();
		}
		protected virtual void ChangeStats()
		{
			int Weight = 3;
			float Atk = originStats.Atk;

			/*			if (_unitEquiq.CurrentWeapon != null)
						{
							Weight = _unitEquiq.CurrentWeapon.WeaponStat.Weight;
							Atk = _unitEquiq.CurrentWeapon.WeaponStat.Atk;
						}*/

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
			0.23f => 3,
			0.25f => 4,
			0.3f => 5,
			0.5f => 6,
			0.7f => 7,
			0.8f => 8,
			0.9f => 9,
			_ => 0.1f
		};

		/*		public virtual void Damaged(float damage, UnitBase giveUnit)
				{
					ThisBase.GetBehaviour<CharacterRender>().DamageRender();
					float half = Half / 100;

					changeStats.Hp -= damage - damage * half;
					if (changeStats.Hp <= 0)
					{
						giveUnit.GetBehaviour<UnitEquiq>().KillCount();
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
				}*/
	}
}
