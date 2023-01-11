using System.Collections;
using System.Collections.Generic;
using Unit;
using UnityEngine;
using System;
using Manager;

namespace Unit.Player
{
    [System.Serializable]
    public class PlayerAttack : UnitAttack
    {
        [SerializeField]
        private float Delay;

        private float timer;

        private PlayerStats playerStats;

        public override void Start()
        {
            timer = Delay;
            playerStats = thisBase?.GetBehaviour<PlayerStats>();
        }

        public override void Update()
        {
            if (timer > 0)
                timer -= Time.deltaTime;
        }

        public override void Attack(Vector3 dir, float time)
        {
            if(timer <= 0)
            {
                Vector3 checkPos = thisBase.transform.position + dir;
                if (playerStats != null && GameManagement.Instance.GetManager<MapManager>().BlockInUnit(checkPos))      playerStats.AddAdrenaline(1);
                GameManagement.Instance.GetManager<MapManager>().GiveDamage<UnitStat>(checkPos, playerStats.GetCurrentStat().atk, time);
                timer = Delay;
                onBehaviourEnd?.Invoke();
            }
        }

        public void DoAttack(Vector3 dir)
		{
            onBehaviourEnd?.Invoke();
        }
    }
}
