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
		a,
		b
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

		public override void Awake()
		{
			InitMovementInput();
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

		private void InitMovementInput()
		{
			InitInGameInput(InputTarget.a,
				new Input()
				{
					action = () =>
					{
						Debug.Log("a");
					},
					keyCode = KeyCode.P,
					inputStatus = InputStatus.Hold
				}
			);
			
			InitInGameInput(InputTarget.b,
				new Input()
				{
					action = () =>
					{
						Debug.Log("b");
					},
					keyCode = KeyCode.Y,
					inputStatus = InputStatus.Hold
				}
			);
			
			
			
			InitInGameInput(InputTarget.LeftMove,
				new Input()
				{
					action = () =>
					{
						Debug.Log("Left");
					},
					keyCode = KeyCode.LeftArrow,
					inputStatus = InputStatus.Press
				}
			);

			InitInGameInput(InputTarget.RightMove,
				new Input()
				{
					action = () =>
					{
						Debug.Log("Right");
					},
					keyCode = KeyCode.RightArrow,
					inputStatus = InputStatus.Press
				}
			);

			InitInGameInput(InputTarget.UpMove,
				new Input()
				{
					action = () =>
					{
						Debug.Log("Up");
					},
					keyCode = KeyCode.UpArrow,
					inputStatus = InputStatus.Press
				}
			);

			InitInGameInput(InputTarget.DownMove,
				new Input()
				{
					action = () =>
					{
						Debug.Log("Down");
					},
					keyCode = KeyCode.DownArrow,
					inputStatus = InputStatus.Press
				}
			);


			InitInGameInput(InputTarget.LeftAttack,
		new Input()
		{
			action = () =>
			{
				Debug.Log("Left");
			},
			keyCode = KeyCode.LeftArrow,
			inputStatus = InputStatus.Press
		}
	);

			InitInGameInput(InputTarget.RightAttack,
				new Input()
				{
					action = () =>
					{
						Debug.Log("Right");
					},
					keyCode = KeyCode.RightArrow,
					inputStatus = InputStatus.Press
				}
			);

			InitInGameInput(InputTarget.UpAttack,
				new Input()
				{
					action = () =>
					{
						Debug.Log("Up");
					},
					keyCode = KeyCode.UpArrow,
					inputStatus = InputStatus.Press
				}
			);

			InitInGameInput(InputTarget.DownAttack,
				new Input()
				{
					action = () =>
					{
						Debug.Log("Down");
					},
					keyCode = KeyCode.DownArrow,
					inputStatus = InputStatus.Press
				}
			);

			InitInGameInput(InputTarget.Skill,
				new Input()
				{
					action = () =>
					{
						Debug.Log("Down");
					},
					keyCode = KeyCode.Space,
					inputStatus = InputStatus.Press
				}
			);
		}
	}
}