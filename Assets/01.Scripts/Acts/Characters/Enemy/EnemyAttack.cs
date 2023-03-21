using System.Collections;
using Actors.Characters;
using Acts.Base;
using Core;
using Data;
using Managements.Managers;
using UnityEngine;

namespace Acts.Characters.Enemy
{
    public class EnemyAttack : Act
    {
        private ItemInfo _defaultStat = null;
        public override void Start()
        {
            _defaultStat = ThisActor.GetAct<CharacterEquipmentAct>().CurrentWeapon.WeaponInfo;
        }

        public void DefaultAttack(Vector3 dir, bool isLast = false)
        {
            var map = Define.GetManager<MapManager>();
            map.AttackBlock(ThisActor.Position + dir, _defaultStat.Atk, _defaultStat.Ats, ThisActor, isLast);
        }

        public void ForwardAttak(Vector3 dir, bool isLast = false)
        {
            var character = ThisActor as CharacterActor;
            character.AddState(CharacterState.Attack);
            if(isLast == false)
                character.AddState(CharacterState.Hold);
            
            var map = Define.GetManager<MapManager>();
            var originPos = ThisActor.Position;
            var nextPos = originPos + dir;
            var degree = originPos.GetDegree(nextPos);
            for (var i = -1; i <= 1; i++)
            {
                var attackPos = new Vector3(i, 0, 1);
                attackPos = attackPos.Rotate(degree);
                attackPos += originPos;
                map.AttackBlock(attackPos, _defaultStat.Atk, _defaultStat.Ats, ThisActor, isLast);
            }
        }

        public void RoundAttack(bool isLast = false)
        {
            var character = ThisActor as CharacterActor;
            character.AddState(CharacterState.Attack);
            if(isLast == false)
                character.AddState(CharacterState.Hold);
            
            for (var i = -1; i <= 1; i++)
            {
                for (var j = -1; j <= 1; j++)
                {
                    var attackPos = new Vector3(i, 0, j);
                    attackPos += ThisActor.Position;
                    var map = Define.GetManager<MapManager>();
                    map.AttackBlock(attackPos, _defaultStat.Atk, _defaultStat.Ats, ThisActor, isLast);
                }
            }
        }
        
        public void BackAttack(Vector3 dir, bool isLast = false)
        {
            var character = ThisActor as CharacterActor;
            character.AddState(CharacterState.Attack);
            if(isLast == false)
                character.AddState(CharacterState.Hold);
            
            ThisActor.StartCoroutine(BackAttackCoroutine(dir, isLast));
        }

        private IEnumerator BackAttackCoroutine(Vector3 dir, bool isLast = false)
        {
            var character = ThisActor as CharacterActor;
            var statInfo = character.GetAct<CharacterEquipmentAct>().CurrentWeapon.WeaponInfo;
            var move = ThisActor.GetAct<CharacterMove>();
            ForwardAttak(dir);
            yield return new WaitUntil(() => !character.HasState(CharacterState.Hold));
            move.Translate(-dir);
            yield return new WaitUntil(() => !character.HasState(CharacterState.Move));
            yield return new WaitForSeconds(statInfo.Afs);
            if (ThisActor.Position.IsInBox(InGame.Player.Position, 3) == false)
            {
                character.RemoveState(CharacterState.Attack);
                yield break;
            }

            
            move.Jump(InGame.Player.Position);
            yield return new WaitUntil(() => !character.HasState(CharacterState.Move));
            RoundAttack(true);
        }

    }
}