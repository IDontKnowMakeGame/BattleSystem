using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RustySpear : BaseSpear
{
	public override void Awake()
	{
		base.Awake();
		GetWeaponStateData("rustySpear");
	}
}
