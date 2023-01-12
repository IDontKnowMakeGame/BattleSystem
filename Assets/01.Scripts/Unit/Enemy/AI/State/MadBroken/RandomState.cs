using UnityEngine;

namespace Unit.Enemy.AI.MadBroken.State
{
    public class RandomState : AIState
    {
        private int randomValue = -1;
        private Vector3 _attackDireciton;

        private NormalState normal = null;
        private BackAttackState back = null;
        private TripleAttackState triple = null;
        public RandomState()
        {
            Name = "Random";
        }

        public override void Awake()
        {
            AITransition toNormal = new AITransition();
            toNormal.SetConditionState(true);
            normal = new NormalState();
            toNormal.SetTarget(normal);
            toNormal.AddCondition(() => randomValue == 0, true);
            AddTransition(toNormal);
            
            AITransition toBack = new AITransition();
            toBack.SetConditionState(true);
            back = new BackAttackState();
            toBack.SetTarget(back);
            toBack.AddCondition(() => randomValue == 1, true);
            AddTransition(toBack);
            
            AITransition toTriple = new AITransition();
            toTriple.SetConditionState(true);
            triple = new TripleAttackState();
            toTriple.SetTarget(triple);
            toTriple.AddCondition(() => randomValue == 2, true);
            AddTransition(toTriple);
            
        }

        protected override void OnEnter()
        {
            randomValue = Random.Range(0, 3);
            randomValue = 1;
            Debug.Log(Name);
            Debug.Log(_attackDireciton);
        }

        protected override void OnExit()
        {
            normal.SetAttackDirection(_attackDireciton);
            back.SetAttackDirection(_attackDireciton);
            triple.SetAttackDirection(_attackDireciton);
        }

        public void SetAttackDirection(Vector3 direction)
        {
            _attackDireciton = direction;
        }
    }
}