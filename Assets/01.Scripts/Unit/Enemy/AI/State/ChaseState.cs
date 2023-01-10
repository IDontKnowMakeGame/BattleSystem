﻿using System.Collections;
using Manager;  
using Unit.Enemy.AI.Conditions;
using Unit.Enemy.Base;
using Unit.Player;
using UnityEngine;
using UnityEngine.UIElements;

namespace Unit.Enemy.AI.State
{
    public class ChaseState : AIState   
    {
        private AttackState attack;
        private LineCheckCondition lineCheck; 
        private Astar pathfinding;
        private bool isChasing;
        public ChaseState()
        {
            Name = "Chase";
        }
        public override void Awake()
        {
            AITransition toRoaming = new AITransition();
            toRoaming.SetConditionState(true, false);
            toRoaming.SetTarget(new RoamingState());
            var rangeCondition = new RangeCheckCondition();
            rangeCondition.SetRange(3);
            rangeCondition.SetPos(GameObject.Find("Enemy").transform, GameObject.Find("Player").transform);
            toRoaming.AddCondition(rangeCondition.CheckCondition, false);
            AddTransition(toRoaming);
            
            AITransition toAttack = new AITransition();
            attack = new AttackState();
            toAttack.SetConditionState(true, false);
            toAttack.SetTarget(attack);
            lineCheck = new LineCheckCondition();
            lineCheck.SetLength(1);
            lineCheck.SetPos(GameObject.Find("Enemy").transform, GameObject.Find("Player").transform);
            toAttack.AddCondition(lineCheck.CheckCondition, true);
            AddTransition(toAttack);

            pathfinding = new Astar();
        }

        protected override void OnEnter()
        {
            Debug.Log("Chase");
        }

        protected override void OnStay()
        {
            if (!isChasing)
            {
                unit.StartCoroutine(ChaseCoroutine());
            }
        }

        protected override void OnExit()
        {
            attack.direction = lineCheck.GetDirection();
        }

        private IEnumerator ChaseCoroutine()
        {
            isChasing = true;
            var map = GameManagement.Instance.GetManager<MapManager>();
            var start = map.GetBlock(GameObject.Find("Enemy").transform.position);
            var end = map.GetBlock(Core.Define.PlayerBase.GetBehaviour<PlayerMove>().position);
            pathfinding.SetRoute(start, end);   

            unit.StartCoroutine(pathfinding.FindPath());

            yield return new WaitUntil(() => pathfinding.HasFound());
                Debug.Log("start chase");
            var path = pathfinding.GetNextPath().GetUnit().transform.position;
            Debug.Log(path);
            unit.GetBehaviour<EnemyMove>().Translate(path);
            yield return new WaitUntil(() => path == unit.transform.position);

            isChasing = false;
        }
    }
}