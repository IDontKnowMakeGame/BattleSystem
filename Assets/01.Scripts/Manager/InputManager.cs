using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputManager : IManager
{
    public enum InputSignal
    {
        MoveForward,
        MoveBackward,
        MoveLeft,
        MoveRight,
        Attack,
    }
    
    private Dictionary<InputSignal, KeyCode> _inputMap;
    
    public InputManager()
    {
        _inputMap = new Dictionary<InputSignal, KeyCode>();
        _inputMap.Add(InputSignal.MoveForward, KeyCode.W);
        _inputMap.Add(InputSignal.MoveBackward, KeyCode.S);
        _inputMap.Add(InputSignal.MoveLeft, KeyCode.A);
        _inputMap.Add(InputSignal.MoveRight, KeyCode.D);
        _inputMap.Add(InputSignal.Attack, KeyCode.Space);
    }
	public bool GetKeyUpInput(InputSignal signal)
	{
		return Input.GetKeyUp(_inputMap[signal]);
	}
    
    public bool GetKeyInput(InputSignal signal)
    {
        return Input.GetKey(_inputMap[signal]);
    }

	public bool GetKeyDownInput(InputSignal signal)
	{
		return Input.GetKeyDown(_inputMap[signal]);
	}

}
