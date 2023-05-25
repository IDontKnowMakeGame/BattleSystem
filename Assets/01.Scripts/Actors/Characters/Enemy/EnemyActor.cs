using System;
using Acts.Characters.Enemy;
using AI;
using Core;
using UnityEngine;
using Random = UnityEngine.Random;
using System.Reflection;

namespace Actors.Characters.Enemy
{
    public enum EnemyType
    {
        None,
        OldShade,
        OldBowShade,
        OldSpearShade,
        CrazyGhostActor,
        All,
    }

    public class EnemyActor : CharacterActor
    {  
        [SerializeField] protected float _secondPhaseHpPercent;
        [SerializeField] protected EnemyAI _enemyAi;
        [SerializeField] protected CharacterEquipmentAct _characterEquipment;
		[SerializeField] private EnemyStatAct _characterStat;
        [SerializeField] protected EnemyAnimation _enemyAnimation;
        [SerializeField] private bool alive = true;
        [SerializeField] private int spriteSize = 1;
        [SerializeField] private EnemyType currentType = EnemyType.None;
        public EnemyType CurrentType => currentType;

        public bool Alive
        {
            get => alive;
            set => alive = value;
        }

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

        protected string GetDirName(Vector3 dir)
        {
            Vector3 cameraDir = InGame.CameraDir();

            var degree = Mathf.Atan2(cameraDir.x, cameraDir.z) * Mathf.Rad2Deg;
            degree = Mathf.Abs(Mathf.RoundToInt(degree));
            dir = InGame.CamDirCheck(dir);

            if(degree == 90)
            {
                dir.x = -dir.x;
                dir.z = -dir.z;
            }
            var result = string.Empty;
            if (dir.z > 0)
                result = "Upper";
            else if (dir.z < 0)
                result = "Lower";
            else if (dir.x != 0)
            {
                result = "Horizontal";
                OnDirectionUpdate?.Invoke(dir.x);
            }
            return result;
        }

        protected void Attack(Vector3 dir, string stateName, Action onAttack = null, bool isLast = true, Action onEnd = null, Action onStart = null)
        {
            var dirName = GetDirName(dir);
            var nextState = dirName + stateName;
            var readyClip =  _enemyAnimation.GetClip( nextState + "Ready");
            var attackClip = _enemyAnimation.GetClip( nextState + "Attack");
            var returnClip = _enemyAnimation.GetClip( nextState + "Return");
            if (readyClip == null || attackClip == null || returnClip == null)
            {
                nextState = "Lower" + stateName;
                readyClip =  _enemyAnimation.GetClip( nextState + "Ready");
                attackClip = _enemyAnimation.GetClip( nextState + "Attack");
                returnClip = _enemyAnimation.GetClip( nextState + "Return");
            }
            onStart?.Invoke();
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
                        onEnd?.Invoke();
                    };
                };
            };
        }

        protected void AttackWithNoReady(Vector3 dir, string stateName, Action onAttack)
        {
            var dirName = GetDirName(dir);
            var nextState = dirName + stateName;
            var attackClip = _enemyAnimation.GetClip( nextState + "Attack");
            var returnClip = _enemyAnimation.GetClip( nextState + "Return");
            if (returnClip == null || attackClip == null)
            {
                nextState = "Lower" + stateName;
                attackClip = _enemyAnimation.GetClip( nextState + "Attack");
                returnClip = _enemyAnimation.GetClip( nextState + "Return");
            }
            if(attackClip == null || returnClip == null) return;
            _enemyAnimation.Play(nextState + "Attack");
            attackClip.SetEventOnFrame(0, () =>
            {
                onAttack?.Invoke();
            });
            attackClip.OnExit = () =>
            {
                _enemyAnimation.Play(nextState + "Return");
                returnClip.OnExit = () =>
                {
                    RemoveState(CharacterState.Attack);
                };
            };
        }

        protected void AttackWithNoReturn(Vector3 dir, string stateName, Action onAttack, Action onEnd)
        {
            var dirName = GetDirName(dir);
            var nextState = dirName + stateName;
            
            var readyClip =  _enemyAnimation.GetClip(nextState + "Ready");
            var attackClip = _enemyAnimation.GetClip(nextState + "Attack");
            if (readyClip == null || attackClip == null)
            {
                nextState = "Lower" + stateName;
                readyClip = _enemyAnimation.GetClip(nextState + "Ready");
                attackClip = _enemyAnimation.GetClip( nextState + "Attack");
            }

            if(readyClip == null || attackClip == null) return;
            _enemyAnimation.Play(nextState + "Ready");
            readyClip.OnExit = () =>
            {
                _enemyAnimation.Play(nextState + "Attack");
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