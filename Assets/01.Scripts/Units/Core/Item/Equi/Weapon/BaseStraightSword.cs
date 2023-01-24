using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unit.Core.Weapon;
using Managements.Managers;

public class BaseStraightSword : Weapon
{
	public override void Start()
	{
		base.Start();
		if (!_isEnemy)
		{
			_inputManager.ChangeInGameKey(InputTarget.UpMove, KeyCode.UpArrow);
			_inputManager.ChangeInGameKey(InputTarget.DownMove, KeyCode.DownArrow);
			_inputManager.ChangeInGameKey(InputTarget.LeftMove, KeyCode.LeftArrow);
			_inputManager.ChangeInGameKey(InputTarget.RightMove, KeyCode.RightArrow);

			_inputManager.ChangeInGameKey(InputTarget.UpAttack, KeyCode.W);
			_inputManager.ChangeInGameKey(InputTarget.DownAttack, KeyCode.S);
			_inputManager.ChangeInGameKey(InputTarget.LeftAttack, KeyCode.A);
			_inputManager.ChangeInGameKey(InputTarget.RightAttack, KeyCode.D);
		}
	}
	protected override void Move(Vector3 vec)
	{
		if (isSkill)
			return;

		_unitMove.Translate(vec);
	}

	protected override void Attack(Vector3 vec)
	{
		_unitAttack.Attack(vec);
	}
}
