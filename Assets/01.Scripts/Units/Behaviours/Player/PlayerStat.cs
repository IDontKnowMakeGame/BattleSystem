using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Units.Behaviours.Unit;
using UnityEngine.SceneManagement;
using DG.Tweening;

namespace Units.Base.Player
{
    [System.Serializable]
    public class PlayerStat : UnitStat
    {
        [SerializeField]
        private Shake DamageShake;

        private PlayerBuff _playerBuff;

		public override void Start()
		{
			base.Start();
            _playerBuff = ThisBase.GetBehaviour<PlayerBuff>();
        }
		public override void Update()
        {
            base.Update();
        }

        public override void Damaged(float damage)
        {
            base.Damaged(damage);
            DamageShake.ScreenShake(new EventParam());
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

            foreach (var a in _unitEquiq._helos)
            {
                Weight += (int)a.addstat.Agi;
                Atk += a.addstat.Atk;

                Weight *= (int)a.addstat.Agi;
                Atk *= a.addstat.Atk;
            }

            Weight += (int)_playerBuff.addstat.Agi;
            Atk += _playerBuff.addstat.Atk;

            Weight *= (int)_playerBuff.multistat.Agi;
            Atk *= _playerBuff.multistat.Atk;

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
