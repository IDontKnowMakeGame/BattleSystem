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
        protected override void OnDetect()
        {
            if (IsDetected) return;
            if (DetectCondition.Invoke(Position) == false) return;
            IsDetected = true;
            StartCoroutine(SpikeCoroutine());
            
        }

        protected override void OnLostDetect()
        {
            if(IsDetected == false) return;
            if (DetectCondition.Invoke(Position)) return;
            IsDetected = false;
        }
        
        private IEnumerator SpikeCoroutine()
        {
            yield return new WaitForSeconds(SpikeDelay);
            plateTransform.localScale = new Vector3(0.5f, 0.5f, 0.5f);
            Define.GetManager<MapManager>().GetBlock(Position).GetUnit()?.GetBehaviour<UnitStat>().Damaged(Damage, this);
            yield return new WaitForSeconds(SpikeTime);
            plateTransform.localScale = new Vector3(0.5f, 0.0f, 0.5f);
            IsDetected = false;
        }
    }
}