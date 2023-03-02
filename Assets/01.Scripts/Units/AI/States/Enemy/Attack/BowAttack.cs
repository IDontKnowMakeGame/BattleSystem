using System.Collections;

namespace Units.AI.States.Enemy.Attack
{
    public class BowAttack : AttackState
    {
        protected override IEnumerator AttackCoroutine()
        {
            //TODO 활 애니메이션을 시작하고 마무리 될때까지 기다린뒤 다음 상태로 넘어가야됨.
            attackCheck.SetBool(false); // 넘어가는 코드
            yield break;
        }

        protected virtual void ArrowAttack()
        {
            //TODO 활 공격하는 함수 위 함수에 넣어서 사용
        }
    }
}