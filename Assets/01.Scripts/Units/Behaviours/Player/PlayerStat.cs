using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Units.Behaviours.Unit;
using UnityEngine.SceneManagement;
using DG.Tweening;
using Unit.Core;

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

        public override void Damaged(float damage)
        {
            base.Damaged(damage);
            //DamageShake.ScreenShake(new EventParam());
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
