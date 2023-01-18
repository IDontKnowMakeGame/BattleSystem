using System;
using System.Collections.Generic;
using Managements.Managers.Base;
using UnityEngine;

namespace Managements.Managers
{
    public enum InputStatus
    {
        Press,
        Hold,
        Release
    }

    public enum InputTarget
    {
        Left,
        Right,
        Up,
        Down,
        Attack,
    }

    public class Input
    {
        public Action action;
        public KeyCode keyCode;
        public InputStatus inputStatus;
    }
    
    public class InputManager : Manager
    {
        private Dictionary<InputTarget, Input> _inGameInputs = new();

        public override void Update()
        {
            foreach (var input in _inGameInputs)
            {
                switch (input.Value.inputStatus)
                {
                    case InputStatus.Press:
                        if (UnityEngine.Input.GetKeyDown(input.Value.keyCode))
                        {
                            input.Value.action?.Invoke();
                        }
                        break;
                    case InputStatus.Hold:
                        if (UnityEngine.Input.GetKey(input.Value.keyCode))
                        {
                            input.Value.action?.Invoke();
                        }
                        break;
                    case InputStatus.Release:
                        if (UnityEngine.Input.GetKeyUp(input.Value.keyCode))
                        {
                            input.Value.action?.Invoke();
                        }
                        break;
                }
            }
        }

        public void InitInGameInput(InputTarget target, Input input)
        {
            _inGameInputs.Add(target, input);
        }

        public void ChangeInGameAction(InputTarget target, Action action)
        {
            _inGameInputs[target].action = action;
        }
        
        public void AddInGameAction(InputTarget target, Action action)
        {
            _inGameInputs[target].action += action;
        }
        
        public void RemoveInGameAction(InputTarget target, Action action)
        {
            _inGameInputs[target].action -= action;
        }

        public void ChangeInGameKey(InputTarget target, KeyCode keyCode)
        {
            _inGameInputs[target].keyCode = keyCode;
        }
        
        public void ChangeInGameStatus(InputTarget target, InputStatus status)
        {
            _inGameInputs[target].inputStatus = status;
        }
    }
}