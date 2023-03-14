using Actor.Acts;
using Actor.Bases;
using Core;
using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActorChange : Act
{
	private ItemID firstWeapon => _actController.WeaponId;
	[SerializeField]
	private ItemID secoundWeapon;

	protected ActorController _actController = null;
	public virtual void Change()
	{
		if (_actController == null)
			_actController = _controller as ActorController;

		Weapon weapon = Define.GetManager<ItemManager>().GetWeapon(secoundWeapon);

		if (weapon == null)
			return;

		secoundWeapon = firstWeapon;

		weapon.Init(_actController);
		weapon.UseItem();

		_actController.weapon = weapon;
		_actController.WeaponId = weapon.itemInfo.Id;
	}
}
