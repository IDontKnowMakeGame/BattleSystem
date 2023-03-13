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
		var controller = _controller as ActorController;
		weapon.Init(controller);
		weapon.UseItem();
		controller.WeaponId = weapon.itemInfo.Id;
		controller.weapon = weapon;
	}
}
