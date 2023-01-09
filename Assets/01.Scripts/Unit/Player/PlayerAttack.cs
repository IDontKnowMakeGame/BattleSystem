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
        private InputManager _inputManager;

        public override void Start()
        {
            _inputManager = GameManagement.Instance.GetManager<InputManager>();
            timer = Delay;
            playerStats = thisBase?.GetBehaviour<PlayerStats>();
        }

        public override void Update()
        {
            if (timer > 0)
                timer -= Time.deltaTime;

            AttackCheck();
        }

        protected override void Attack(Vector3 dir)
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

        public void AttackCheck()
		{
            if (_inputManager.GetKeyDownInput(InputManager.InputSignal.FowardAttack))
			{
                Attack(Vector3.forward);
            }
            if (_inputManager.GetKeyDownInput(InputManager.InputSignal.BackwardAttack))
            {
                Attack(Vector3.back);
            }
            if (_inputManager.GetKeyDownInput(InputManager.InputSignal.LeftAttack))
            {
                Attack(Vector3.left);
            }
            if (_inputManager.GetKeyDownInput(InputManager.InputSignal.RightAttack))
            {
                Attack(Vector3.right);
            }
        }
    }
}
