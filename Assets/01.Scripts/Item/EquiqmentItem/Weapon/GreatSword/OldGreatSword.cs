using Actors.Bases;
using Actors.Characters;
using Core;
using System.Collections;
using UnityEngine;
using UnityEngine.Rendering.Universal;
using Actors.Characters.Player;
using Acts.Characters.Player;
using Tools;

public class OldGreatSword : GreatSword
{
	private GameObject particleObj;

	public override void Skill(Vector3 vec)
	{
		if (_isCoolTime)
			return;

		if (_playerActor.HasAnyState())
			return;

		_isCoolTime = true;
		_characterActor.GetAct<PlayerAnimation>().Play("Skill");
		_characterActor.GetAct<CharacterStatAct>().Half += 30;

		ClipBase clip = _characterActor.GetAct<PlayerAnimation>().GetClip("Skill");
		clip.SetEventOnFrame(3, () =>
		{
			particleObj = Define.GetManager<ResourceManager>().Instantiate("OldGreatSwordAura").transform.gameObject;
			particleObj.transform.position = _characterActor.transform.position;
			EventParam eventParam = new EventParam();
			eventParam.intParam = 1;
			eventParam.stringParam = "OldGreatSkill";
			Define.GetManager<EventManager>().TriggerEvent(EventFlag.PlayTimeLine, eventParam); ;
		});
		clip.SetEventOnFrame(clip.fps - 1, () =>
		{
			_characterActor.GetAct<CharacterStatAct>().Half -= 30;
			Define.GetManager<ResourceManager>().Destroy(particleObj);
		});
	}
}
