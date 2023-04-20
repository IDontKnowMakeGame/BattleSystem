﻿using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Actors.Bases;
using Actors.Characters;
using Actors.Characters.Enemy;
using Actors.Characters.Player;
using Acts.Base;
using Acts.Characters.Enemy;
using Core;
using DG.Tweening;
using Managements.Managers;
using UnityEngine;
using UnityEngine.Animations;
using Random = UnityEngine.Random;

namespace Acts.Characters
{
    public class CharacterMove : Act
    {
        private Transform _thisTransform;
        public static event Action<int, Vector3> OnMoveEnd = null;
        protected bool _isMoving = false;
        private Vector3 dir;
        private CharacterActor _character => ThisActor as CharacterActor;

        [SerializeField]
        private float defaultSpeed = 0;

        private IEnumerator _positionUpdateCoroutine;

        // Test Code
        protected bool enableQ = false;

        public override void Awake()
        {
            _thisTransform = ThisActor.transform;
            
        }

        public virtual void Translate(Vector3 direction)
        {
            var nextPos = ThisActor.Position + direction;
            Move(nextPos);
        }

        public virtual void BackStep(Vector3 direction)
        {
            if (_isMoving) return;
            var seq = DOTween.Sequence();
            var currentPos = ThisActor.Position;
            var nextPos = ThisActor.Position + direction;
            nextPos.y = 1;
            

            var map = Define.GetManager<MapManager>();

            if (!map.IsStayable(nextPos.SetY(0)))
            {
                MoveStop();
                return;
            }
            
            var block = map.GetBlock(nextPos.SetY(0));
            if(block.CheckActorOnBlock(ThisActor) == false) return;
            _character.AddState(Actors.Characters.CharacterState.Move);

            dir = (currentPos - nextPos).SetY(0);
            MoveBackAnimation();

            var speed = _character.GetAct<CharacterStatAct>().ChangeStat.speed;
            seq.Append(_thisTransform.DOMove(nextPos, speed - defaultSpeed).SetEase(Ease.Linear));
            seq.AppendCallback(() =>
            {
                OnMoveEnd?.Invoke(ThisActor.UUID, nextPos - _character.Position);
                MoveStop(); 
                seq.Kill();
            });
        }

        private void MoveBackAnimation()
        {
            dir = dir.normalized;
            var animation = ThisActor.GetAct<EnemyAnimation>();
            Debug.Log(dir);
            if (dir == Vector3.forward)
            {
                animation.Play("LowerMove");
            }

            if (dir == Vector3.back)
            {
                animation.Play("UpperMove");
            }

            if (dir == Vector3.left)
            {
                ThisActor.SpriteTransform.localScale = new Vector3(-3, 3, 3);
                animation.Play("HorizontalBackStep");
            }

            if (dir == Vector3.right)
            {
                ThisActor.SpriteTransform.localScale = new Vector3(3, 3,3);
                animation.Play("HorizontalBackStep");   
            }
        }

        public virtual void Move(Vector3 position)
        {
/*            var attackState = CharacterState.Attack | CharacterState.Hold;
            if (_character.HasState(attackState))
            {
                Debug.Log("attackState");
                _character.RemoveState(CharacterState.Move);
                return;
            }*/
            
            if (_isMoving)
            {
                enableQ = false;
                return;
            }
            var seq = DOTween.Sequence();
            var currentPos = ThisActor.Position;
            var nextPos = position;
            nextPos.y = 1;
            

            var map = Define.GetManager<MapManager>();

            if (!map.IsStayable(nextPos.SetY(0)))
            {
                enableQ = false;
                MoveStop();
                return;
            }
            
            var block = map.GetBlock(nextPos.SetY(0));
            if(block.CheckActorOnBlock(ThisActor) == false)
            {
                enableQ = false;
                return;
            }

            if (_character.HasState(CharacterState.Move) == false)
                _character.AddState(Actors.Characters.CharacterState.Move);
            else return;

            dir = (currentPos - nextPos).SetY(0);
            AnimationCheck();

            var speed = _character.GetAct<CharacterStatAct>().ChangeStat.speed;
            seq.Append(_thisTransform.DOMove(nextPos, speed - defaultSpeed).SetEase(Ease.Linear));
            seq.InsertCallback((speed - defaultSpeed) / 2, () => enableQ = false);
            seq.AppendCallback(() =>
            {
				OnMoveEnd?.Invoke(ThisActor.UUID, position - _character.Position);
                MoveStop(); 
				seq.Kill();
            });
        }

        public virtual void Jump(Vector3 targetPos, Vector3 dir, int distance)
        {
            if (_isMoving) return;
            var seq = DOTween.Sequence();
            int i = distance;
            var nextPos = targetPos + dir * distance;
            while (i > 0)
            {
                nextPos = targetPos + dir * i;
                var nextBlock = InGame.GetBlock(nextPos.SetY(0));
                if(nextBlock != null)
                        break;
                i--;
            }

            nextPos.y = 1;
            var map = Define.GetManager<MapManager>();

            if (map.GetBlock(nextPos.SetY(0)).IsActorOnBlock)
            {
                
            }
            else if (!map.IsStayable(nextPos.SetY(0)))
            {
                Debug.Log(1);
                return;
            }

            var block = map.GetBlock(nextPos.SetY(0));
            if(block.CheckActorOnBlock(ThisActor) == false) return;
            _character.AddState(Actors.Characters.CharacterState.Move);

            var speed = _character.GetAct<CharacterStatAct>().ChangeStat.speed;
            seq.Append(_thisTransform.DOJump(nextPos, 1, 1, speed));
            seq.AppendCallback(() =>
            {
                map.GetBlock(nextPos.SetY(0)).SetActorOnBlock(ThisActor);
                MoveStop();
                seq.Kill();
            });
        }

        private bool isChasing = false;
        public void Chase(Actor target)
        {
            if(_character.HasState(CharacterState.Move)) return;
            if (isChasing) return;
            ThisActor.StartCoroutine(AstarCoroutine(target.Position));
        }

        Astar astar = new Astar();
        private IEnumerator AstarCoroutine(Vector3 end)
        {
            isChasing = true;
            if (InGame.GetBlock(end).isWalkable == false)
            {
                yield break;
            }
            astar.SetPath(ThisActor.Position, end);
            ThisActor.StartCoroutine(astar.FindPath());
            yield return new WaitUntil(astar.IsFinished);
            var nextBlock = astar.GetNextPath();
            if (nextBlock == null)
            {
                yield break;
            }
            var nextPos = nextBlock.Position;
            Move(nextPos);
        }
        protected virtual void AnimationCheck()
        {
            dir = dir.normalized;
            var animation = ThisActor.GetAct<EnemyAnimation>();
            if (dir == Vector3.forward)
            {
                animation.Play("UpperMove");
            }

            if (dir == Vector3.back)
            {
                animation.Play("LowerMove");
            }

            if (dir == Vector3.left)
            {
                ThisActor.SpriteTransform.localScale = new Vector3(3, 3,3);
                animation.Play("HorizontalMove");
            }

            if (dir == Vector3.right)
            {
                ThisActor.SpriteTransform.localScale = new Vector3(-3, 3, 3);
                animation.Play("HorizontalMove");   
            }
        }

        public void KnockBack()
        {
            var map = Define.GetManager<MapManager>();
            var dirs = new[] { Vector3.forward, Vector3.back, Vector3.left, Vector3.right };
            dirs.Where((v) =>
            {
                var pos = ThisActor.Position + v;
                return map.IsStayable(pos);
            });
            var dir = dirs[Random.Range(0, dirs.Length)];
            _character.AddState(CharacterState.KnockBack);
            var originPos = ThisActor.Position;
            var nextPos = originPos + dir;
            nextPos.y = 1;
            var seq = DOTween.Sequence();
            seq.Append(_thisTransform.DOMove(nextPos, 0.1f).SetEase(Ease.Flash));
            seq.AppendCallback(() =>
            {
                _character.RemoveState(CharacterState.KnockBack);
                map.GetBlock(nextPos.SetY(0)).SetActorOnBlock(ThisActor);
                seq.Kill();
            });
        }

        protected virtual void MoveStop()
        {
            _character.RemoveState(CharacterState.Move);
        }
    }
}