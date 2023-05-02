using Core;
using UnityEngine;

namespace Actors.Characters.Enemy
{
    public class BossActor : EnemyActor
    {
        protected override void Update()
        {
            if (InGame.Player == null)
            {
                return;
            }
            base.Update();
        }
    }
}