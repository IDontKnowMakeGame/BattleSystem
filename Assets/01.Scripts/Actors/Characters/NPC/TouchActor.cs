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


        protected override void Update()
        {
            foreach (var pos in _touchPosArea)
            {
                if(InGame.Player.Position == Position + pos)
                {
                    _onTouch?.Invoke();
                }
            }
            base.Update();
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