namespace Actors.Characters.Traps
{
    public class ArrowTrapActor : DispenserTrapActor
    {
        protected override void Shoot()
        {
            var bulletObj = pool.Pop(bulletPrefab).gameObject;
            bulletObj.transform.position = transform.position;
            bulletObj.transform.rotation = transform.rotation;

            var bullet = bulletObj.GetComponent<Arrow>();
            var dir = transform.rotation.eulerAngles.Euler2Dir();
            bullet.Shoot(dir, Position, this, 2, 25, 10);
        }
    }
}