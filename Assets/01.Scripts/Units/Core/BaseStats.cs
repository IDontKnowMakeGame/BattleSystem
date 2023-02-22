using System;

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
            Atk = stats.Atk;
            Agi = stats.Agi;
        }
    }

    [Serializable]
    public class WeaponStats
    {
        public float Atk = 0f;
        public float Ats = 0f;
        public float Afs = 0f;
        public int Weight = 0;

  //      public static WeaponStats operator +(WeaponStats a, WeaponStats b)
		//{
  //          return new WeaponStats() { Atk = a.Atk + b.Atk, Afs = a.Afs + b.Afs, Ats = a.Ats + b.Ats, Weight = a.Weight + b.Weight };
		//}
    }
}