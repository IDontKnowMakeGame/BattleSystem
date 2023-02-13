using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Units.Behaviours.Unit;
using UnityEngine.SceneManagement;
using DG.Tweening;
using Unit.Core;
using Units.Base.Unit;

namespace Units.Base.Player
{
    [System.Serializable]
    public class PlayerStat : UnitStat
    {
        [SerializeField]
        private Shake DamageShake;

        public UnitStats addstat = new UnitStats { Agi = 0, Atk = 0, Hp = 0 };
        public UnitStats multistat = new UnitStats { Agi = 1, Atk = 1, Hp = 1 };
        public override void Start()
		{
			base.Start();
        }
		public override void Update()
        {
            base.Update();
        }

        public override void Damaged(float damage, UnitBase giveUnit)
        {
            base.Damaged(damage, giveUnit);

            EventParam param = new EventParam();
            param.intParam = 0;
            Define.GetManager<EventManager>().TriggerEvent(EventFlag.


        }

		protected override void ChangeStats()
		{
            int Weight = 3;
            float Atk = OriginStats.Atk;

            if (_unitEquiq.CurrentWeapon != null)
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

        private void PlusStat()
		{

		}

        private void MultiStat()
		{

		}

        public override void Die()
        {
            base.Die();
            DOTween.KillAll();
            SceneManager.LoadScene(SceneManager.GetActiveScene().name);
            ThisBase.GetBehaviour<PlayerMove>().SpawnSetting();
        }

    }
}
