using System;
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
        public Func<Vector3, bool> DetectCondition = null;

        protected override void Awake()
        {
            base.Awake();
            InputManager.OnInteractionPress += Interact;
            DetectCondition = vector3 => InGame.PlayerBase.Position.IsNeighbor(vector3);
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
            if (DetectCondition.Invoke(Position))
            {
                // TODO: Interact
                Debug.Log("Interact");
            }
        }

        protected virtual void OnDetect()
        {
            if(IsDetected) return;
            if (DetectCondition.Invoke(Position) == false) return;
            IsDetected = true;
            Debug.Log("Detect");
        }
        
        protected virtual void OnLostDetect()
        {
            if (IsDetected == false) return;
            if (DetectCondition.Invoke(Position)) return;
            IsDetected = false;
            Debug.Log("Lost Detect");
        }
    }
}