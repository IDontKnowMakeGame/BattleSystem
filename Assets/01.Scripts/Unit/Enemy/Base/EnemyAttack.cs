using Manager;
using Unit.Player;
using UnityEngine;

namespace Unit.Enemy.Base
{
    public class EnemyAttack : UnitAttack
    {
        public void SlideAttack(Vector3 dir, float damage, float delay)
        {
            if (dir.x != 0)
            {
                WaitAttack(dir + Vector3.back, damage, delay);
                WaitAttack(dir, damage, delay);
                WaitAttack(dir + Vector3.forward, damage, delay);
            }
            else if (dir.z != 0)
            {
                WaitAttack(dir + Vector3.left, damage, delay);
                WaitAttack(dir, damage, delay);
                WaitAttack(dir + Vector3.right, damage, delay);
            }
        }

        public void KnockBackAttack(Vector3 pos, float damage, Vector3 knockbackDir)
        {
            var map = GameManagement.Instance.GetManager<MapManager>();
            map.GiveDamage<PlayerStats>(pos, damage, 0);
            var targetBlock = map.GetBlock(pos);
            var target = targetBlock.GetUnit();
            if (target != null)
            {
                if (target == thisBase) return;
                target.GetBehaviour<PlayerMove>().Translate(-knockbackDir, 0.3f);
            }
        }

        public void HalfAttack(Vector3 dir, float damage, float delay)
        {
            if (dir.x != 0)
            {
                WaitAttack(dir + Vector3.back, damage, delay);
                WaitAttack(dir, damage, delay);
                WaitAttack(dir + Vector3.forward, damage, delay);
                WaitAttack(Vector3.back, damage, delay);
                WaitAttack(Vector3.forward, damage, delay);
            }
            else if (dir.z != 0)
            {
                WaitAttack(dir + Vector3.left, damage, delay);
                WaitAttack(dir, damage, delay);
                WaitAttack(dir + Vector3.right, damage, delay);
                WaitAttack(Vector3.left, damage, delay);
                WaitAttack(Vector3.right, damage, delay);
            }
        }
    }
}