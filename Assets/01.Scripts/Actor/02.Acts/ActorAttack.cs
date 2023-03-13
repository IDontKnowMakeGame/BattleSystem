using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace Actor.Acts
{
    public class ActorAttack : Act
    {
        private AttackCollider attackCol;

        void Start()
        {
            attackCol = transform.GetComponentInChildren<AttackCollider>();
        }

        public void Attack(Vector3 pos, AttackInfo info)
        {
            attackCol.SetAttackCol(info);

            List<EnemyController> enemys = new List<EnemyController>();

            enemys.Add(attackCol.CurrntDirNearEnemy());

            foreach (EnemyController enemy in enemys)
            {
                Debug.Log(enemy.name); 
            }

            Debug.Log("����");
        }
    }
}
