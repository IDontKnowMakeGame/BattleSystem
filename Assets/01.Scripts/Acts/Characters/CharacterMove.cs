using System;
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
            
            Debug.Log("Move");
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

            PositionUpdate(nextPos);
            var speed = _character.GetAct<CharacterStatAct>().ChangeStat.speed;
            seq.Append(_thisTransform.DOMove(nextPos, speed - defaultSpeed).SetEase(Ease.Linear));
            seq.InsertCallback((speed - defaultSpeed) / 2, () => enableQ = false);
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
                var nextBlock = InGame.GetBlock(nextPos.SetY(0));
                if(nextBlock != null)
                        break;
                i--;
            }

            nextPos.y = 1;
            var map = Define.GetManager<MapManager>();
            var knockBack = false;
            Actor detectedTarget = null;
            if (map.GetBlock(nextPos.SetY(0)).IsActorOnBlock)
            {
                detectedTarget = map.GetBlock(nextPos.SetY(0)).ActorOnBlock;
                knockBack = true;
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
                _isMoving = false;
                if (knockBack)
                {
                    var target = map.GetBlock(nextPos.SetY(0)).ActorOnBlock;
                    if (!target) return;
                    if(detectedTarget == target)
                        target.GetAct<CharacterMove>()?.KnockBack(dir);
                }
                originPos = nextPos;
                map.GetBlock(nextPos.SetY(0)).SetActorOnBlock(ThisActor);
                MoveStop();
                seq.Kill();
            });
            PositionUpdate(nextPos);
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
            var nextBlock = InGame.GetBlock(nextPos);
            var currentBlock = InGame.GetBlock(originPos);
            nextBlock.isMoving = true;
            while (_isMoving)
            {
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

                if (Vector3.Distance(pos.SetY(0), nextPos) <= 0.1f) 
                {
                    nextBlock.SetActorOnBlock(ThisActor);
                    currentBlock.RemoveActorOnBlock();
                    ThisActor.Position = pos;
                    _isMoving = false;
                }
                yield return new WaitForSeconds(Time.deltaTime);
            }
            nextBlock.isMoving = false;
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

        public void KnockBack(Vector3 dir)
        {
            Debug.Log(dir);
            _character.AddState(CharacterState.KnockBack);
            var originPos = ThisActor.Position;
            var nextPos = originPos + dir;
            var map = Define.GetManager<MapManager>();
            if (!map.IsStayable(nextPos))
            {
                _character.Stun();
                return;
            }
            nextPos.y = 1;
            var seq = DOTween.Sequence();
            seq.Append(_thisTransform.DOMove(nextPos, 0.1f).SetEase(Ease.Flash));
            seq.AppendCallback(() =>
            {
                _character.RemoveState(CharacterState.KnockBack);
                map.GetBlock(nextPos.SetY(0)).SetActorOnBlock(ThisActor);
                ThisActor.Position = nextPos;
                seq.Kill();
            });
        }

        protected virtual void MoveStop()
        {
            _character.RemoveState(CharacterState.Move);
        }
    }
}