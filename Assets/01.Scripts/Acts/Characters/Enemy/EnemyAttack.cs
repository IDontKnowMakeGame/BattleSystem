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
        private ItemInfo _defaultStat => ThisActor.GetAct<CharacterEquipmentAct>().CurrentWeapon.WeaponInfo;

        public void DefaultAttack(Vector3 dir, bool isLast = false)
        {
            var map = Define.GetManager<MapManager>();
            map.AttackBlock(ThisActor.Position + dir, _defaultStat.Atk, _defaultStat.Ats, ThisActor, isLast);
            ThisActor.StartCoroutine(ResetAttackCoroutine());
        }

        public void ForwardAttack(Vector3 dir, bool isLast = false)
        {
            var character = ThisActor as CharacterActor;

            if (isLast)
            {
                character.AddState(CharacterState.Attack);
                ThisActor.StartCoroutine(ResetAttackCoroutine());
            }
            else
            {
                character.AddState(CharacterState.Hold);
                ThisActor.StartCoroutine(ResetHoldCoroutine());
            }


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
            if (isLast)
            {
                character.AddState(CharacterState.Attack);
                ThisActor.StartCoroutine(ResetAttackCoroutine());
            }
            else
            {
                character.AddState(CharacterState.Hold);
                ThisActor.StartCoroutine(ResetHoldCoroutine());
            }

            var map = Define.GetManager<MapManager>();

            for (var i = -1; i <= 1; i++)
            {
                for (var j = -1; j <= 1; j++)
                {
                    var attackPos = new Vector3(i, 0, j);
                    attackPos += ThisActor.Position;
                    map.AttackBlock(attackPos, _defaultStat.Atk, _defaultStat.Ats, ThisActor, isLast);
                }
            }
        }

        public void HalfAttack(Vector3 dir, bool isLast = false)
        {
            var character = ThisActor as CharacterActor;
            if (isLast)
            {
                character.AddState(CharacterState.Attack);
                ThisActor.StartCoroutine(ResetAttackCoroutine());
            }
            else
            {
                character.AddState(CharacterState.Hold);
                ThisActor.StartCoroutine(ResetHoldCoroutine());
            }

            var map = Define.GetManager<MapManager>();
            var originPos = ThisActor.Position;
            var nextPos = originPos + dir;
            var degree = originPos.GetDegree(nextPos);

            for (var i = -1; i <= 1; i++)
            {
                for (var j = 0; j <= 1; j++)
                {
                    if (i == 0 && j == 0)
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
            if (isLast)
            {
                character.AddState(CharacterState.Attack);
                ThisActor.StartCoroutine(ResetAttackCoroutine());
            }
            else
            {
                character.AddState(CharacterState.Hold);
                ThisActor.StartCoroutine(ResetHoldCoroutine());
            }

            ThisActor.StartCoroutine(BackAttackCoroutine(dir, isLast));
        }

        private IEnumerator BackAttackCoroutine(Vector3 dir, bool isLast = false)
        {
            var character = ThisActor as CharacterActor;
            var move = ThisActor.GetAct<CharacterMove>();
            ForwardAttack(dir);
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
            if (isLast == false)
                character.AddState(CharacterState.Hold);

            ThisActor.StartCoroutine(TripleAttackCoroutine(dir, isLast));
        }

        private IEnumerator TripleAttackCoroutine(Vector3 dir, bool isLast)
        {
            var characeter = ThisActor as CharacterActor;
            var statInfo = characeter.GetAct<CharacterEquipmentAct>().CurrentWeapon.WeaponInfo;
            var move = ThisActor.GetAct<CharacterMove>();
            yield return new WaitForSeconds(statInfo.Ats);
            ForwardAttack(dir);
            yield return new WaitUntil(() => !characeter.HasState(CharacterState.Hold));
            if (ThisActor.Position + dir != InGame.Player.Position)
            {
                move.Translate(dir);
                yield return new WaitUntil(() => !characeter.HasState(CharacterState.Move));
            }

            ForwardAttack(dir);
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
            if (isLast)
            {
                character.AddState(CharacterState.Attack);
                ThisActor.StartCoroutine(ResetAttackCoroutine());
            }
            else
            {
                character.AddState(CharacterState.Hold);
                ThisActor.StartCoroutine(ResetHoldCoroutine());
            }

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
                if (distance == 0)
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

            ThisActor.StartCoroutine(ResetAttackCoroutine());
            ThisActor.StartCoroutine(ResetHoldCoroutine());

        }

        private void ResetAttack()
        {
            var character = ThisActor as CharacterActor;
            if (character.HasState(CharacterState.Attack) == false)
                return;
            character.RemoveState(CharacterState.Attack);
        }

        private void ResetHold()
        {
            var character = ThisActor as CharacterActor;
            if (character.HasState(CharacterState.Hold) == false)
                return;
            character.RemoveState(CharacterState.Hold);
        }

        private IEnumerator ResetAttackCoroutine()
        {
            yield return new WaitForSeconds(5f);
            ResetAttack();
        }

        private IEnumerator ResetHoldCoroutine()
        {
            yield return new WaitForSeconds(5f);
            ResetHold();
        }
    }
}