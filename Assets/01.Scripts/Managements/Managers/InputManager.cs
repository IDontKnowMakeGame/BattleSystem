using System;
using System.Collections.Generic;
using System.Linq;
using Core;
using Managements.Managers.Base;
using UnityEngine;

namespace Managements.Managers
{
	public enum KeyboardInput
	{
		MoveForward,
		MoveBackward,
		MoveLeft,
		MoveRight,
		AttackForward,
		AttackBackward,
		AttackLeft,
		AttackRight,
		Skill
	}
	
	[Serializable]
	public struct KeyboardInputData
	{
		public KeyboardInput keyboardInput;
		public KeyCode keyCode;
	}
	
	public class InputManager : Manager
	{
		public static event Action<Vector3> OnMovePress;
		public static event Action<Vector3> OnMoveHold;
		public static event Action<Vector3> OnMoveRelease;

		private List<KeyboardInputData> _keyboardInputDatas = new()
		{
			new KeyboardInputData() { keyboardInput = KeyboardInput.MoveForward, keyCode = KeyCode.UpArrow },
			new KeyboardInputData() { keyboardInput = KeyboardInput.MoveBackward, keyCode = KeyCode.DownArrow },
			new KeyboardInputData() { keyboardInput = KeyboardInput.MoveLeft, keyCode = KeyCode.LeftArrow },
			new KeyboardInputData() { keyboardInput = KeyboardInput.MoveRight, keyCode = KeyCode.RightArrow },
			new KeyboardInputData() { keyboardInput = KeyboardInput.AttackForward, keyCode = KeyCode.W },
			new KeyboardInputData() { keyboardInput = KeyboardInput.AttackBackward, keyCode = KeyCode.S },
			new KeyboardInputData() { keyboardInput = KeyboardInput.AttackLeft, keyCode = KeyCode.A },
			new KeyboardInputData() { keyboardInput = KeyboardInput.AttackRight, keyCode = KeyCode.D },
			new KeyboardInputData() { keyboardInput = KeyboardInput.Skill, keyCode = KeyCode.Space }
		};

		public override void Awake()
		{
			
		}

		public override void Update()
		{
			InputPress();
			InputHold();
			InputRelease();
		}

		private void InputPress()
		{
			// Press
			if (Input.GetKey(GetKeyCode(KeyboardInput.MoveForward)))
			{
				OnMovePress?.Invoke(Vector3.forward);
			}
			if (Input.GetKey(GetKeyCode(KeyboardInput.MoveBackward)))
			{
				OnMovePress?.Invoke(Vector3.back);
			}
			if (Input.GetKey(GetKeyCode(KeyboardInput.MoveLeft)))
			{
				OnMovePress?.Invoke(Vector3.left);
			}
			if (Input.GetKey(GetKeyCode(KeyboardInput.MoveRight)))
			{
				OnMovePress?.Invoke(Vector3.right);
			}
		}
		
		private void InputRelease()
		{
			// Release
			if (Input.GetKeyUp(GetKeyCode(KeyboardInput.MoveForward)))
			{
				OnMoveRelease?.Invoke(Vector3.forward);
			}
			if (Input.GetKeyUp(GetKeyCode(KeyboardInput.MoveBackward)))
			{
				OnMoveRelease?.Invoke(Vector3.back);
			}
			if (Input.GetKeyUp(GetKeyCode(KeyboardInput.MoveLeft)))
			{
				OnMoveRelease?.Invoke(Vector3.left);
			}
			if (Input.GetKeyUp(GetKeyCode(KeyboardInput.MoveRight)))
			{
				OnMoveRelease?.Invoke(Vector3.right);
			}
		}

		private void InputHold()
		{
			// Hold
			if (Input.GetKey(GetKeyCode(KeyboardInput.MoveForward)))
			{
				OnMoveHold?.Invoke(Vector3.forward);
			}
			if (Input.GetKey(GetKeyCode(KeyboardInput.MoveBackward)))
			{
				OnMoveHold?.Invoke(Vector3.back);
			}

			if (Input.GetKey(GetKeyCode(KeyboardInput.MoveLeft)))
			{
				OnMoveHold?.Invoke(Vector3.left);
			}
		}
		
		public KeyCode GetKeyCode(KeyboardInput input)
		{
			return (from keyboardInputData in _keyboardInputDatas where keyboardInputData.keyboardInput == input select keyboardInputData.keyCode).FirstOrDefault();
		}
		
		public void ChangeKeyCode(KeyboardInput input, KeyCode keyCode)
		{
			var keyboardInputData = _keyboardInputDatas.FirstOrDefault(x => x.keyboardInput == input);
			keyboardInputData.keyCode = keyCode;
		}
	}
}