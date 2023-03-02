using System;
using Units.Base.Trap;
using UnityEngine;

namespace Units.Behaviours.Unit
{
    [Serializable]
    public class UnitCollider : UnitBehaviour
    {
        [SerializeField] private Vector3 size = Vector3.one;
        public static event Action<GameObject> OnPlateTriggered;

        public override void Update()
        {
            CheckCollision();
        }

        public void CheckCollision()
        {
            var traps = Physics.OverlapBox(ThisBase.transform.position, size * 0.5f, Quaternion.identity, LayerMask.GetMask("Trap"));
            if (traps == null) return;
            foreach (var trap in traps)
            {
                OnPlateTriggered.Invoke(trap.gameObject);
            }
        }
    }
}