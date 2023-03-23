using Actors.Characters;
using Core;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OldSpear : Spear
{
	private int count=0;

	public override void Equiqment(CharacterActor actor)
	{
		base.Equiqment(actor);
		Define.GetManager<EventManager>().StartListening(EventFlag.Attack, AttackUp);
	}

	public override void UnEquipment(CharacterActor actor)
	{
		base.UnEquipment(actor);
		Define.GetManager<EventManager>().StopListening(EventFlag.Attack, AttackUp);
	}

	private void AttackUp(EventParam eventP)
	{
		count++;
		if (count == 2)
			_weaponBuffInfo.Atk = WeaponInfo.Atk;
		else
		{
			_weaponLevelInfo.Atk = 0;
			count = 0;
		}
	}
}
