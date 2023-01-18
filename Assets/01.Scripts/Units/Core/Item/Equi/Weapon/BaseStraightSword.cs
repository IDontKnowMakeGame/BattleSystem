using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unit.Core.Weapon;

public class BaseStraightSword : Weapon
{
	public override void Start()
	{
		base.Start();
	}
	protected override void Move(Vector3 vec)
	{
		if (isSkill)
			return;
		if (!_isEnemy)
		{
			if (Input.GetKeyDown(KeyCode.UpArrow))
			{
				_unitMove.Translate(Vector3.forward);
			}
			if (Input.GetKeyDown(KeyCode.DownArrow))
			{
				_unitMove.Translate(Vector3.back);
			}
			if (Input.GetKeyDown(KeyCode.LeftArrow))
			{
				_unitMove.Translate(Vector3.left);
			}
			if (Input.GetKeyDown(KeyCode.RightArrow))
			{
				_unitMove.Translate(Vector3.right);
			}
		}
		else
			_unitMove.Translate(vec);
	}

	protected override void Attack(Vector3 vec)
	{
		if (!_isEnemy)
		{
			if (Input.GetKeyDown(KeyCode.W))
			{
				_unitAttack.Attack(Vector3.forward);
			}
			if (Input.GetKeyDown(KeyCode.S))
			{
				_unitAttack.Attack(Vector3.back);
			}
			if (Input.GetKeyDown(KeyCode.A))
			{
				_unitAttack.Attack(Vector3.left);
			}
			if (Input.GetKeyDown(KeyCode.D))
			{
				_unitAttack.Attack(Vector3.right);
			}
		}
		else
			_unitAttack.Attack(vec);
	}
}
