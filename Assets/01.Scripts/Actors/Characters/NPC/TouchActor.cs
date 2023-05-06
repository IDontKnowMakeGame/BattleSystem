using System;
using System.Collections.Generic;
using Actors.Bases;
using Core;
using UnityEngine;
using UnityEngine.Events;

namespace Actors.Characters.NPC
{
    public class TouchActor : Actor
    {
        public List<Vector3> _touchPosArea = new List<Vector3>();
        public UnityEvent _onTouch = new UnityEvent();

        [SerializeField]
        private bool OneShot = false;

        private bool check = false;

        protected override void Update()
        {
            foreach (var pos in _touchPosArea)
            {
                if(InGame.Player.Position == Position + pos)
                {
                    if (!OneShot || !check)
                    {
                        _onTouch?.Invoke();
                        check = true;
                    }
                }
            }
            base.Update();
            UpdatePosition();
        }

        private void OnDrawGizmos()
        {
            Gizmos.color = Color.red;
            foreach (var pos in _touchPosArea)
            {
                Gizmos.DrawWireCube(Position + pos, Vector3.one);
            }
        }
    }
}