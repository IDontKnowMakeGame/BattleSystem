﻿using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using Actors.Bases;
using Actors.Characters;
using Acts.Base;
using Acts.Characters.Enemy;
using Core;
using DG.Tweening;
using Managements.Managers;
using UnityEngine;
using UnityEngine.Animations;

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

            PositionUpdate(nextPos);
            var speed = _character.GetAct<CharacterStatAct>().ChangeStat.speed;
            seq.Append(_thisTransform.DOMove(nextPos, speed - defaultSpeed).SetEase(Ease.Linear));
            seq.AppendCallback(() =>
            {
                OnMoveEnd?.Invoke(ThisActor.UUID, nextPos - _character.Position);
                ThisActor.Position = nextPos;
                _isMoving = false;
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
            
            if (_isMoving) return;
            var seq = DOTween.Sequence();
            var currentPos = ThisActor.Position;
            var nextPos = position;
            nextPos.y = 1;
            
            Debug.Log("Move");

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
            AnimationCheck();

            PositionUpdate(nextPos);
            var speed = _character.GetAct<CharacterStatAct>().ChangeStat.speed;
            seq.Append(_thisTransform.DOMove(nextPos, speed - defaultSpeed).SetEase(Ease.Linear));
            seq.AppendCallback(() =>
            {
				OnMoveEnd?.Invoke(ThisActor.UUID, position - _character.Position);
				ThisActor.Position = nextPos;
                _isMoving = false;
                MoveStop(); 
				seq.Kill();
            });
        }

        public virtual void Jump(Vector3 originPos, Vector3 dir, int distance)
        {
            if (_isMoving) return;
            var seq = DOTween.Sequence();
            var currentPos = originPos;
            int i = distance;
            var nextPos = originPos + dir * distance;
            while (i > 0)
            {
                nextPos = originPos + dir * i;
                nextPos.y = 1;
                var nextBlock = InGame.GetBlock(nextPos.SetY(0));
                if(nextBlock != null)
                        break;
                i--;
            }

            var map = Define.GetManager<MapManager>();

            if (!map.IsStayable(nextPos.SetY(0)))
            {
                MoveStop();
                Debug.Log(1);
                return;
            }

            var block = map.GetBlock(nextPos.SetY(0));
            if(block.CheckActorOnBlock(ThisActor) == false) return;
            _character.AddState(Actors.Characters.CharacterState.Move);

            PositionUpdate(nextPos);
            var speed = _character.GetAct<CharacterStatAct>().ChangeStat.speed;

            seq.Append(_thisTransform.DOJump(nextPos, 1, 1, speed));
            seq.AppendCallback(() =>
            {
                originPos = nextPos;
                _isMoving = false;
                MoveStop();
                seq.Kill();
            });
        }

        public void PositionUpdate(Vector3 nextPos)
        {
            if(_positionUpdateCoroutine != null)
                ThisActor.StopCoroutine(_positionUpdateCoroutine);
            _positionUpdateCoroutine = PositionUpdateCoroutine(nextPos);
            ThisActor.StartCoroutine(_positionUpdateCoroutine);
        }
        public IEnumerator PositionUpdateCoroutine(Vector3 nextPos)
        {
            _isMoving = true;
            var originPos = ThisActor.Position;
            nextPos.y = 0;
            var block = InGame.GetBlock(nextPos);
            block.isMoving = true;
            while (_isMoving)
            {
                yield return new WaitForFixedUpdate();
                var pos = _thisTransform.position;
                var x = Mathf.RoundToInt(pos.x);
                var z = Mathf.RoundToInt(pos.z);
                if (Mathf.Abs(x - pos.x) != 0)
                {
                    pos.x = x;
                }

                if (Mathf.Abs(z - pos.z) != 0)
                {
                    pos.z = z;
                }
                InGame.SetActorOnBlock(ThisActor, pos);
                ThisActor.Position = pos;
            }

            block.isMoving = false;
            var dir = ThisActor.Position - originPos;
            _positionUpdateCoroutine = null;
        }

        public void Chase(Actor target)
        {
            if(_character.HasState(CharacterState.Move)) return;
            ThisActor.StartCoroutine(AstarCoroutine(target.Position));
        }

        Astar astar = new Astar();
        private IEnumerator AstarCoroutine(Vector3 end)
        {
            if (InGame.GetBlock(end).isWalkable == false)
            {
                _character.RemoveState(CharacterState.Move);
                yield break;
            }
            _character.AddState(CharacterState.Move);
        
            astar.SetPath(ThisActor.Position, end);
            ThisActor.StartCoroutine(astar.FindPath());
            yield return new WaitUntil(astar.IsFinished);
            
            var nextBlock = astar.GetNextPath();
            if (nextBlock == null)
            {
                _character.RemoveState(CharacterState.Move);
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

        protected virtual void MoveStop()
        {
            _character.RemoveState(CharacterState.Move);
        }
    }
}