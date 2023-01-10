using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BasicTwinSword : Weapon
{
	public override void Awake()
	{
		base.Awake();
	}

	public override void Start()
	{
		base.Start();
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
		if (_inputManager.GetKeyDownInput(InputManager.InputSignal.MoveForward))
		{
			_attack.Attack(Vector3.left + Vector3.forward,.1f);
			_attack.Attack(Vector3.right + Vector3.forward,.1f);
		}
		if (_inputManager.GetKeyDownInput(InputManager.InputSignal.MoveBackward))
		{
			_attack.Attack(Vector3.left + Vector3.back,.1f);
			_attack.Attack(Vector3.right + Vector3.back,.1f);
		}
		if (_inputManager.GetKeyDownInput(InputManager.InputSignal.MoveLeft))
		{
			_attack.Attack(Vector3.forward + Vector3.left,.1f);
			_attack.Attack(Vector3.back + Vector3.left,.1f);
		}
		if (_inputManager.GetKeyDownInput(InputManager.InputSignal.MoveRight))
		{
			_attack.Attack(Vector3.forward + Vector3.right,.1f);
			_attack.Attack(Vector3.back + Vector3.right,.1f);
		}
	}
}
