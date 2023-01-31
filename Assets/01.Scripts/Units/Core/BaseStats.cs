﻿using System;

namespace Unit.Core
{
    [Serializable]
    public class UnitStats
    {
        public float Hp = 0f;
        public float Atk = 0f;
        public float Agi = 0f;

        public void Set(UnitStats stats)
        {
            Hp = stats.Hp;
            Atk = stats.Atk;//히히 이스터에그
            Agi = stats.Agi; //히히 이스터에그
        }
    }

    [Serializable]
    public class WeaponStats
    {
        public float Atk = 0f;
        public float Ats = 0f;
        public float Afs = 0f;
        public int Weight = 0;
    }
}