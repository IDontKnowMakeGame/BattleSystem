using System.Collections;
using Actors.Characters;
using Acts.Base;
using Core;
using Data;
using Managements.Managers;
using Palmmedia.ReportGenerator.Core;
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

        public void HalfAttack(Vector3 dir, bool isLast = false)
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
                for(var j = 0; j <= 1; j++)
                {
                    if(i == 0 && j == 0)
                        continue;
                    var attackPos = new Vector3(i, 0, j);
                    attackPos = attackPos.Rotate(degree);
                    attackPos += originPos;
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
            var move = ThisActor.GetAct<CharacterMove>();
            ForwardAttak(dir);
            yield return new WaitUntil(() => !character.HasState(CharacterState.Hold));
            move.Translate(-dir);
            yield return new WaitUntil(() => !character.HasState(CharacterState.Move));
            yield return new WaitForSeconds(_defaultStat.Afs);
            if (ThisActor.Position.IsInBox(InGame.Player.Position, 3) == false)
            {
                character.RemoveState(CharacterState.Attack);
                yield break;
            }

            
            move.Jump(InGame.Player.Position + dir);
            yield return new WaitUntil(() => !character.HasState(CharacterState.Move));
            RoundAttack(isLast);
        }

        public void TripleAttack(Vector3 dir, bool isLast = false)
        {
            var character = ThisActor as CharacterActor;
            character.AddState(CharacterState.Attack);
            if(isLast == false)
                character.AddState(CharacterState.Hold);
            
            ThisActor.StartCoroutine(TripleAttackCoroutine(dir, isLast));
        }
        
        private IEnumerator TripleAttackCoroutine(Vector3 dir, bool isLast)
        {
            var characeter = ThisActor as CharacterActor;
            var statInfo = characeter.GetAct<CharacterEquipmentAct>().CurrentWeapon.WeaponInfo;
            var move = ThisActor.GetAct<CharacterMove>();
            yield return new WaitForSeconds(statInfo.Ats);
            ForwardAttak(dir);
            yield return new WaitUntil(() => !characeter.HasState(CharacterState.Hold));
            if (ThisActor.Position + dir != InGame.Player.Position)
            {
                move.Translate(dir);
                yield return new WaitUntil(() => !characeter.HasState(CharacterState.Move));
            }
            ForwardAttak(dir);
            yield return new WaitUntil(() => !characeter.HasState(CharacterState.Hold));
            if (ThisActor.Position + dir != InGame.Player.Position)
            {
                move.Translate(dir);
                yield return new WaitUntil(() => !characeter.HasState(CharacterState.Move));
            }
            HalfAttack(dir, isLast);
        }
        
        public void SoulAttack(Vector3 dir, bool isLast = false)
        {
            var character = ThisActor as CharacterActor;
            character.AddState(CharacterState.Attack);
            if(isLast == false)
                character.AddState(CharacterState.Hold);
            
            ThisActor.StartCoroutine(SoulAttackCoroutine(dir, isLast));
        }

        private IEnumerator SoulAttackCoroutine(Vector3 dir, bool isLast)
        {
            var map = Define.GetManager<MapManager>();
            var characeter = ThisActor as CharacterActor;
            var move = ThisActor.GetAct<CharacterMove>();
            yield return new WaitForSeconds(_defaultStat.Ats);
            var distance = 3;
            var nextPos = ThisActor.Position - dir * distance;
            while (map.IsWalkable(nextPos) == false)
            {
                distance--;
                nextPos = ThisActor.Position - dir * distance;
                if(distance == 0)
                    break;
            }
            move.Jump(nextPos);
            yield return new WaitUntil(() => !characeter.HasState(CharacterState.Move));
            distance = 1;
            nextPos = ThisActor.Position + dir * distance;
            while (map.IsWalkable(nextPos) == true)
            {
                distance++;
                nextPos = ThisActor.Position + dir * distance;
            }


            for (var vec = ThisActor.Position; vec != nextPos; vec += dir)
            {
                Debug.Log(vec);
                map.AttackBlock(vec, _defaultStat.Atk, _defaultStat.Ats, ThisActor, isLast);
            }
        }
    }
}