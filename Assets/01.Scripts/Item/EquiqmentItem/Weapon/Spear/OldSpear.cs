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
			_stat.AddDrainageAtk("OldSpear",2);
			_characterActor.GetAct<CharacterStatAct>().StatChange();
		}
		else if(count == 3)
		{
			count = 0;
			GameObject obj = Define.GetManager<ResourceManager>().Instantiate("Hit3");
			obj.transform.position = _characterActor.Position + _currentVec + Vector3.up + -_currentVec/2;
			_stat.DelDrainageAtk("OldSpear");
			_characterActor.GetAct<CharacterStatAct>().StatChange();
		}
	}
}
