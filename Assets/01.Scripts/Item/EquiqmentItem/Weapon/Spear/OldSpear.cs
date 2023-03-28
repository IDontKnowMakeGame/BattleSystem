using Actors.Characters;
using Acts.Characters;
using Core;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OldSpear : Spear
{
	private int count=0;
	private float beforeAtk = 0;
	public override void Equiqment(CharacterActor actor)
	{
		base.Equiqment(actor);
		CharacterAttack.OnAttackEnd += AttackUp;
	}

	public override void UnEquipment(CharacterActor actor)
	{
		base.UnEquipment(actor);
		CharacterAttack.OnAttackEnd -= AttackUp;
	}

	private void AttackUp(int id)
	{
		if (id != _characterActor.UUID)
			return;

		count++;
		if (count == 2)
		{
			beforeAtk = WeaponInfo.Atk;
			_weaponBuffInfo.Atk += WeaponInfo.Atk;
		}
		else if(count == 3)
		{
			Debug.Log(_weaponBuffInfo.Atk);
			Debug.Log(beforeAtk);
			_weaponBuffInfo.Atk -= beforeAtk;
			Debug.Log(_weaponBuffInfo.Atk);
			count = 0;
		}
	}
}
