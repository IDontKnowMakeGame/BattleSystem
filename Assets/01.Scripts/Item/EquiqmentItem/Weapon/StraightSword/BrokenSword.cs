using Actors.Characters;
using Acts.Characters;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BrokenSword : StraightSword
{
	private int count = 0;
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
		if (count == 99)
		{
			_weaponBuffInfo.Atk += 99999999999999999;
		}
		else if (count == 100)
		{
			_weaponBuffInfo.Atk -= 99999999999999999;
			count = 0;
		}
	}
}
