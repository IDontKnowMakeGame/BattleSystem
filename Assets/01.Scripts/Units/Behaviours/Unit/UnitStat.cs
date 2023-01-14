using System;
using Unit.Core;
using UnityEngine;
using Behaviour = Units.Behaviours.Base.Behaviour;

namespace Units.Behaviours.Unit
{
    [Serializable]
    public class UnitStat : Behaviour
    {
        [SerializeField] private UnitStats unitStats = null;
    }
}