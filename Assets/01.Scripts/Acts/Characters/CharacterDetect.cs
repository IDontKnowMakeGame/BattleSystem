using System;
using Acts.Base;
using Core;
using UnityEngine;

namespace Acts.Characters
{
    public class CharacterDetect : Act
    {
        public event Action<Vector3> EnterDetect;
        public event Action<Vector3> StayDetect;
        public event Action<Vector3> ExitDetect;
        private bool isDetecting = false;

        public override void Update()
        {
            if (InGame.Player == null) return;
            var dirs = new[] { Vector3.forward, Vector3.right, Vector3.back, Vector3.left };

            var detect = false;
            foreach (var dir in dirs)
            {
                if (ThisActor.Position + dir != InGame.Player.Position) continue;

                if (!isDetecting)
                    EnterDetect?.Invoke(dir);
                isDetecting = true;
                detect = true;
                StayDetect?.Invoke(dir);
            }

            if (detect == false)
            {
                isDetecting = false;
                ExitDetect?.Invoke(Vector3.zero);
            }
        }
    }
}