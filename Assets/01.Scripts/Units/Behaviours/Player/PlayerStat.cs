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
                Debug.Log(_unitEquiq.CurrentWeapon.WeaponStat);
                Weight = _unitEquiq.CurrentWeapon.WeaponStat.Weight;
                Atk = _unitEquiq.CurrentWeapon.WeaponStat.Atk;
            }

            Weight -= (int)_playerBuff.Stat.Agi;
            Atk *= _playerBuff.Stat.Atk;

            changeStats.Agi = WeightToSpeed(Weight);
            changeStats.Atk = Atk;
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
