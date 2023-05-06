

using UnityEngine;

namespace Actors.Characters.Traps
{
    public class ArrowTrapActor : DispenserTrapActor
    {
        [SerializeField]
        private int _speed = 2;
		[SerializeField]
		private int _damage = 25;
		[SerializeField]
		private int _range = 10;

        protected override void Shoot()
        {
            var bulletObj = pool.Pop(bulletPrefab).gameObject;
            bulletObj.transform.position = transform.position;
            bulletObj.transform.rotation = transform.rotation;

            var bullet = bulletObj.GetComponent<Arrow>();
            var dir = transform.rotation.eulerAngles.Euler2Dir();
            bullet.Shoot(dir, Position, this, _speed, _damage, _range, true);
        }
    }
}