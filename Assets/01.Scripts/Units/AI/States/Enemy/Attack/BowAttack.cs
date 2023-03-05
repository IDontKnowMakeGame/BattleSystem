using System.Collections;
using Tools;
using Units.Behaviours.Unit;
using UnityEngine;

namespace Units.AI.States.Enemy.Attack
{
    public class BowAttack : AttackState
    {
        private Vector3 _dir { get { return Quaternion.Euler(0, -angle, 0) * Vector3.forward; } }
    protected override IEnumerator AttackCoroutine()
        {
            //TODO 활 애니메이션을 시작하고 마무리 될때까지 기다린뒤 다음 상태로 넘어가야됨.
            UnitAnimation unitAnimation = ThisBase.GetBehaviour<UnitAnimation>();
            AnimeClip animeClip = unitAnimation.GetClip();
            unitAnimation.GetClip().SetEventOnFrame(0, () => ArrowAttack());
            attackCheck.SetBool(false); // 넘어가는 코드
            yield break;
        }

        protected virtual void ArrowAttack()
        {
            //TODO 활 공격하는 함수 위 함수에 넣어서 사용
            UnitEquiq equiq = ThisBase.GetBehaviour<UnitEquiq>();
            if (equiq.CurrentWeapon is BaseBow)
                (equiq.CurrentWeapon as BaseBow).Shoot(_dir);
            else if(equiq.SecoundWeapon is BaseBow)
                (equiq.CurrentWeapon as BaseBow).Shoot(_dir);
        }
    }
}