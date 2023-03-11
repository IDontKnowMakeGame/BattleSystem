using Actor.Acts;
using Core;
using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActorChange : Act
{
	public void Change()
	{
		Define.GetManager<ItemManager>().weapons[_actorController.WeaponId].UseItem();
	}
}
