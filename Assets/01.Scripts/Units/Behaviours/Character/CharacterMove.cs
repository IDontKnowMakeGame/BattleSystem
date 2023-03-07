using DG.Tweening;
using Managements.Managers;
using UnityEngine;
using Behaviour = Units.Behaviours.Default.Behaviour;

namespace Units.Behaviours.Character
{
    public class CharacterMove : Behaviour
    {
        private static float _speed = 0f;

        public override void Awake()
        {
            InputManager.OnMovePressed += Translate;
        }

        public void Translate(Vector3 direction)
        {
            MoveTo(ThisBase.Position + direction);
        }

        public void MoveTo(Vector3 position)
        {
            var state = ThisBase.GetBehaviour<CharacterState>();
            if(state.ContainState(State.Move))
                return;
            state.AddState(State.Move);
            Sequence seq = DOTween.Sequence();
            var originPos = ThisBase.Position;
            var nextPos = position;
            seq.Append(ThisBase.transform.DOMove(nextPos, _speed).SetEase(Ease.Linear));
            seq.AppendCallback(() =>
            {
                ThisBase.Position = nextPos;
                ThisBase.transform.position = nextPos;
                state.RemoveState(State.Move);
                seq.Kill();
            });

        }

        public static void WeightToSpeed(int weight)
        {
            var speed = (Mathf.Pow(weight, 2) + 20) * 0.01f;
            _speed = speed;
        }
    }
}