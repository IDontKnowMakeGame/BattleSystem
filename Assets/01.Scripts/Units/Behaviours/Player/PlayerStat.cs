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
        public override void Update()
        {
            base.Update();
            if(Input.GetKeyDown(KeyCode.Q))
            {
                Die();
            }
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
