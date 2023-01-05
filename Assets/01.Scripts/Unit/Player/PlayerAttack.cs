using System.Collections;
using System.Collections.Generic;
using Unit;
using UnityEngine;
using System;

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

            if(Input.GetKeyDown(KeyCode.Z))
            {
                Attack(Vector3.zero);
            }
        }

        protected override void Attack(Vector3 dir)
        {
            if(timer <= 0)
            {
                if (playerStats != null) playerStats.AddAdrenaline(1);
                Debug.Log("����");
                timer = Delay;
                onAttackEnd?.Invoke();
            }
        }

        public void DoAttack(Vector3 dir)
		{
            Debug.Log("����");
            onAttackEnd?.Invoke();
        }
    }
}
