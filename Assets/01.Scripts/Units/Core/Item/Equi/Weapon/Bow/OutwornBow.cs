using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OutwornBow : BaseBow
{
	public override void Awake()
	{
		base.Awake();
		GetWeaponStateData("outwornBow");
	}
}
