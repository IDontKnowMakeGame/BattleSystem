using Manager;
using System.Collections;
using UnityEngine;

public class BasicTwinSword : Weapon
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
		if (isSkill)
			return;

		if (_inputManager.GetKeyDownInput(InputManager.InputSignal.MoveForward))
		{
			_move.Translate(Vector3.forward, _basicData.Speed);
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
		if (isSkill)
			return;

		if (_inputManager.GetKeyDownInput(InputManager.InputSignal.MoveForward))
		{
			_attack.Attack(Vector3.left + Vector3.forward, _basicData.attackSpeed, _basicData.attackAfterDelay, _basicData.damage);
			_attack.Attack(Vector3.right + Vector3.forward, _basicData.attackSpeed, _basicData.attackAfterDelay, _basicData.damage);
		}
		if (_inputManager.GetKeyDownInput(InputManager.InputSignal.MoveBackward))
		{
			_attack.Attack(Vector3.left + Vector3.back, _basicData.attackSpeed, _basicData.attackAfterDelay, _basicData.damage);
			_attack.Attack(Vector3.right + Vector3.back, _basicData.attackSpeed, _basicData.attackAfterDelay, _basicData.damage);
		}
		if (_inputManager.GetKeyDownInput(InputManager.InputSignal.MoveLeft))
		{
			_attack.Attack(Vector3.forward + Vector3.left, _basicData.attackSpeed, _basicData.attackAfterDelay, _basicData.damage);
			_attack.Attack(Vector3.back + Vector3.left, _basicData.attackSpeed, _basicData.attackAfterDelay, _basicData.damage);
		}
		if (_inputManager.GetKeyDownInput(InputManager.InputSignal.MoveRight))
		{
			_attack.Attack(Vector3.forward + Vector3.right, _basicData.attackSpeed, _basicData.attackAfterDelay, _basicData.damage);
			_attack.Attack(Vector3.back + Vector3.right, _basicData.attackSpeed, _basicData.attackAfterDelay, _basicData.damage);
		}
	}
}
