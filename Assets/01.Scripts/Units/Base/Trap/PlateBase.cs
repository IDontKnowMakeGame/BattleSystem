using Core;
using Managements.Managers;
using Units.Base.Interactable;
using UnityEngine;

namespace Units.Base.Trap
{
    public class PlateBase : InteractableUnitBase
    {
        [SerializeField] protected Transform plateTransform;
        protected override void Awake()
        {
            base.Awake();
            DetectCondition = vector3 => InGame.PlayerBase.Position == vector3;
        }

        protected override void Start()
        {
            base.Start();
            Define.GetManager<MapManager>().GetBlock(Position).UnitOnBlock();
        }

        public override void Interact()
        {
            return;
        }
        
        protected override void OnDetect()
        {
            if(IsDetected) return;
            if (DetectCondition.Invoke(Position) == false) return;
            IsDetected = true;
            plateTransform.localScale = new Vector3(0.8f, 0.05f, 0.8f);
        }
        
        protected override void OnLostDetect()
        {
            if (IsDetected == false) return;
            if (DetectCondition.Invoke(Position)) return;
            IsDetected = false;
            plateTransform.localScale = new Vector3(0.8f, 0.15f, 0.8f);
        }
    }
}