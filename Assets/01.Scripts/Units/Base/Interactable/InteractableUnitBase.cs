using Core;
using Managements.Managers;
using Units.Base.Unit;
using UnityEngine;

namespace Units.Base.Interactable
{
    public class InteractableUnitBase : UnitBase
    {
        public virtual bool IsInteracted { get; set; }
        public virtual bool IsDetected { get; set; }
        [field:SerializeField] public override Vector3 SpawnPos { get; set; }

        protected override void Awake()
        {
            base.Awake();
            InputManager.OnInteractionPress += Interact;
        }

        protected override void Update()
        {
            OnDetect();
            OnLostDetect();
            base.Update();
        }

        public virtual void Interact()
        {
            if(IsInteracted) return;
            if (InGame.PlayerBase.Position.IsNeighbor(Position))
            {
                // TODO: Interact
            }
        }

        protected virtual void OnDetect()
        {
            if(IsDetected) return;
            if (InGame.PlayerBase.Position.IsNeighbor(Position) == false) return;
            IsDetected = true;
            Debug.Log("Detect");
        }
        
        protected virtual void OnLostDetect()
        {
            if (IsDetected == false) return;
            if (InGame.PlayerBase.Position.IsNeighbor(Position)) return;
            IsDetected = false;
            Debug.Log("Lost Detect");
        }
    }
}