using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Acts.Base;

namespace Acts.Characters
{
    public class CharacterAttack : Act
    {
        /// <summary>
        /// event�� �̿��Ͽ� ������ Ÿ�ֿ̹� �������� ���� �ϴ� �Լ�
        /// </summary>
        public virtual void AttackCheck(AttackInfo attackInfo)
        {

        }

        public virtual void ReadyAttackAnimation(Vector3 dir)
        {

        }
    }
}