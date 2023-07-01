using System.Collections;
using Actors.Characters;
using Actors.Characters.Enemy;
using Core;
using UnityEngine;

namespace Acts.Characters.Enemy.Boss.KnightStatue
{
    public class KnightStatueAttack : EnemyAttack
    {
        public void JumpAttack(int range, bool isLast = true)
        {
            ThisActor.StartCoroutine(JumpAttackCoroutine(range, isLast));
        }

        public IEnumerator JumpAttackCoroutine(int range, bool isLast = true)
        {
            var move = ThisActor.GetAct<CharacterMove>();
            var animation = ThisActor.GetAct<EnemyAnimation>();
            var character = ThisActor as EnemyActor;
            character.AddState(CharacterState.Attack);
            character.canKnockBack = true;
            animation.Play("JumpReady");
            
            animation.curClip.OnExit += () => Define.GetManager<SoundManager>().PlayAtPoint("Boss/KnightStatue/Jump", ThisActor.Position, 1);
            move.Stamp(InGame.Player.Position, Vector3.zero, 0, 3f);
            yield return new WaitUntil(() => character != null && !character.HasState(CharacterState.Move));
            
            character.AttackWithNoReady(Vector3.zero, "Jump", () =>
            {
                RoundAttack(range, isLast);
                character.canKnockBack = false;
            });
        }
    }
}