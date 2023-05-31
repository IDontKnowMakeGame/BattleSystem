using Actors.Characters;
using Actors.Characters.Player;
using Acts.Characters.Player;
using Core;
using Managements;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ascalon : GreatSword
{
	private GameObject _obj = null;

	public override void Skill(Vector3 vec)
	{
		if (_isCoolTime)
			return;

		_isCoolTime = true;

		_obj = GameManagement.Instance.GetManager<ResourceManager>().Instantiate("AscalonEffect");
		_obj.transform.SetParent(_characterActor.transform);
		_obj.transform.localPosition = Vector3.zero;

		_stat.PercentAtk(30);

		PlayerAttack.OnAttackEnd += SkillEnd;
	}

	public override void Update()
	{
		base.Update();
	}

	public override void UnEquipment(CharacterActor actor)
	{
		base.UnEquipment(actor);

		if(_obj)
		{
			_obj.transform.SetParent(null);
			GameManagement.Instance.GetManager<ResourceManager>()?.Destroy(_obj);
			PlayerAttack.OnAttackEnd -= SkillEnd;
			_stat.PercentAtk(-30);
		}
	}

	private void SkillEnd(int id)
	{
		if (id != _characterActor.UUID)
			return;

		_obj.transform.SetParent(null);
		GameManagement.Instance.GetManager<ResourceManager>().Destroy(_obj);

		_stat.PercentAtk(-30);
		GameObject obj = GameManagement.Instance.GetManager<ResourceManager>().Instantiate("Dragon Slayer's Realm");
		obj.transform.position = _characterActor.Position + InGame.CamDirCheck(_currrentVector) + (Vector3.up / 2);
		obj.GetComponent<DragonRealm>().Init(AscalonData.duration, AscalonData.decrease);
		PlayerAttack.OnAttackEnd -= SkillEnd;
	}
}
