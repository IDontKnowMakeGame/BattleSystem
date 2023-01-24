using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IDamaged
{
	public float Half { get; set; }

	public void Damaged(float damage);
	public void Die();
}
