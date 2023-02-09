using Core;
using System.Collections;
using System.Collections.Generic;
using Units.Base.Player;
using Units.Behaviours.Unit;
using UnityEngine;

public class Arrow
{
	public GameObject thisObject;

	public bool isStick;

	public virtual void Start()
	{
		
	}
	public virtual void Stick(EnemyBase enemyBase)
	{
		enemyBase.transform.parent = thisObject.transform;
		isStick = true;
	}

	public virtual void PullOut(PlayerBase playerBase)
	{
		isStick = false;
		BaseBow arrow = playerBase.GetBehaviour<UnitEquiq>().CurrentWeapon as BaseBow;
		arrow.hasArrow = true;
		Define.GetManager<ResourceManagers>().Destroy(thisObject);
	}
}
