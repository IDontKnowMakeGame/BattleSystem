using System;
using System.Collections;
using System.Collections.Generic;
using Actors.Bases;
using Actors.Characters;
using Acts.Base;
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
        public static event Action<int, Vector3> OnMoveEnd;
        protected bool _isMoving = false;
        private CharacterActor _character => ThisActor as CharacterActor;

        [SerializeField]
        private float defaultSpeed = 0;

        public override void Awake()
        {
            _thisTransform = ThisActor.transform;
        }

        public virtual void Translate(Vector3 direction)
        {
            var nextPos = ThisActor.Position + direction;
            Move(nextPos);
        }
        
        public virtual void Move(Vector3 position)
        {
            if (_isMoving) return;
            var seq = DOTween.Sequence();
            var currentPos = ThisActor.Position;
            var nextPos = position;
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
            
            MoveAnimation();

            ThisActor.StartCoroutine(PositionUpdateCoroutine(nextPos));
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

        public virtual void Jump(Vector3 position)
        {
            if (_isMoving) return;
            var seq = DOTween.Sequence();
            var currentPos = ThisActor.Position;
            var nextPos = position;
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

            MoveAnimation();

            ThisActor.StartCoroutine(PositionUpdateCoroutine(nextPos));
            var speed = _character.GetAct<CharacterStatAct>().ChangeStat.speed;

            seq.Append(_thisTransform.DOJump(nextPos, 1, 1, speed));
            seq.AppendCallback(() =>
            {
                ThisActor.Position = nextPos;
                _isMoving = false;
                MoveStop();
                seq.Kill();
            });
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
                var x = Mathf.Round(pos.x);
                var z = Mathf.Round(pos.z);
                if (x != pos.x)
                {
                    pos.x = x;
                }

                if (z != pos.z)
                {
                    pos.z = z;
                }
                InGame.SetActorOnBlock(ThisActor, pos);
                ThisActor.Position = pos;
            }

            block.isMoving = false;
            var dir = ThisActor.Position - originPos;
        }

        public void Chase(Actor target)
        {
            if(_character.HasState(CharacterState.Move)) return;
            ThisActor.StartCoroutine(AstarCoroutine(target.Position));
        }

        private IEnumerator AstarCoroutine(Vector3 end)
        {
            _character.AddState(CharacterState.Move);
            var astar = new Astar();
            astar.SetPath(ThisActor.Position, end);
            ThisActor.StartCoroutine(astar.FindPath());
            yield return new WaitUntil(astar.IsFinished);
            var nextBlock = astar.GetNextPath();
            if (nextBlock == null) yield break;
            var nextPos = nextBlock.Position;
            Move(nextPos);
        }
        protected virtual void MoveAnimation()
        {

        }

        protected virtual void MoveStop()
        {
            _character.RemoveState(CharacterState.Move);
        }
    }
}