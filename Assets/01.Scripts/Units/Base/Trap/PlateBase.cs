using System;
using Core;
using Managements.Managers;
using Units.Base.Interactable;
using Units.Base.Unit;
using Units.Behaviours.Unit;
using UnityEngine;

namespace Units.Base.Trap
{
    public class PlateBase : UnitBase
    {
        [SerializeField] protected Transform plateTransform;
        protected bool IsDetected = false;

        protected override void Awake()
        {
            base.Awake();
        }

        protected override void Start()
        {
            base.Start();
            UnitCollider.OnPlateTriggered += Interact;
            InGame.SetUnit(null, Position);
        }

        public virtual void Interact(GameObject obj)
        {
            if (obj != gameObject) return;
            IsDetected = true;
        }

        protected override void Update()
        {
            ChangeScale();
            IsDetected = false;
            base.Update();
        }

        private void ChangeScale()
        {
            if (IsDetected)
                plateTransform.localScale = new Vector3(1, 0.05f, 1);
            else
            {
                plateTransform.localScale = new Vector3(1, 0.15f, 1);
            }
        }

        protected override void OnDisable()
        {
            UnitCollider.OnPlateTriggered -= Interact;
            base.OnDisable();
        }

        protected override void OnDestroy()
        {
            UnitCollider.OnPlateTriggered -= Interact;
            base.OnDestroy();
        }

        protected override void OnApplicationQuit()
        {
            UnitCollider.OnPlateTriggered -= Interact;
            base.OnApplicationQuit();
        }
    }
}