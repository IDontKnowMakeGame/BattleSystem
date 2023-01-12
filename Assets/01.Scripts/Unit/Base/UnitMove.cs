using System;
using Manager;
using UnityEngine;

namespace Unit
{
    public class UnitMove : Behaviour
    {
        public Vector3 position;
        protected bool isMoving = false;
        protected Vector3 _originPosition;
        public override void Start()
        {
            position = thisBase.transform.position;
        }

        public virtual void Translate(Vector3 dir, float speed = 0)
        {

        }
        public bool IsMoving() => isMoving;

        public void Move(Vector3 nextPos)
        {
            position = nextPos;
            GameManagement.Instance.GetManager<MapManager>().GetBlock(position).MoveUnitOnBlock(thisBase);
            GameManagement.Instance.GetManager<MapManager>().GetBlock(_originPosition).RemoveUnitOnBlock();
            _originPosition = nextPos;
        }
    }
}