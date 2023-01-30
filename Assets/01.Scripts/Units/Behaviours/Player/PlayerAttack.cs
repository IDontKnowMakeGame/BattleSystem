using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Units.Behaviours.Unit;
using Units.Base.Unit;

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
                    Debug.LogError("attackColParent?? NULL????.");
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
            // To Do InputManager?? ???
            if (Input.GetKeyDown(KeyCode.W))
                Attack(Vector3.up);
        }

        public void Attack(float damage, bool near = false)
        {
            ThisBase.GetBehaviour<PlayerMove>().ClearMove();
            List<UnitBase> enemys = new List<UnitBase>();

            if (near)
               enemys.Add(attackColParent.CurrntDirNearEnemy());
            else
                enemys = attackColParent.AllCurrentDirEnemy();

            foreach (UnitBase enemy in enemys)
            {
                enemy.GetBehaviour<UnitStat>().Damaged(1);
            }
        }
    }
}
