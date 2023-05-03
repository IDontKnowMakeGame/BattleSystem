

using UnityEngine;

namespace Actors.Characters.Traps
{
    public class ArrowTrapActor : DispenserTrapActor
    {
        public int speed = 2;
        protected override void Shoot()
        {
            var bulletObj = pool.Pop(bulletPrefab).gameObject;
            bulletObj.transform.position = transform.position;
            bulletObj.transform.rotation = transform.rotation;

            var bullet = bulletObj.GetComponent<Arrow>();
            var dir = transform.rotation.eulerAngles.Euler2Dir();
            Debug.Log(dir);
            bullet.Shoot(dir, Position, this, speed, 25, 10,true);
        }
    }
}