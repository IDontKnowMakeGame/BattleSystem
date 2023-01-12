using System.Collections;
using Manager;
using Unit.Enemy.AI.Conditions;
using Unit.Enemy.AI.ElderBroken.State;
using Unit.Enemy.Base;
using Unit.Player;
using UnityEngine;

namespace Unit.Enemy.AI.MadBroken.State
{
    public class ChaseState : AIState   
    {
        private RandomState random;
        private LineCheckCondition lineCheck;
        private Astar pathfinding;
        public ChaseState()
        {
            Name = "Chase";
        }
        public override void Awake()
        {
            AITransition toRandom = new AITransition();
            toRandom.SetConditionState(true, false);
            random = new RandomState();
            toRandom.SetTarget(random);
            lineCheck = new LineCheckCondition();
            lineCheck.SetLength(1);
            lineCheck.SetPos(unit.transform, Core.Define.PlayerBase.transform);
            toRandom.AddCondition(lineCheck.CheckCondition, true);
            toRandom.AddCondition(() => !unit.GetBehaviour<EnemyMove>().IsMoving(), true);
            
            AddTransition(toRandom);
            
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
            random.SetAttackDirection(lineCheck.GetDirection());
        }
    }
}