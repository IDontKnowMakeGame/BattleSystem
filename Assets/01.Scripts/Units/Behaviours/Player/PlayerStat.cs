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
            base.ChangeStats();
			changeStats.Atk *= _playerBuff.Stat.Atk;
			changeStats.Agi *= _playerBuff.Stat.Atk;
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
