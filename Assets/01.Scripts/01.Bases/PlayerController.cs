using Actor.Acts;
using Managements.Managers;
using System;
using UnityEngine;
using System.Collections;

namespace Actor.Bases
{
    public class PlayerController : ActorController
    {
        public Action OnSkill = null;
        protected override void Start()
        {
            base.Start();
            OnMove += GetAct<ActorMove>().Translate;
            OnChange += GetAct<PlayerActorChange>().Change;
            OnSkill = weapon.Skill;

            InputManager.OnChangePress += () => { OnChange?.Invoke(); };
            InputManager.OnMovePress += (pos) => { OnMove?.Invoke(pos, weapon); };
            InputManager.OnAttackPress += (pos) => { OnAttack?.Invoke(pos, weapon); };
            InputManager.OnSkillPress += OnSkill;
        }
    }
    protected override void Start()
    {
        base.Start();
        OnMove += GetAct<ActorMove>().Translate;
        OnChange += GetAct<PlayerActorChange>().Change;

        InputManager.OnChangePress += () => { OnChange?.Invoke(); };
        InputManager.OnMovePress += (dir) => { Move(dir, weapon); };
        InputManager.OnAttackPress += (pos) => { Attack(pos, weapon); };
    }

    public void Move(Vector3 dir, Weapon currentWeapon)
    {
        OnMove.Invoke(dir, currentWeapon);

        if (dir.x != 0f)
        {

        }
    }

    public void Attack(Vector3 pos, Weapon currentWeapon)
    {
        // 상태 확인 (조건문)
        //애니메이션 실행
        // 코루틴 실행
    }

    /*        IEnumerator AttackCoro(Weapon a, Vector3 pos)
            {
                yield return new WaitForSeconds(a.itemInfo.Ats);
                OnAttack?.Invoke(pos, a);
            }*/
}
}