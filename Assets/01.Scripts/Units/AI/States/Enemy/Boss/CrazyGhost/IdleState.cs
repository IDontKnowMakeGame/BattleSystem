using Core;
using Managements.Managers;
using Unit.Base.AI;
using Unit.Enemy.AI.Conditions;
using UnityEngine;

namespace Units.AI.States.Enemy.Boss.CrazyGhost
{
    public class IdleState : AIState
    {
        private TimeCheckCondition timeCheck;
        public override void Awake()
        {
            var toChase = new AITransition();
            timeCheck = new TimeCheckCondition();
            timeCheck.SetResult(true);
            timeCheck.SetTime(2);
            toChase.AddCondition(timeCheck);
            toChase.SetTarget(new ChaseState());
            timeCheck._logicCondition = true;
            var areaCheck = new AreaCheckCondition();
            areaCheck.SetUnits(InGame.PlayerBase, null);
            areaCheck.SetArea(Define.GetManager<FloorManager>().CurrentFloor.BossArea[0], Define.GetManager<FloorManager>().CurrentFloor.BossArea[1]);
            areaCheck.SetResult(true);
            areaCheck._logicCondition = true;
            toChase.AddCondition(areaCheck);  
            AddTransition(toChase);
        }

        protected override void OnEnter()
        {
            Debug.Log("IdleState");
        }

        protected override void OnExit()
        {
            timeCheck.ResetTime();
        }
    }
}