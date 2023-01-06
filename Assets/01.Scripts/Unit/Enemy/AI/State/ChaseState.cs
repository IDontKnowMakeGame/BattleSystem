﻿using Manager;
using Unit.Enemy.AI.Conditions;
using UnityEngine;

namespace Unit.Enemy.AI.State
{
    public class ChaseState : AIState
    {
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
        }

        protected override void OnEnter()
        {
            Debug.Log("Chase");
        }

        protected override void OnStay()
        {
            var pos = GameObject.Find("Enemy").transform.position;
            pos.y = 0;
            for (var i = -6; i <= 6; i++)
            {
                for (var j = -6; j <= 6; j++)
                {
                    var blockPos = pos + new Vector3(i, 0, j);
                    var block = GameManagement.Instance.GetManager<MapManager>().GetBlock(blockPos);
                    if (Mathf.FloorToInt(Vector3.Distance(pos, blockPos)) <= 3)
                    {
                        block.GetComponent<MeshRenderer>().material.color = Color.green;
                    }
                }
            }
        }

        protected override void OnExit()
        {
            var pos = GameObject.Find("Enemy").transform.position;
            pos.y = 0;
            for (var i = -6; i <= 6; i++)
            {
                for (var j = -6; j <= 6; j++)
                {
                    var blockPos = pos + new Vector3(i, 0, j);
                    var block = GameManagement.Instance.GetManager<MapManager>().GetBlock(blockPos);
                    block.GetComponent<MeshRenderer>().material.color = Color.white;
                }
            }
        }
    }
}