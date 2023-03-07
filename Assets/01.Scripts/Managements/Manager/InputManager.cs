using System;
using UnityEngine;

namespace Managements.Managers
{
    public class InputManager : Manager
    {
        public static event Action<Vector3> OnMovePressed;

        public override void Update()
        {
            InputMove();
        }

        private void InputMove()
        {
            if (Input.GetKeyDown(KeyCode.W))
            {
                OnMovePressed?.Invoke(Vector3.forward);
            }
            if (Input.GetKeyDown(KeyCode.S))
            {
                OnMovePressed?.Invoke(Vector3.back);
            }
            if (Input.GetKeyDown(KeyCode.A))
            {
                OnMovePressed?.Invoke(Vector3.left);
            }
            if (Input.GetKeyDown(KeyCode.D))
            {
                OnMovePressed?.Invoke(Vector3.right);
            }
        }
    }
}