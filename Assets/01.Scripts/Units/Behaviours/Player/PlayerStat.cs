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

        public override void Update()
        {
            base.Update();
        }

        public override void Damaged(float damage)
        {
            base.Damaged(damage);
            DamageShake.ScreenShake(Vector3.one);
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
