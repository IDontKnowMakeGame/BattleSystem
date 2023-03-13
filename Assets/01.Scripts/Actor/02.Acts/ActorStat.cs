using System;
using Data;
using UnityEngine;

namespace Actor.Acts
{
    public class ActorStat : Act
    {
        [SerializeField]
        private BaseStat _currentStat;
        public BaseStat CurrentStat
        {
            get
            {
                var data = weaponInfo + haloInfo;
                var stat = new BaseStat
                {
                    MaxHp = data.Hp,
                    Atk = data.Atk,
                    Ats = data.Ats,
                    Afs = data.Afs,
                    Weight = data.Weight
                };
                _currentStat = stat;
                return stat;
            }
        }
        public ItemInfo weaponInfo;
        public ItemInfo haloInfo;

        private void Start()
        {
            Debug.Log(CurrentStat);
        }
    }
}