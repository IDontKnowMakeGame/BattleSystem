using Manager;
using Unit.Enemy.AI.Conditions;
using UnityEngine;

namespace Unit.Enemy.AI.State
{
    public class AttackState : AIState
    {
        public Vector3 direction;
        public float delay;
        public AttackState()
        {
            Name = "Attack";
        }

        public override void Awake()
        {
            AITransition toChase = new AITransition();
            toChase.SetConditionState(true, false);
            toChase.SetTarget(new ChaseState());
            var timeCondition = new TimeCheckCondition();
            delay = unit.GetBehaviour<UnitStat>().GetCurrentStat().ats;
            timeCondition.SetTime(delay);
            toChase.AddCondition(timeCondition.CheckCondition, true);
            AddTransition(toChase);

        }

        protected override void OnEnter()
        {
            Debug.Log(Name);
            var pos = GameObject.Find("Enemy").transform.position;
            if (direction.x != 0)
            {
                GameManagement.Instance.GetManager<MapManager>().GiveDamage(pos + direction + Vector3.forward, 1, delay);
                GameManagement.Instance.GetManager<MapManager>().GiveDamage(pos + direction, 1, delay);
                GameManagement.Instance.GetManager<MapManager>().GiveDamage(pos + direction + Vector3.back, 1, delay);
            }

            if (direction.z != 0)
            {
                GameManagement.Instance.GetManager<MapManager>().GiveDamage(pos + direction + Vector3.right, 1, delay);
                GameManagement.Instance.GetManager<MapManager>().GiveDamage(pos + direction, 1, delay);
                GameManagement.Instance.GetManager<MapManager>().GiveDamage(pos + direction + Vector3.left, 1, delay);
            }
        }
    }
}