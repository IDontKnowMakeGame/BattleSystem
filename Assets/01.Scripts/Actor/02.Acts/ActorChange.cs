using Actor.Acts;
using Actor.Bases;
using Core;
using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActorChange : Act
{
	[SerializeField]
	private ItemID firstWeapon;
	[SerializeField]
	private ItemID secoundWeapon;
	public virtual void Change()
	{
		Weapon weapon = Define.GetManager<ItemManager>().weapons[secoundWeapon];
		weapon.Init(_actorController);
		weapon.UseItem();

		secoundWeapon = _actorController.WeaponId;
		_actorController.WeaponId = weapon.itemInfo.Id;
		_actorController.weapon = weapon;
	}
}
