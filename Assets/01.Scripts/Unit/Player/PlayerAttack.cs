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
            if (timer <= 0)
            {
                isAttak = true;
                thisBase.transform.localScale = new Vector3(dir == Vector3.left ? -1 : dir == Vector3.right ? 1 : thisBase.transform.localScale.x, 1, 1);
                thisBase.GetBehaviour<PlayerMove>().ClearMove();
                thisBase.GetBehaviour<PlayerAnimation>().DoAttack();
                if (GameManagement.Instance.GetManager<MapManager>().GetBlock(thisBase.transform.position + dir)?.GetUnit() != null)
                {
                    thisBase.StartCoroutine(GameManagement.Instance.GetManager<CameraManager>().CameraShaking(3, 0.1f, 0.3f));
                    if (playerStats != null) playerStats.AddAdrenaline(1);
                }
                timer = afterTime;
				GameManagement.Instance.GetManager<MapManager>().GiveDamage<UnitStat>(thisBase.transform.position+dir, damage, time, AttackEnd);
                onBehaviourEnd?.Invoke();
            }
        }

        public void AttackEnd()
		{
            isAttak = false;
		}
        public void DoAttack(Vector3 dir)
		{
            onBehaviourEnd?.Invoke();
        }
    }
}
