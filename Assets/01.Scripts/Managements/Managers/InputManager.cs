using System;
using System.Collections.Generic;
using Core;
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
		LeftMove,
		RightMove,
		UpMove,
		DownMove,
		LeftAttack,
		RightAttack,
		UpAttack,
		DownAttack,
		Skill,
	}

	public class Input
	{
		public List<Action> actions;
		public KeyCode keyCode;
	}

	public class InputManager : Manager
	{
		private Dictionary<InputTarget, Input> _inGameInputs = new();

		public override void Update()
		{
			foreach (var input in _inGameInputs)
			{
				if (UnityEngine.Input.GetKeyDown(input.Value.keyCode))
				{
					input.Value.actions[(int)InputStatus.Press]?.Invoke();
				}
				if (UnityEngine.Input.GetKey(input.Value.keyCode))
				{
					input.Value.actions[(int)InputStatus.Hold]?.Invoke();
				}
				if (UnityEngine.Input.GetKeyUp(input.Value.keyCode))
				{
					input.Value.actions[(int)InputStatus.Release]?.Invoke();
				}
			}
		}

		public override void Awake()
		{
			InitInGameInput(InputTarget.LeftMove, new Input() { keyCode = KeyCode.A});
			InitInGameInput(InputTarget.RightMove, new Input() { keyCode = KeyCode.D});
			InitInGameInput(InputTarget.UpMove, new Input() { keyCode = KeyCode.W});
			InitInGameInput(InputTarget.DownMove, new Input() { keyCode = KeyCode.S});
		}

		public void InitInGameInput(InputTarget target, Input input)
		{
			_inGameInputs.Add(target, input);
		}
		
		public void ChangeInGameAction(InputTarget target, InputStatus status, Action action)
		{
			_inGameInputs[target].actions[(int)status] = action;
		}

		public void AddInGameAction(InputTarget target, InputStatus status, Action action)
		{
			_inGameInputs[target].actions[(int)status] += action;
		}

		public void RemoveInGameAction(InputTarget target, InputStatus status, Action action)
		{
			_inGameInputs[target].actions[(int)status] -= action;
		}

		public void ChangeInGameKey(InputTarget target, KeyCode keyCode)
		{
			_inGameInputs[target].keyCode = keyCode;
		}
	}
}