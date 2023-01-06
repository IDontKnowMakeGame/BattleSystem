using Manager;
using Unit.Enemy.AI.Conditions;
using UnityEngine;

namespace Unit.Enemy.AI.State
{
    public class RoamingState : AIState
    {
        public RoamingState()
        {
            Name = "Roaming";
        }
        public override void Awake()
        {
            AITransition toChase = new AITransition();
            toChase.SetConditionState(true, false);
            toChase.SetTarget(new ChaseState());
            var rangeCondition = new RangeCheckCondition();
            rangeCondition.SetRange(1);
            rangeCondition.SetPos(GameObject.Find("Enemy").transform, GameObject.Find("Player").transform);
            toChase.AddCondition(rangeCondition.CheckCondition, true);
            AddTransition(toChase);
        }

        protected override void OnEnter()
        {
            Debug.Log(Name);
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
                    if (Mathf.FloorToInt(Vector3.Distance(pos, blockPos)) <= 1)
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
        }
    }
}