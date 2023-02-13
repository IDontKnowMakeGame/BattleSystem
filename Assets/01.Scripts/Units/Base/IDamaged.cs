using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Units.Base.Unit;

public interface IDamaged
{
	public float Half { get; set; }

	public void Damaged(float damage, UnitBase giveUnit);
	public void Die();
}
