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
        FowardAttack,
        BackwardAttack,
        LeftAttack,
        RightAttack,
        Skill,
        TestWeaponChange,
        TestHit
    }
    
    private Dictionary<InputSignal, KeyCode> _inputMap;
    
    public InputManager()
    {
        _inputMap = new Dictionary<InputSignal, KeyCode>();
        _inputMap.Add(InputSignal.MoveForward, KeyCode.UpArrow);
        _inputMap.Add(InputSignal.MoveBackward, KeyCode.DownArrow);
        _inputMap.Add(InputSignal.MoveLeft, KeyCode.LeftArrow);
        _inputMap.Add(InputSignal.MoveRight, KeyCode.RightArrow);
        _inputMap.Add(InputSignal.FowardAttack, KeyCode.W);
        _inputMap.Add(InputSignal.LeftAttack, KeyCode.A);
        _inputMap.Add(InputSignal.RightAttack, KeyCode.D);
        _inputMap.Add(InputSignal.BackwardAttack, KeyCode.S);
        _inputMap.Add(InputSignal.Skill, KeyCode.Space);
        _inputMap.Add(InputSignal.TestWeaponChange, KeyCode.T);
        _inputMap.Add(InputSignal.TestHit, KeyCode.P);
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
