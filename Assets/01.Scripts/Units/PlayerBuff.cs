using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Units.Behaviours.Unit;

namespace Units.Base.Player
{
    [System.Serializable]
    public class PlayerBuff : UnitBehaviour
    {
        [SerializeField]
        [Range(0, 10)] private int anger;
        [SerializeField]
        [Range(0, 10)] private int adneraline;

        public void ChangeAnger(int percent)
        {
            anger += percent;
        }

        public void ChangeAdneraline(int percent)
        {
            adneraline += percent;
        }
    }
}
