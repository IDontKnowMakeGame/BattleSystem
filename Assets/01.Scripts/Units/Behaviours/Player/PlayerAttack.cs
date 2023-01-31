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

        [SerializeField]
        private CameraZoom cameraZoom;

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
            List<EnemyBase> enemys = new List<EnemyBase>();

            if (near)
               enemys.Add(attackColParent.CurrntDirNearEnemy());
            else
               enemys = attackColParent.AllCurrentDirEnemy();

            if (enemys.Count > 0)
                ThisBase.StartCoroutine(cameraZoom.ZoomInOut(1f));

            foreach(EnemyBase enemy in enemys)
            {
                enemy.ThisStat.Damaged(damage);
                GameObject obj = GameManagement.Instance.GetManager<ResourceManagers>().Instantiate("Damage");
                obj.GetComponent<DamagePopUp>().DamageText(damage, enemy.transform.position);
                onBehaviourEnd?.Invoke();
            }
        }
    }
}
