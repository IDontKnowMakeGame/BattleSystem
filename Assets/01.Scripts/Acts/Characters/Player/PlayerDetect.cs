using System;
using Acts.Base;
using Core;
using UnityEngine;

namespace Acts.Characters.Player
{
    public class PlayerDetect : Act
    {
        public event Action<Vector3> EnterDetect;
        public event Action<Vector3> StayDetect;
        public event Action<Vector3> ExitDetect;
        private bool isDetecting = false;
        public override void Update()
        {
            var dirs = new[] { Vector3.forward, Vector3.right, Vector3.back, Vector3.left };

            foreach (var dir in dirs)
            {
                if (InGame.GetActor(ThisActor.Position + dir) == null) continue;
                if (InGame.GetActor(ThisActor.Position + dir) as InteractionActor == null) continue;

                if (!isDetecting)
                    EnterDetect?.Invoke(dir);
                isDetecting = true;
                StayDetect?.Invoke(dir);
            }

            if (isDetecting == false)
                ExitDetect?.Invoke(Vector3.zero);
            isDetecting = false;
        }
    }
}