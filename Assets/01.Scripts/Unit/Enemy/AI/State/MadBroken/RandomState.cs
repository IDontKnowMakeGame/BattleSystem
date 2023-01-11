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
            toNormal.SetTarget(new NormalState());
            toNormal.AddCondition(() => randomValue == 0, true);
            
            AITransition toBack = new AITransition();
            toBack.SetConditionState(true);
            toBack.SetTarget(new BackAttackState());
            toNormal.AddCondition(() => randomValue == 1, true);
            
            
            AITransition toTriple = new AITransition();
            toBack.SetConditionState(true);
            toTriple.SetTarget(new TripleAttackState());
            toNormal.AddCondition(() => randomValue == 2, true);
            
            
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