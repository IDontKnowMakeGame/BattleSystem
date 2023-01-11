using UnityEngine;

namespace Unit.Enemy.AI.MadBroken.State
{
    public class RandomState : AIState
    {
        private int randomValue = -1;
        private Vector3 _attackDireciton;
        public RandomState()
        {
            Name = "Random";
        }

        public override void Awake()
        {
            AITransition toNormal = new AITransition();
            toNormal.SetConditionState(true);
            var normal = new NormalState();
            //normal.
            toNormal.SetTarget(normal);
            toNormal.AddCondition(() => randomValue == 0, true);
            AddTransition(toNormal);
            
            AITransition toBack = new AITransition();
            toBack.SetConditionState(true);
            var back = new BackAttackState();
            //back.
            toBack.SetTarget(back);
            toBack.AddCondition(() => randomValue == 1, true);
            AddTransition(toBack);
            
            AITransition toTriple = new AITransition();
            toTriple.SetConditionState(true);
            var triple = new TripleAttackState();
            //triple.
            toTriple.SetTarget(triple);
            toTriple.AddCondition(() => randomValue == 2, true);
            AddTransition(toTriple);
            
        }

        protected override void OnEnter()
        {
            randomValue = Random.Range(0, 3);
            Debug.Log(Name);
            Debug.Log(_attackDireciton);
        }
        
        public void SetAttackDirection(Vector3 direction)
        {
            _attackDireciton = direction;
        }
    }
}