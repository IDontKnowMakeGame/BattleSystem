using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Acts.Base;
using System;

namespace Acts.Characters
{
    public class CharacterAttack : Act
    {
		public static Action<int> OnAttackEnd;
		/// <summary>
		/// event�� �̿��Ͽ� ������ Ÿ�ֿ̹� �������� ���� �ϴ� �Լ�
		/// </summary>
        /// 

        
		public virtual void AttackCheck(AttackInfo attackInfo)
        {

        }

        public virtual void ReadyAttackAnimation(AttackInfo attackInfo)
        {

        }
    }
}