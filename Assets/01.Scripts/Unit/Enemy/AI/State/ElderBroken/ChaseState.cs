using System.Collections;
using Manager;  
using Unit.Enemy.AI.Conditions;
using Unit.Enemy.Base;
using Unit.Player;
using UnityEngine;
using UnityEngine.UIElements;

namespace Unit.Enemy.AI.ElderBroken.State
{
    public class ChaseState : AIState   
    {
        private AttackState attack;
        private LineCheckCondition lineCheck; 
        private Astar pathfinding;
        public ChaseState()
        {
            Name = "Chase";
        }
        public override void Awake()
        {
            AITransition toRoaming = new AITransition();
            toRoaming.SetConditionState(true, true);
            toRoaming.SetTarget(new RoamingState());
            var rangeCondition = new RangeCheckCondition();
            rangeCondition.SetRange(3);
            rangeCondition.SetPos(unit.transform, Core.Define.PlayerBase.transform);
            toRoaming.AddCondition(rangeCondition.CheckCondition, false);
            toRoaming.AddCondition(() => !pathfinding.IsChasing(), true);
            AddTransition(toRoaming);
            
            AITransition toAttack = new AITransition();
            attack = new AttackState();
            toAttack.SetConditionState(true, false);
            toAttack.SetTarget(attack);
            lineCheck = new LineCheckCondition();
            lineCheck.SetLength(1);
            lineCheck.SetPos(unit.transform, Core.Define.PlayerBase.transform);
            toAttack.AddCondition(lineCheck.CheckCondition, true);      
            toAttack.AddCondition(() => !pathfinding.IsChasing(), true);
            AddTransition(toAttack);

            pathfinding = new Astar();
            pathfinding.SetUnit(unit);
        }

        protected override void OnEnter()
        {
            Debug.Log("Chase");
        }

        protected override void OnStay()
        {
            if (!pathfinding.IsChasing())
            {
                var map = GameManagement.Instance.GetManager<MapManager>();
                var player = map.GetBlock(Core.Define.PlayerBase.GetBehaviour<UnitMove>().position);
                var my = map.GetBlock(unit.GetBehaviour<UnitMove>().position);
                pathfinding.SetRoute(my, player);
                unit.StartCoroutine(pathfinding.ChaseCoroutine());
            }
        }

        protected override void OnExit()
        {
            attack.direction = lineCheck.GetDirection();
        }
    }
}