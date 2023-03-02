using System.Collections;
using Core;
using Managements.Managers;
using Units.Behaviours.Unit;
using UnityEngine;

namespace Units.Base.Trap
{
    public class SpikeTrapBase : PlateBase
    {
        [SerializeField] private int Damage = 30;
        [SerializeField] private float SpikeTime = 0.5f;
        [SerializeField] private float SpikeDelay = 0.2f;

        public override void Interact(GameObject obj)
        {
            if (obj != gameObject) return;
            if (IsDetected == true) return;
            StartCoroutine(SpikeCoroutine());
        }

        protected override void Update()
        {
            
        }

        private IEnumerator SpikeCoroutine()
        {
            IsDetected = true;
            yield return new WaitForSeconds(SpikeDelay);
            plateTransform.localScale = new Vector3(0.5f, 0.5f, 0.5f);
            Define.GetManager<MapManager>().GetBlock(Position).GetUnit()?.GetBehaviour<UnitStat>().Damaged(Damage, this);
            yield return new WaitForSeconds(SpikeTime);
            plateTransform.localScale = new Vector3(0.5f, 0.0f, 0.5f);
            IsDetected = false;
        }
    }
}