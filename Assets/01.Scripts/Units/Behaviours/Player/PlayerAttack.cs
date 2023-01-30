using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Units.Behaviours.Unit;
using Units.Base.Unit;
using Managements;
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
        }

        public void Attack(float damage, bool near = false)
        {
            List<UnitBase> enemys = new List<UnitBase>();

            GameObject obj = GameManagement.Instance.GetManager<ResourceManagers>().Instantiate("Damage");
            obj.GetComponent<DamagePopUp>().DamageText(damage, ThisBase.transform.position);

            if (near)
               enemys.Add(attackColParent.CurrntDirNearEnemy());
            else
               enemys = attackColParent.AllCurrentDirEnemy();
        }
    }
}
