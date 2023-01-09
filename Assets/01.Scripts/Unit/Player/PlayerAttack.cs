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

        public Action onAttackEnd;
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

        public override void Attack(Vector3 dir)
        {
            if(timer <= 0)
            {
                Debug.Log("PAttack");
                if (playerStats != null) playerStats.AddAdrenaline(1);
                timer = Delay;
                GameManagement.Instance.GetManager<MapManager>().GiveDamage(thisBase.transform.position+dir, playerStats.GetCurrentStat().atk, 0);
                onAttackEnd?.Invoke();
            }
        }

        public void DoAttack(Vector3 dir)
		{
            onAttackEnd?.Invoke();
        }
    }
}
