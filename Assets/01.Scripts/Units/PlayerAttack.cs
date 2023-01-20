using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Units.Behaviours.Unit;

namespace Units.Base.Player
{
    [System.Serializable]
    public class PlayerAttack : UnitAttack
    {
        private AttackCollider attackColParent;
        public AttackCollider AttackColParent
        {
            get
            {
                if (attackColParent == null)
                {
                    Debug.LogError("attackColParent가 NULL입니다.");
                    return new AttackCollider();
                }
                return attackColParent;
            }
        }

        public override void Start()
        {
            base.Start();
            attackColParent = GameObject.FindObjectOfType<AttackCollider>();
        }

        public override void Update()
        {
            base.Update();
            // To Do InputManager로 변환
            if (Input.GetKeyDown(KeyCode.W))
                Attack(Vector3.up);
        }

        public override void Attack(Vector3 idx)
        {
            // To Do 콜라이더를 통한 Attack Check
            ThisBase.GetBehaviour<PlayerMove>().ClearMove();
            ThisBase.GetBehaviour<PlayerAnimation>().AttackAnimation();
        }
    }
}
