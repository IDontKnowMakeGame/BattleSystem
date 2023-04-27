using Core;
using UnityEngine;

namespace Actors.Characters.Traps
{
    public class DispenserTrapActor : TrapActor
    {
        [SerializeField] protected GameObject bulletPrefab = null;
        
        protected PoolManager pool = null;

        protected override void Awake()
        {
            pool = Define.GetManager<PoolManager>();
            pool.CreatePool(bulletPrefab);
            
            OnTrapTrigger += Shoot;                                                                     
            base.Awake();
        }
        
        protected virtual void Shoot()
        {
            var bullet = Define.GetManager<PoolManager>().Pop(bulletPrefab).gameObject;
            bullet.transform.position = transform.position;
            bullet.transform.rotation = transform.rotation;
            bullet.SetActive(true);
        }
    }
}