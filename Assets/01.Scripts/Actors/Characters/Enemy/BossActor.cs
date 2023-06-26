using Core;
using UnityEngine;
using UnityEngine.Events;

namespace Actors.Characters.Enemy
{
    public class BossActor : EnemyActor
    {
        public UnityEvent OnRevive = new UnityEvent();
        protected override void Update()
        {
            if (InGame.Player == null)
            {
                return;
            }
            base.Update();
        }

        public void Revive()
        {
            RemoveState(CharacterState.Die);
            OnRevive?.Invoke();
            
        }
    }
}