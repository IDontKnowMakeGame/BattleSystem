using Actor.Acts;
using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActorChange : Act
{
	public void Change(Weapon weapon)
	{
		weapon?.UseItem();
	}
}
