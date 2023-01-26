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
		public readonly List<Action> Actions = new(3);
		public KeyCode KeyCode;
	}

	public class InputManager : Manager
	{
		private Dictionary<InputTarget, Input> _inGameInputs = new();

		public override void Update()
		{
			foreach (var input in _inGameInputs)
			{
				if (UnityEngine.Input.GetKeyDown(input.Value.KeyCode))
				{
					input.Value.Actions[(int)InputStatus.Press]?.Invoke();
				}
				if (UnityEngine.Input.GetKey(input.Value.KeyCode))
				{
					input.Value.Actions[(int)InputStatus.Hold]?.Invoke();
				}
				if (UnityEngine.Input.GetKeyUp(input.Value.KeyCode))
				{
					input.Value.Actions[(int)InputStatus.Release]?.Invoke();
				}
			}
		}

		public override void Awake()
		{
			InitInGameInput(InputTarget.LeftMove, KeyCode.LeftArrow);
			InitInGameInput(InputTarget.RightMove, KeyCode.RightArrow );
			InitInGameInput(InputTarget.UpMove, KeyCode.UpArrow );
			InitInGameInput(InputTarget.DownMove, KeyCode.DownArrow );
			InitInGameInput(InputTarget.LeftAttack, KeyCode.A );
			InitInGameInput(InputTarget.RightAttack, KeyCode.D );
			InitInGameInput(InputTarget.UpAttack, KeyCode.W );
			InitInGameInput(InputTarget.DownAttack, KeyCode.S );
			InitInGameInput(InputTarget.Skill, KeyCode.Space );
		}

		public void InitInGameInput(InputTarget target, KeyCode keyCode)
		{
			var input = new Input() { KeyCode = keyCode, Actions = { null, null, null }};
			_inGameInputs.Add(target, input);
		}
		
		public void ChangeInGameAction(InputTarget target, InputStatus status, Action action)
		{
			_inGameInputs[target].Actions[(int)status] = action;
		}

		public void AddInGameAction(InputTarget target, InputStatus status, Action action)
		{
			_inGameInputs[target].Actions[(int)status] += action;
		}

		public void RemoveInGameAction(InputTarget target, InputStatus status, Action action)
		{
			_inGameInputs[target].Actions[(int)status] -= action;
		}

		public void ChangeInGameKey(InputTarget target, KeyCode keyCode)
		{
			_inGameInputs[target].KeyCode = keyCode;
		}
		
		public void ClearInGameAction(InputTarget target)
		{
			//_inGameInputs[target].Actions.Clear();
			for (int i = 0; i<3; i++)
			{
				_inGameInputs[target].Actions[i] = null;
			}
		}
	}
}