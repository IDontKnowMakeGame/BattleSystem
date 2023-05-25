using System;
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

            var seq = DOTween.Sequence();
            var speed = _character.GetAct<CharacterStatAct>().ChangeStat.speed;
            seq.Append(_thisTransform.DOMove(nextPos, speed - defaultSpeed).SetEase(Ease.Linear));
            seq.AppendCallback(() =>
            {
                Debug.Log("MoveEnd");
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
                MoveStop();
                return;
            }

            var ccState = CharacterState.Stun | CharacterState.KnockBack | CharacterState.StopMove;
            if (_character.HasState(ccState))
            {
                enableQ = false;
                MoveStop();
                return;
            }
            var currentPos = ThisActor.Position;
            var nextPos = position;
            nextPos.y = 1;
            

            var map = Define.GetManager<MapManager>();
            var direction = (nextPos - currentPos).SetY(0).GetDirection();
            int dirMagnitude = (int)Vector3Int.RoundToInt((nextPos - currentPos).SetY(0)).magnitude;

            for(int i = dirMagnitude; i > 1; i--)
            {
                if (map.IsStayable(nextPos.SetY(0))) break;
                nextPos -= direction;
            }

            if (!map.IsStayable(nextPos.SetY(0)))
            {
                enableQ = false;
                MoveStop();
                return;
            }
            
            
            if (map.IsBlocking(currentPos + direction))
            {
                enableQ = false;
                MoveStop();
                return;
            }
            
            var block = map.GetBlock(nextPos.SetY(0));
            if(block.CheckActorOnBlock(ThisActor) == false)
            {
                enableQ = false;
                MoveStop();
                return;
            }

            if (_character.HasState(CharacterState.Move) == false)
                _character.AddState(Actors.Characters.CharacterState.Move);
            else
            {
                MoveStop();
                return;
            }
            AnimationCheck();

            var speed = _character.GetAct<CharacterStatAct>().ChangeStat.speed;
            block.isWalkable = false;
            var seq = DOTween.Sequence();
            seq.Append(_thisTransform.DOMove(nextPos, speed - defaultSpeed).SetEase(Ease.Linear));
            seq.InsertCallback((speed - defaultSpeed) / 2, () => enableQ = false);
            seq.AppendCallback(() =>
            {
				OnMoveEnd?.Invoke(ThisActor.UUID, position - _character.Position);
                MoveStop();
                block.isWalkable = true;
                isChasing = false;
				seq.Kill();
            });
        }

        public virtual void Jump(Vector3 targetPos, Vector3 dir, int distance, float power = 1)
        {
            if (_isMoving) return;
            int i = distance;
            var nextPos = targetPos + dir * distance;
            while (i >= 0)
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
                MoveStop();
                return;
            }

            var block = map.GetBlock(nextPos.SetY(0));
            if(block.CheckActorOnBlock(ThisActor) == false) return;
            _character.AddState(Actors.Characters.CharacterState.Move);

            var speed = _character.GetAct<CharacterStatAct>().ChangeStat.speed;
            var seq = DOTween.Sequence();
            block.isWalkable = false;
            seq.Append(_thisTransform.DOJump(nextPos, power, 1, speed));
            seq.AppendCallback(() =>
            {
                map.GetBlock(nextPos.SetY(0)).SetActorOnBlock(ThisActor);
                MoveStop();
                block.isWalkable = true;
                isChasing = false;
                seq.Kill();
            });
        }

        private bool isChasing = false;
        public void Chase(Actor target)
        {
            if(_character.HasState(CharacterState.Chase)) return;
            _character.AddState(CharacterState.Chase);
            if(_character.HasState(CharacterState.Move)) return;
            if(_character.HasState(CharacterState.Attack)) return;
            if(_character.HasState(CharacterState.Hold)) return;
            if (isChasing) return;
            ThisActor.StartCoroutine(AstarCoroutine(target.Position));
        }

        Astar astar = new Astar();
        private IEnumerator AstarCoroutine(Vector3 end)
        {
            isChasing = true;
            if (InGame.GetBlock(end).isWalkable == false)
            {
                isChasing = false;
                _character.RemoveState(CharacterState.Chase);
                yield break;
            }
            astar.SetPath(ThisActor.Position, end);
            ThisActor.StartCoroutine(astar.FindPath());
            yield return new WaitUntil(astar.IsFinished);
            var nextBlock = astar.GetNextPath();
            if (nextBlock == null)
            {
                isChasing = false;
                _character.RemoveState(CharacterState.Chase);
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
                animation.Play("HorizontalMove");
                ThisActor.OnDirectionUpdate?.Invoke(dir.x);
            }

            if (dir == Vector3.right)
            {
                animation.Play("HorizontalMove");   
                ThisActor.OnDirectionUpdate?.Invoke(dir.x);
            }
        }

        protected virtual void MoveStop()
        {
            _character.RemoveState(CharacterState.Move);
            _character.RemoveState(CharacterState.Chase);
            isChasing = false;
        }


    }
}