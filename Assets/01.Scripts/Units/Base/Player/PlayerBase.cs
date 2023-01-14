using Units.Base.Unit;
using Units.Behaviours.Unit;
using UnityEngine;

namespace Units.Base.Player
{
    public class PlayerBase : UnitBase
    {
        protected override void Init()
        {
            AddBehaviour(thisStat);
            AddBehaviour<UnitMove>().MoveTo(Vector3.zero);
        }
    }
}