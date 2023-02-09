using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Units.Base;
using Units.Behaviours.Unit;

public class ArrowCollider : MonoBehaviour
{
	public void OnTriggerEnter2D(Collider2D collision)
	{
		var a = collision.GetComponent<Units.Base.Units>();
		if(a as EnemyBase)
		{
			//a.GetBehaviour<UnitStat>().Damaged()
		}
	}
}
