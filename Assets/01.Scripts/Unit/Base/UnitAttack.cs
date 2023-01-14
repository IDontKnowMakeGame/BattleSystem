using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Manager;
using System;

namespace Unit
{
    public class UnitAttack : Behaviour
    {
        public bool IsAttack => isAttak;
        protected bool isAttak;
        public virtual void Attack(Vector3 dir, float time, float atferTime,float damage = 0)
        {
            
        }

        public virtual void WaitAttack(Vector3 dir, float damage,float time, Action action = null)
		{
            GameManagement.Instance.GetManager<MapManager>().GiveDamage<UnitStat>(dir, damage, time,action);
        }
    }
}
