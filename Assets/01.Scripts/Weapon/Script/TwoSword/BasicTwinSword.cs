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
		if(!_isEnemy)
		{
			Skill();
			Attack(Vector3.zero);
			Move(Vector3.zero);
		}
		Timer();
	}

	protected override void Move(Vector3 vec)
	{
		if (isSkill)
			return;

		if (!_isEnemy)
		{
			if (_inputManager.GetKeyDownInput(InputManager.InputSignal.MoveForward))
			{
				_playerMove.InputMovement(Vector3.forward, _basicData.Speed);
			}
			if (_inputManager.GetKeyDownInput(InputManager.InputSignal.MoveBackward))
			{
				_playerMove.InputMovement(Vector3.back, _basicData.Speed);
			}
			if (_inputManager.GetKeyDownInput(InputManager.InputSignal.MoveLeft))
			{
				_playerMove.InputMovement(Vector3.left, _basicData.Speed);
			}
			if (_inputManager.GetKeyDownInput(InputManager.InputSignal.MoveRight))
			{
				_playerMove.InputMovement(Vector3.right, _basicData.Speed);
			}
		}
		else
			_move.Translate(vec, _basicData.Speed);
	}

	protected override void Attack(Vector3 vec)
	{
		if (isSkill)
			return;

		//if (_move.IsMoving())
		//	return;

		if (!_isEnemy)
		{
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
		else
			_attack.Attack(vec, _basicData.attackSpeed, _basicData.attackAfterDelay, _basicData.damage);
	}
}
