using Managements.Managers;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TwinSword : Weapon
{
	public override void Init()
	{
		InputManager<StraightSword>.OnAttackPress += Attack;
	}
	public override void LoadWeaponClassLevel()
	{

	}

	public override void LoadWeaponLevel()
	{

	}
	public virtual void Attack(Vector3 vec)
	{

	}
}
