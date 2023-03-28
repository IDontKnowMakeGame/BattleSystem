using System.Collections;
using Actors.Characters;
using Acts.Base;
using Blocks.Acts;
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

        public void DefaultAttack(MovementType shakeType = MovementType.None, bool isLast = false)
        {
            var character = ThisActor as CharacterActor;

            if (isLast)
            {
                character.AddState(CharacterState.Attack);
            }
            else
            {
                character.AddState(CharacterState.Hold);
            }
            var map = Define.GetManager<MapManager>();
            map.AttackBlock(ThisActor.Position, _defaultStat.Atk, _defaultStat.Ats, ThisActor, shakeType, isLast);
        }
        
        public void ForwardAttack(Vector3 dir, MovementType shakeType = MovementType.None, bool isLast = false)
        {
            var character = ThisActor as CharacterActor;

            if (isLast)
            {
                character.AddState(CharacterState.Attack);
            }
            else
            {
                character.AddState(CharacterState.Hold);
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
                map.AttackBlock(attackPos, _defaultStat.Atk, _defaultStat.Ats, ThisActor, shakeType, isLast);
            }
            DefaultAttack(MovementType.None, isLast);
        }

        public void RoundAttack(MovementType shakeType = MovementType.None, bool isLast = false)
        {
            var character = ThisActor as CharacterActor;
            if (isLast)
            {
                character.AddState(CharacterState.Attack);
            }
            else
            {
                character.AddState(CharacterState.Hold);
            }

            var map = Define.GetManager<MapManager>();

            for (var i = -1; i <= 1; i++)
            {
                for (var j = -1; j <= 1; j++)
                {
                    var attackPos = new Vector3(i, 0, j);
                    attackPos += ThisActor.Position;
                    if (i == 0 && j == 0)
                        map.AttackBlock(attackPos, _defaultStat.Atk, _defaultStat.Ats, ThisActor, MovementType.None, isLast);
                        
                    map.AttackBlock(attackPos, _defaultStat.Atk, _defaultStat.Ats, ThisActor, shakeType, isLast, 0.3f);
                }
            }
        }

        public void HalfAttack(Vector3 dir, MovementType shakeType = MovementType.None, bool isLast = false)
        {
            var character = ThisActor as CharacterActor;
            if (isLast)
            {
                character.AddState(CharacterState.Attack);
            }
            else
            {
                character.AddState(CharacterState.Hold);
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
                    map.AttackBlock(attackPos, _defaultStat.Atk, _defaultStat.Ats, ThisActor, shakeType, isLast);
                }
            }
            DefaultAttack(MovementType.None, isLast);
        }

        public void BackAttack(Vector3 dir, bool isLast = false)
        {
            var character = ThisActor as CharacterActor;
            if (isLast)
            {
                character.AddState(CharacterState.Attack);
            }
            else
            {
                character.AddState(CharacterState.Hold);
            }

            ThisActor.StartCoroutine(BackAttackCoroutine(dir, isLast));
        }

        private IEnumerator BackAttackCoroutine(Vector3 dir, bool isLast = false)
        {
            var character = ThisActor as CharacterActor;
            var move = ThisActor.GetAct<CharacterMove>();
            yield return new WaitUntil(() => !character.HasState(CharacterState.Move));
            yield return new WaitForSeconds(_defaultStat.Ats);
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
            RoundAttack(MovementType.Bounce, isLast);
        }

        public void TripleAttack(Vector3 dir, bool isLast = false)
        {
            var character = ThisActor as CharacterActor;
            if (isLast)
            {
                character.AddState(CharacterState.Attack);
            }
            else
            {
                character.AddState(CharacterState.Hold);
            }

            ThisActor.StartCoroutine(TripleAttackCoroutine(dir, isLast));
        }

        private IEnumerator TripleAttackCoroutine(Vector3 dir, bool isLast)
        {
            var characeter = ThisActor as CharacterActor;
            var statInfo = characeter.GetAct<CharacterEquipmentAct>().CurrentWeapon.WeaponInfo;
            var move = ThisActor.GetAct<CharacterMove>();
            yield return new WaitUntil(() => !characeter.HasState(CharacterState.Move));
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

            HalfAttack(dir, MovementType.None, isLast);
        }

        public void SoulAttack(Vector3 dir, bool isLast = false)
        {
            var character = ThisActor as CharacterActor;
            if (isLast)
            {
                character.AddState(CharacterState.Attack);
            }
            else
            {
                character.AddState(CharacterState.Hold);
            }

            ThisActor.StartCoroutine(SoulAttackCoroutine(dir, isLast));
        }

        private IEnumerator SoulAttackCoroutine(Vector3 dir, bool isLast)
        {
            var map = Define.GetManager<MapManager>();
            var character = ThisActor as CharacterActor;
            var move = ThisActor.GetAct<CharacterMove>();
            var degree = ThisActor.Position.GetDegree(InGame.Player.Position);
            yield return new WaitUntil(() => !character.HasState(CharacterState.Move));
            yield return new WaitForSeconds(_defaultStat.Ats);
            var distance = 3;
            var nextPos = InGame.Player.Position - dir * distance;
            
            while (map.IsWalkable(nextPos) == false)
            {
                distance--;
                nextPos = InGame.Player.Position - dir * distance;
                if (distance == 0)
                    break;
            }

            move.Jump(nextPos);
            yield return new WaitUntil(() => !character.HasState(CharacterState.Move));
            distance = 1;
            nextPos = ThisActor.Position + dir * distance;
            while (map.IsCheckFitBlock(nextPos, ThisActor))
            {
                distance++;
                nextPos = ThisActor.Position + dir * distance;

            }
            
            for (var vec = ThisActor.Position; vec != nextPos; vec += dir)
            {
                var absDir = new Vector3(Mathf.Abs(dir.x), 0, Mathf.Abs(dir.z));
                var quat = Quaternion.Euler(0, degree, 0);
                var leftVec = quat * Vector3.left;
                var rightVec = quat * Vector3.right;
                if (absDir is {x: > 0, z: > 0})
                {
                    leftVec = quat * new Vector3(-1, 0, -1);
                    leftVec.x = Mathf.RoundToInt(leftVec.x);
                    leftVec.z = Mathf.RoundToInt(leftVec.z);
                    rightVec = quat * new Vector3(1, 0, -1);
                    rightVec.x = Mathf.RoundToInt(rightVec.x);
                    rightVec.z = Mathf.RoundToInt(rightVec.z);
                }

                var leftPos = vec + leftVec;
                var rightPos = vec + rightVec;
                map.AttackBlock(vec, _defaultStat.Atk * 2,_defaultStat.Ats, ThisActor, MovementType.Shake);
                map.AttackBlock(leftPos, _defaultStat.Atk * 2, _defaultStat.Ats, ThisActor, MovementType.Shake);
                map.AttackBlock(rightPos, _defaultStat.Atk * 2, _defaultStat.Ats, ThisActor, MovementType.Shake);
            }
            
            yield return new WaitUntil(() => !character.HasState(CharacterState.Hold));

            yield return new WaitForSeconds(5f);
            
            character.RemoveState(CharacterState.Attack);
            character.RemoveState(CharacterState.Hold);
        }

    }
}