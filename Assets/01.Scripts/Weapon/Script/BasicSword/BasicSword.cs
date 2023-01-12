using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Manager;
//using Unit.Player;

public class BasicSword : Weapon
{
	public override void Start()
	{
		base.Start();
	}

	public override void Update()
	{
		if(!_isEnemy)
		{
			Skill();
			Move(Vector3.zero);
			Attack(Vector3.zero);
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
		if (!_isEnemy)
		{
			if (_inputManager.GetKeyDownInput(InputManager.InputSignal.FowardAttack))
			{
				_attack.Attack(Vector3.forward, _basicData.attackSpeed, _basicData.attackAfterDelay, _basicData.damage);
			}
			if (_inputManager.GetKeyDownInput(InputManager.InputSignal.BackwardAttack))
			{
				_attack.Attack(Vector3.back, _basicData.attackSpeed, _basicData.attackAfterDelay, _basicData.damage);
			}
			if (_inputManager.GetKeyDownInput(InputManager.InputSignal.LeftAttack))
			{
				_attack.Attack(Vector3.left, _basicData.attackSpeed, _basicData.attackAfterDelay, _basicData.damage);
			}
			if (_inputManager.GetKeyDownInput(InputManager.InputSignal.RightAttack))
			{
				_attack.Attack(Vector3.right, _basicData.attackSpeed, _basicData.attackAfterDelay, _basicData.damage);
			}
		}
		else
			_attack.Attack(vec, _basicData.attackSpeed, _basicData.attackAfterDelay, _basicData.damage);
	}
}
