using System;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
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
		Skill,
		SubKey,
		ChangeKey,
		OffKey,
		TestChangeKey,
		Interaction,
		Slot01,
		Slot02,
		Slot03,
		Slot04,
		Slot05,
	}

	[Serializable]
	public class KeyboardInputData
	{
		public KeyboardInput keyboardInput;
		public KeyCode keyCode;
	}


	public class InputManager : Manager
	{
		public static event Action<Vector3> OnMovePress;
		public static event Action<Vector3> OnMoveHold;
		public static event Action<Vector3> OnMoveRelease;

		public static event Action<Vector3> OnAttackPress;
		public static event Action<Vector3> OnAttackHold;
		public static event Action<Vector3> OnAttackRelease;

		public static event Action OnSkillPress;
		public static event Action OnSkillHold;
		public static event Action OnSkillRelease;

		public static event Action OnSubPress;
		public static event Action OnChangePress;
		public static event Action OnOffPress;
		public static event Action OnTestChangePress;
		public static event Action OnInteractionPress;

		public static event Action<int> OnItemPress;


		private static List<KeyboardInputData> _keyboardInputDatas = new()
		{
			new KeyboardInputData() { keyboardInput = KeyboardInput.MoveForward, keyCode = KeyCode.UpArrow },
			new KeyboardInputData() { keyboardInput = KeyboardInput.MoveBackward, keyCode = KeyCode.DownArrow },
			new KeyboardInputData() { keyboardInput = KeyboardInput.MoveLeft, keyCode = KeyCode.LeftArrow },
			new KeyboardInputData() { keyboardInput = KeyboardInput.MoveRight, keyCode = KeyCode.RightArrow },
			new KeyboardInputData() { keyboardInput = KeyboardInput.AttackForward, keyCode = KeyCode.W },
			new KeyboardInputData() { keyboardInput = KeyboardInput.AttackBackward, keyCode = KeyCode.S },
			new KeyboardInputData() { keyboardInput = KeyboardInput.AttackLeft, keyCode = KeyCode.A },
			new KeyboardInputData() { keyboardInput = KeyboardInput.AttackRight, keyCode = KeyCode.D },
			new KeyboardInputData() { keyboardInput = KeyboardInput.Skill, keyCode = KeyCode.Space },
			new KeyboardInputData() { keyboardInput = KeyboardInput.SubKey, keyCode = KeyCode.V },
			new KeyboardInputData() { keyboardInput = KeyboardInput.ChangeKey, keyCode = KeyCode.R },
			new KeyboardInputData() { keyboardInput = KeyboardInput.OffKey, keyCode = KeyCode.Q },
			new KeyboardInputData() { keyboardInput = KeyboardInput.TestChangeKey, keyCode = KeyCode.T },
			new KeyboardInputData() { keyboardInput = KeyboardInput.Interaction, keyCode = KeyCode.E },

			new KeyboardInputData() {  keyboardInput = KeyboardInput.Slot01, keyCode = KeyCode.Alpha1},
			new KeyboardInputData() {  keyboardInput = KeyboardInput.Slot02, keyCode = KeyCode.Alpha2},
			new KeyboardInputData() {  keyboardInput = KeyboardInput.Slot03, keyCode = KeyCode.Alpha3},
			new KeyboardInputData() {  keyboardInput = KeyboardInput.Slot04, keyCode = KeyCode.Alpha4},
			new KeyboardInputData() {  keyboardInput = KeyboardInput.Slot05, keyCode = KeyCode.Alpha5},
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
			if (Input.GetKeyDown(GetKeyCode(KeyboardInput.MoveForward)))
			{
				OnMovePress?.Invoke(Vector3.forward);
			}
			if (Input.GetKeyDown(GetKeyCode(KeyboardInput.MoveBackward)))
			{
				OnMovePress?.Invoke(Vector3.back);
			}
			if (Input.GetKeyDown(GetKeyCode(KeyboardInput.MoveLeft)))
			{
				OnMovePress?.Invoke(Vector3.left);
			}
			if (Input.GetKeyDown(GetKeyCode(KeyboardInput.MoveRight)))
			{
				OnMovePress?.Invoke(Vector3.right);
			}

			if (Input.GetKeyDown(GetKeyCode(KeyboardInput.AttackForward)))
			{
				OnAttackPress?.Invoke(Vector3.forward);
			}
			if (Input.GetKeyDown(GetKeyCode(KeyboardInput.AttackBackward)))
			{
				OnAttackPress?.Invoke(Vector3.back);
			}
			if (Input.GetKeyDown(GetKeyCode(KeyboardInput.AttackLeft)))
			{
				OnAttackPress?.Invoke(Vector3.left);
			}
			if (Input.GetKeyDown(GetKeyCode(KeyboardInput.AttackRight)))
			{
				OnAttackPress?.Invoke(Vector3.right);
			}

			if (Input.GetKeyDown(GetKeyCode(KeyboardInput.Skill)))
			{
				OnSkillPress?.Invoke();
			}

			if (Input.GetKeyDown(GetKeyCode(KeyboardInput.SubKey)))
			{
				OnSubPress?.Invoke();
			}

			if (Input.GetKeyDown(GetKeyCode(KeyboardInput.ChangeKey)))
			{
				OnChangePress?.Invoke();
			}

			if (Input.GetKeyDown(GetKeyCode(KeyboardInput.OffKey)))
			{
				OnOffPress?.Invoke();
			}

			if (Input.GetKeyDown(GetKeyCode(KeyboardInput.TestChangeKey)))
			{
				OnTestChangePress?.Invoke();
			}

			if (Input.GetKeyDown(GetKeyCode(KeyboardInput.Interaction)))
			{
				OnInteractionPress?.Invoke();
			}

			if (Input.GetKeyDown(GetKeyCode(KeyboardInput.Slot01)))
			{
				OnItemPress?.Invoke(1);
			}

			if (Input.GetKeyDown(GetKeyCode(KeyboardInput.Slot02)))
			{
				OnItemPress?.Invoke(2);
			}

			if (Input.GetKeyDown(GetKeyCode(KeyboardInput.Slot03)))
			{
				OnItemPress?.Invoke(3);
			}

			if (Input.GetKeyDown(GetKeyCode(KeyboardInput.Slot04)))
			{
				OnItemPress?.Invoke(4);
			}

			if (Input.GetKeyDown(GetKeyCode(KeyboardInput.Slot05)))
			{
				OnItemPress?.Invoke(5);
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

			if (Input.GetKeyUp(GetKeyCode(KeyboardInput.AttackForward)))
			{
				OnAttackRelease?.Invoke(Vector3.forward);
			}
			if (Input.GetKeyUp(GetKeyCode(KeyboardInput.AttackBackward)))
			{
				OnAttackRelease?.Invoke(Vector3.back);
			}
			if (Input.GetKeyUp(GetKeyCode(KeyboardInput.AttackLeft)))
			{
				OnAttackRelease?.Invoke(Vector3.left);
			}
			if (Input.GetKeyUp(GetKeyCode(KeyboardInput.AttackRight)))
			{
				OnAttackRelease?.Invoke(Vector3.right);
			}

			if (Input.GetKeyUp(GetKeyCode(KeyboardInput.Skill)))
			{
				OnSkillRelease?.Invoke();
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

			if (Input.GetKey(GetKeyCode(KeyboardInput.AttackForward)))
			{
				OnAttackHold?.Invoke(Vector3.forward);
			}
			if (Input.GetKey(GetKeyCode(KeyboardInput.AttackBackward)))
			{
				OnAttackHold?.Invoke(Vector3.back);
			}
			if (Input.GetKey(GetKeyCode(KeyboardInput.AttackLeft)))
			{
				OnAttackHold?.Invoke(Vector3.left);
			}
			if (Input.GetKey(GetKeyCode(KeyboardInput.AttackRight)))
			{
				OnAttackHold?.Invoke(Vector3.right);
			}

			if (Input.GetKeyUp(GetKeyCode(KeyboardInput.Skill)))
			{
				OnSkillHold?.Invoke();
			}
		}

		public KeyCode GetKeyCode(KeyboardInput input)
		{
			return (from keyboardInputData in _keyboardInputDatas where keyboardInputData.keyboardInput == input select keyboardInputData.keyCode).FirstOrDefault();
		}

		public static void ChangeKeyCode(KeyboardInput input, KeyCode keyCode)
		{
			for (var i = 0; i < _keyboardInputDatas.Count; i++)
			{
				if (_keyboardInputDatas[i].keyboardInput == input)
				{
					_keyboardInputDatas[i].keyCode = keyCode;
					return;
				}
			}
		}
	}
}