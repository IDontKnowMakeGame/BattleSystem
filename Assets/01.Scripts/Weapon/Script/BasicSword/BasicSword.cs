using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Manager;

public class BasicSword : Weapon
{
	public override void Awake()
	{
		base.Awake();
	}

	public override void Update()
	{
		Timer();
		Skill();
		Move();
		Attack();
	}

	protected override void Move()
	{
		if (_inputManager.GetKeyDownInput(InputManager.InputSignal.MoveForward))
		{
			_move.Translate(Vector3.forward, _basicData.Speed);
			Debug.Log(_basicData.Speed);
		}
		if (_inputManager.GetKeyDownInput(InputManager.InputSignal.MoveBackward))
		{
			_move.Translate(Vector3.back, _basicData.Speed);
		}
		if (_inputManager.GetKeyDownInput(InputManager.InputSignal.MoveLeft))
		{
			_move.Translate(Vector3.left, _basicData.Speed);
		}
		if (_inputManager.GetKeyDownInput(InputManager.InputSignal.MoveRight))
		{
			_move.Translate(Vector3.right, _basicData.Speed);
		}
	}

	protected override void Attack()
	{
		if (_inputManager.GetKeyDownInput(InputManager.InputSignal.FowardAttack))
		{
			_attack.Attack(Vector3.forward, _basicData.attackSpeed);
		}
		if (_inputManager.GetKeyDownInput(InputManager.InputSignal.BackwardAttack))
		{
			_attack.Attack(Vector3.back, _basicData.attackSpeed);
		}
		if (_inputManager.GetKeyDownInput(InputManager.InputSignal.LeftAttack))
		{
			_attack.Attack(Vector3.left, _basicData.attackSpeed);
		}
		if (_inputManager.GetKeyDownInput(InputManager.InputSignal.RightAttack))
		{
			_attack.Attack(Vector3.right, _basicData.attackSpeed);
		}
	}
}
