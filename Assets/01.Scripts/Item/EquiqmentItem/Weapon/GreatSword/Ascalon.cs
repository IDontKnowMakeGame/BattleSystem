using Actors.Characters.Player;
using Acts.Characters.Player;
using Core;
using Managements;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ascalon : GreatSword
{
	public override void Skill()
	{
		if (_isCoolTime)
			return;

		_isCoolTime = true;

		_stat.PercentAtk(30);

		PlayerAttack.OnAttackEnd += SkillEnd;
	}

	public override void Update()
	{
		base.Update();
		Debug.Log(_stat.Half);
	}

	private void SkillEnd(int id)
	{
		if (id != _characterActor.UUID)
			return;

		_stat.PercentAtk(-30);
		GameObject obj = GameManagement.Instance.GetManager<ResourceManager>().Instantiate("Dragon Slayer's Realm");
		obj.transform.position = _characterActor.Position + InGame.CamDirCheck(_currrentVector) + (Vector3.up / 2);
		obj.GetComponent<DragonRealm>().Init(AscalonData.duration, AscalonData.decrease);
		PlayerAttack.OnAttackEnd -= SkillEnd;
	}
}
