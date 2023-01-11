using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Manager;

namespace Unit
{
    public class UnitAttack : Behaviour
    {
        public virtual void Attack(Vector3 dir, float time)
        {

        }

        public virtual void WaitAttack(Vector3 dir, float damage,float time)
		{
            GameManagement.Instance.GetManager<MapManager>().GiveDamage<UnitStat>(thisBase.transform.position + dir, damage, time);
        }
    }
}
