using Actor.Acts;
using Core;
using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActorChange : Act
{
	public virtual void Change()
	{
		_actorController.weapon = Define.GetManager<ItemManager>().weapons[_actorController.WeaponId];
		_actorController.weapon.UseItem();
	}
}
