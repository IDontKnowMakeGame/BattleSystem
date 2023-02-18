using Core;
using Managements.Managers;
using Units.Base.Unit;
using UnityEngine;

namespace Units.Base.Interactable
{
    public class InteractableUnitBase : UnitBase
    {
        public virtual bool IsInteracted { get; set; }
        protected override void Awake()
        {
            base.Awake();
            InputManager.OnInteractionPress += Interact;
        }

        public virtual void Interact()
        {
            if(IsInteracted) return;
            if(InGame.PlayerBase.Position.IsNeighbor(Position))
                Debug.Log("Interacted");
        }
    }
}