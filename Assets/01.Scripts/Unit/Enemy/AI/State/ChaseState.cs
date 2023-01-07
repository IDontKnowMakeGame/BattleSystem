using Manager;
using Unit.Enemy.AI.Conditions;
using UnityEngine;

namespace Unit.Enemy.AI.State
{
    public class ChaseState : AIState
    {
        private AttackState attack;
        private LineCheckCondition lineCheck; 
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
            
            for (var i = -1; i <= 1; i++)
            {
                for (var j = -1; j <= 1; j++)
                {
                    if(i == 0 && j == 0) continue;
                    if(i != 0 && j != 0) continue;
                    
                    var blockPos = pos + new Vector3(i, 0, j);
                    var block = GameManagement.Instance.GetManager<MapManager>().GetBlock(blockPos);
                    if (Mathf.FloorToInt(Vector3.Distance(pos, blockPos)) <= 3)
                    {
                        block.GetComponent<MeshRenderer>().material.color = Color.red;
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
            attack.direction = lineCheck.GetDirection();
        }
    }
}