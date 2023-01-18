using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unit.Core.Weapon;
public class BaseTwinSword : Weapon
{
	public override void Awake()
	{
		base.Awake();
	}

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
			if (Input.GetKeyDown(KeyCode.UpArrow))
			{
				_unitAttack.Attack(Vector3.forward + Vector3.left);
				_unitAttack.Attack(Vector3.forward + Vector3.right);
			}
			if (Input.GetKeyDown(KeyCode.DownArrow))
			{
				_unitAttack.Attack(Vector3.back + Vector3.left);
				_unitAttack.Attack(Vector3.back + Vector3.right);
			}
			if (Input.GetKeyDown(KeyCode.LeftArrow))
			{
				_unitAttack.Attack(Vector3.left + Vector3.forward);
				_unitAttack.Attack(Vector3.left + Vector3.back);
			}
			if (Input.GetKeyDown(KeyCode.RightArrow))
			{
				_unitAttack.Attack(Vector3.right + Vector3.forward);
				_unitAttack.Attack(Vector3.right + Vector3.back);
			}
		}
		else
			_unitAttack.Attack(vec);
	}
}
