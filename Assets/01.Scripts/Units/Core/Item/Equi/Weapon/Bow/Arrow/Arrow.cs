using Core;
using System.Collections;
using System.Collections.Generic;
using Units.Base.Player;
using Units.Base;
using Units.Behaviours.Unit;
using UnityEngine;


public class Arrow
{
	public GameObject thisObject;

	public bool isStick;
	public virtual void Start()
	{

	}
	public virtual void Stick(Units.Base.Units unitBase, Vector3 vec)
	{
		StickDir(unitBase, vec);
		isStick = true;
	}

	public virtual void PullOut(Units.Base.Units unitBase)
	{
		isStick = false;
		BaseBow bow = unitBase.GetBehaviour<UnitEquiq>().CurrentWeapon as BaseBow;
		bow.hasArrow = true;
		Define.GetManager<ResourceManagers>().Destroy(thisObject);
	}

	protected virtual void StickDir(Units.Base.Units unitBase, Vector3 vec)
	{
		if (vec == Vector3.zero)
			thisObject.transform.rotation = Quaternion.Euler(30, 0, 0);

		thisObject.transform.parent = unitBase.transform;
		thisObject.transform.localPosition = unitBase is EnemyBase ? Vector3.zero : Vector3.up;
		if (unitBase is BlockBase)
			return;
		thisObject.transform.localPosition -= vec;
	}
}
