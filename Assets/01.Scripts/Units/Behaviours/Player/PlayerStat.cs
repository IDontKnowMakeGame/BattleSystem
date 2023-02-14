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
            Core.Define.GetManager<EventManager>().TriggerEvent(EventFlag.PlayTimeLine, param);
        }

        public void AddHP(float addVal)
        {
            if (originStats.Hp <= changeStats.Hp) return;

            changeStats.Hp += addVal;

            EventParam param = new EventParam();
            param.floatParam = changeStats.Hp / originStats.Hp;

            Core.Define.GetManager<EventManager>().TriggerEvent(EventFlag.AddPlayerHP, param);
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
