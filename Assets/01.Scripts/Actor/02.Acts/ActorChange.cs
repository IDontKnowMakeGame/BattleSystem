using Actor.Acts;
using Actor.Bases;
using Core;
using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using Actor.Bases;
using UnityEngine;

public class ActorChange : Act
{
	public virtual void Change()
	{
		Weapon weapon = Define.GetManager<ItemManager>().weapons[ItemID.OldTwinSword];
		weapon.Init(_actorController);
		weapon.UseItem();
		_actorController.WeaponId = weapon.itemInfo.Id;
		_actorController.weapon = weapon;
	}
}
