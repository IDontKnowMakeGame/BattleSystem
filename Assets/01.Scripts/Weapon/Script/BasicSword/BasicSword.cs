using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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
		Skii();
	}

	protected override void Move()
	{
		if (_inputManager.GetKeyDownInput(InputManager.InputSignal.MoveForward))
		{
			_move.Translate(Vector3.forward);
		}
		if (_inputManager.GetKeyDownInput(InputManager.InputSignal.MoveBackward))
		{
			_move.Translate(Vector3.back);
		}
		if (_inputManager.GetKeyDownInput(InputManager.InputSignal.MoveLeft))
		{
			_move.Translate(Vector3.left);
		}
		if (_inputManager.GetKeyDownInput(InputManager.InputSignal.MoveRight))
		{
			_move.Translate(Vector3.right);
		}
	}

	protected override void Attack()
	{
		if (_inputManager.GetKeyDownInput(InputManager.InputSignal.FowardAttack))
		{
			_attack.Attack(Vector3.forward);
		}
		if (_inputManager.GetKeyDownInput(InputManager.InputSignal.BackwardAttack))
		{
			_attack.Attack(Vector3.back);
		}
		if (_inputManager.GetKeyDownInput(InputManager.InputSignal.LeftAttack))
		{
			_attack.Attack(Vector3.left);
		}
		if (_inputManager.GetKeyDownInput(InputManager.InputSignal.RightAttack))
		{
			_attack.Attack(Vector3.right);
		}
	}
}
