using System.Linq;
using Actors.Bases;
using Actors.Characters;
using Acts.Base;
using Core;
using DG.Tweening;
using Managements.Managers;
using UnityEngine;

namespace Acts.Characters
{
    public class CharacterCC : Act
    {
        private CharacterActor _character => ThisActor as CharacterActor;
        private Transform _thisTransform;

        public override void Awake()
        {
            _thisTransform = ThisActor.transform;
            _character.OnKnockBack += KnockBack;
            base.Awake();
        }

        public void KnockBack(int power, Actor attacker)
        {
            var map = Define.GetManager<MapManager>();
            var dirs = new[] { Vector3.forward * power, Vector3.back * power, Vector3.left * power, Vector3.right * power };
            dirs.Where((v) =>
            {
                var pos = ThisActor.Position + v;
                return map.IsStayable(pos);
            });
            var dir = (ThisActor.Position - attacker.Position).GetDirection();
            if (dir == Vector3.zero)
                dir = dirs[Random.Range(0, dirs.Length)];
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
    }
}