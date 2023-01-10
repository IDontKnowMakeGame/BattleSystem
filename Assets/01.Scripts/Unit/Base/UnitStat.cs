using System;
using UnityEngine;

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

        public override void Start()
        {
            currentStat.hp = originalStat.hp;
            currentStat.atk = originalStat.atk;
            currentStat.agi = originalStat.agi;
            currentStat.ats = originalStat.ats;
            currentStat.def = originalStat.def;
        }

        public virtual void Damaged(float damage)
        {
            Debug.Log(">");
            currentStat.hp -= damage;
            if (currentStat.hp <= 0)
                Die();
        }

        public virtual void Die()
        {
            
        }
    }
}
