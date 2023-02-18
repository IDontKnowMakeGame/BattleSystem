using Core;
using Managements.Managers;
using Units.Base.Unit;
using UnityEngine;

namespace Units.Base.Interactable
{
    public class InteractableUnitBase : UnitBase
    {
        protected override void Awake()
        {
            base.Awake();
            InputManager.OnInteractionPress += Interact;
        }

        public virtual void Interact()
        {
            if(InGame.PlayerBase.Position.IsNeighbor(Position))
                Debug.Log("Interacted");
        }
    }
}