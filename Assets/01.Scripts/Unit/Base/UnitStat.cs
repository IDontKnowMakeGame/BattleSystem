using System;
using UnityEngine;
using Manager;

namespace Unit
{
    [Serializable]
    public class UnitStat : Behaviour, IDamaged
    {
        [SerializeField] private BaseStat originalStat;

        [SerializeField] private BaseStat currentStat;  

        public BaseStat GetOriginalStat()
        {
            return originalStat;
        }

        public BaseStat GetCurrentStat()
        {
            return currentStat;
        }

        public override void Awake()
        {
            currentStat.hp = originalStat.hp;
            currentStat.atk = originalStat.atk;
            currentStat.agi = originalStat.agi;
            currentStat.ats = originalStat.ats;
            currentStat.def = originalStat.def;
        }

        public virtual void Damaged(float damage)
        {
            GameObject obj = GameManagement.Instance.GetManager<ResourceManagers>().Instantiate("Damage");
            obj.GetComponent<DamagePopUp>().DamageText((int)damage, this.thisBase.transform.position);
            currentStat.hp -= damage;
            if (currentStat.hp <= 0)
                Die();
        }

        public virtual void Die()
        {
            
        }
    }
}
