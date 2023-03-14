using Managements.Managers;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class WeaponAttack
{
	public virtual void Setting()
	{
		InputManager.OnAttackPress += Attak;
	}
	public virtual void Attak(Vector3 vec)
	{

	}
	public virtual void Reset()
	{
		InputManager.OnAttackPress += Attak;
	}
}
