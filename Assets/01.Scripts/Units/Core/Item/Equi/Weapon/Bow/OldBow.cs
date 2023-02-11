using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OldBow : BaseBow
{
	public override void Awake()
	{
		base.Awake();
		GetWeaponStateData("oldBow");
	}
}
