using System;
using UnityEngine;

namespace Unit
{
    [Serializable]
    public class UnitStat : Behaviour
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
            currentStat = originalStat;
        }
    }
}