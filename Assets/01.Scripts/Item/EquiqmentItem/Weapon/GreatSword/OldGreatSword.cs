using Actors.Bases;
using Actors.Characters;
using Core;
using System.Collections;
using UnityEngine;
using UnityEngine.Rendering.Universal;

public class OldGreatSword : GreatSword
{
	private GameObject particleObj;

	public override void Skill(Vector3 vec)
	{
		if (_isCoolTime)
			return;

		_isCoolTime = true;
		_characterActor.StartCoroutine(HalfSkill());
		particleObj = Define.GetManager<ResourceManager>().Instantiate("OldGreatSwordAura").transform.gameObject;
		particleObj.transform.position = _characterActor.transform.position;
		EventParam eventParam = new EventParam();
		eventParam.intParam = 1;
		eventParam.stringParam = "OldGreatSkill";
		Define.GetManager<EventManager>().TriggerEvent(EventFlag.PlayTimeLine, eventParam);
	}

	private IEnumerator HalfSkill()
	{
		_characterActor.GetAct<CharacterStatAct>().Half += 30;
		yield return new WaitForSeconds(0.5f);
		_characterActor.GetAct<CharacterStatAct>().Half -= 30;
		Define.GetManager<ResourceManager>().Destroy(particleObj);
	}
}
