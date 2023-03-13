using Actor.Acts;
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
		var actorCon = _controller as ActorController;
		actorCon.weapon = Define.GetManager<ItemManager>().weapons[actorCon.WeaponId];
		actorCon.weapon.UseItem();
	}
}
