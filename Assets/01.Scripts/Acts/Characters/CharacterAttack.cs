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
		/// event를 이용하여 때리는 타이밍에 데미지를 들어가게 하는 함수
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