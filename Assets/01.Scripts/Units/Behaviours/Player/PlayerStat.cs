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
            
            ThisBase.GetBehaviour<PlayerBuff>().ChangeAnger(1);
            ThisBase.GetBehaviour<PlayerItem>().PlayerPortion.ResetPortion();

            float value = (changeStats.Hp / originStats.Hp) * 100f;
            Core.Define.GetManager<UIManager>().SetHpValue((int)value);

            EventParam param = new EventParam();
            param.intParam = 0;
            Core.Define.GetManager<EventManager>().TriggerEvent(EventFlag.PlayTimeLine, param);
        }

        public void AddHP(float addVal)
        {
            if (originStats.Hp <= changeStats.Hp) return;

            changeStats.Hp += addVal;

            float value = (changeStats.Hp / originStats.Hp) * 100;

            Core.Define.GetManager<UIManager>().SetHpValue((int)value);
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

            Core.Define.GetManager<UIManager>().SetMaxHpValue((int)(OriginStats.Hp/10));
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
            ThisBase.SpawnSetting();
            ThisBase.GetBehaviour<PlayerMove>().ResetMove();
            Respawn();
        }

        public void Respawn()
        {
            SceneManager.LoadScene("Lobby");
        }
    }
}
