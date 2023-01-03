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
        _inputMap.Add(InputSignal.MoveForward, KeyCode.UpArrow);
        _inputMap.Add(InputSignal.MoveBackward, KeyCode.DownArrow);
        _inputMap.Add(InputSignal.MoveLeft, KeyCode.LeftArrow);
        _inputMap.Add(InputSignal.MoveRight, KeyCode.RightArrow);
        _inputMap.Add(InputSignal.Attack, KeyCode.Z);
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
