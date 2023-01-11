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

        public override void Attack(Vector3 dir, float time, float afterTime,float damage = 0)
        {
            if(timer <= 0)
            {
                if (playerStats != null) playerStats.AddAdrenaline(1);
                timer = afterTime;
				GameManagement.Instance.GetManager<MapManager>().GiveDamage<UnitStat>(thisBase.transform.position+dir, damage, time);
                onBehaviourEnd?.Invoke();
            }
        }

        public void DoAttack(Vector3 dir)
		{
            onBehaviourEnd?.Invoke();
        }
    }
}
