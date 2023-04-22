using System;
using Acts.Characters.Enemy;
using AI;
using Core;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Actors.Characters.Enemy
{
    public class EnemyActor : CharacterActor
    {
        [SerializeField] protected float _secondPhaseHpPercent;
        [SerializeField] protected EnemyAI _enemyAi;
        [SerializeField] protected CharacterEquipmentAct _characterEquipment;
		[SerializeField] private EnemyStatAct _characterStat;
        [SerializeField] protected EnemyAnimation _enemyAnimation;
        [SerializeField] private bool alive = true;

        public bool Alive => alive;

		protected override void Init()
        {
            AddAct(_characterEquipment);
            AddAct(_characterStat);
            AddAct(_enemyAnimation);
            base.Init();
        }

        protected bool IsSecondPhase()
        {
            var stat = GetAct<CharacterStatAct>();
            var maxHp = stat.BaseStat.hp;
            var hp = stat.ChangeStat.hp;
            var result = (hp / maxHp) * 100f < _secondPhaseHpPercent;
            return result;
        }

        private string GetDirName(Vector3 dir)
        {
            var result = string.Empty;
            if (dir.z > 0)
                result = "Upper";
            else if (dir.z < 0)
                result = "Lower";
            else if (dir.x != 0)
            {
                result = "Horizontal";
                SpriteTransform.localScale = new Vector3(3 * -dir.x, 3, 3);
            }
            return result;
        }

        protected void Attack(Vector3 dir, string stateName, Action onAttack = null, bool isLast = true)
        {
            var dirName = GetDirName(dir);
            Debug.Log(dirName);
            var nextState = dirName + stateName;
            var readyClip =  _enemyAnimation.GetClip( nextState + "Ready");
            var attackClip = _enemyAnimation.GetClip( nextState + "Attack");
            var returnClip = _enemyAnimation.GetClip( nextState + "Return");
            if (readyClip == null || attackClip == null)
            {
                nextState = "Lower" + stateName;
                readyClip =  _enemyAnimation.GetClip( nextState + "Ready");
                attackClip = _enemyAnimation.GetClip( nextState + "Attack");
                returnClip = _enemyAnimation.GetClip( nextState + "Return");
            }
            _enemyAnimation.Play( nextState + "Ready");
            readyClip.OnExit = () =>
            {
                _enemyAnimation.Play( nextState + "Attack");
                attackClip.SetEventOnFrame(0, () =>
                {
                    onAttack?.Invoke();
                });
                attackClip.OnExit = () =>
                {
                    _enemyAnimation.Play( nextState + "Return");
                    returnClip.OnExit = () =>
                    {
                        if(isLast)
                            RemoveState(CharacterState.Attack);
                    };
                };
            };
        }

        protected void AttackWithNoReady(string stateName, Action onAttack)
        {
            var attackClip = _enemyAnimation.GetClip(stateName + "Attack");
            var returnClip = _enemyAnimation.GetClip(stateName + "Return");
            if(attackClip == null || returnClip == null) return;
            _enemyAnimation.Play(stateName + "Attack");
            attackClip.SetEventOnFrame(0, () =>
            {
                onAttack?.Invoke();
            });
            attackClip.OnExit = () =>
            {
                _enemyAnimation.Play(stateName + "Return");
                returnClip.OnExit = () =>
                {
                    RemoveState(CharacterState.Attack);
                };
            };
        }

        protected void AttackWithNoReturn(string stateName, Action onAttack, Action onEnd)
        {
            var readyClip =  _enemyAnimation.GetClip(stateName + "Ready");
            var attackClip = _enemyAnimation.GetClip(stateName + "Attack");
            if(readyClip == null || attackClip == null) return;
            _enemyAnimation.Play(stateName + "Ready");
            readyClip.OnExit = () =>
            {
                _enemyAnimation.Play(stateName + "Attack");
                attackClip.SetEventOnFrame(0, () =>
                {
                    onAttack?.Invoke();
                });
                attackClip.OnExit = () =>
                {
                    onEnd?.Invoke();
                };
            };
        }
    }
}