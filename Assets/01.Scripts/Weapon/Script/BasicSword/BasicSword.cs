using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Manager;
using Unit.Player;

public class BasicSword : Weapon
{
	public override void Awake()
	{
		base.Awake();
	}

	public override void Update()
	{
		Move();
		Attack();
		Timer();
		Skill();
	}

	protected override void Move()
	{
		if (_inputManager.GetKeyDownInput(InputManager.InputSignal.MoveForward))
		{
			((PlayerMove)_move).InputMovement(Vector3.forward, _basicData.Speed);
			Debug.Log(_move);
		}
		if (_inputManager.GetKeyDownInput(InputManager.InputSignal.MoveBackward))
		{
			((PlayerMove)_move).InputMovement(Vector3.back, _basicData.Speed);
		}
		if (_inputManager.GetKeyDownInput(InputManager.InputSignal.MoveLeft))
		{
			((PlayerMove)_move).InputMovement(Vector3.left, _basicData.Speed);
		}
		if (_inputManager.GetKeyDownInput(InputManager.InputSignal.MoveRight))
		{
			((PlayerMove)_move).InputMovement(Vector3.right, _basicData.Speed);
		}
	}

	protected override void Attack()
	{
		if (_inputManager.GetKeyDownInput(InputManager.InputSignal.FowardAttack))
		{
			_attack.Attack(Vector3.forward, _basicData.attackSpeed, _basicData.damage);
		}
		if (_inputManager.GetKeyDownInput(InputManager.InputSignal.BackwardAttack))
		{
			_attack.Attack(Vector3.back, _basicData.attackSpeed, _basicData.damage);
		}
		if (_inputManager.GetKeyDownInput(InputManager.InputSignal.LeftAttack))
		{
			_attack.Attack(Vector3.left, _basicData.attackSpeed, _basicData.damage);
		}
		if (_inputManager.GetKeyDownInput(InputManager.InputSignal.RightAttack))
		{
			_attack.Attack(Vector3.right, _basicData.attackSpeed, _basicData.damage);
		}
	}
}
