using System.Collections;
using UnityEngine;

public class OldGreatSword : GreatSword
{
	public override void Skill(Vector3 vec)
	{
		if (_isCoolTime)
			return;

		_isCoolTime = true;
		_characterActor.StartCoroutine(HalfSkill());
	}

	private IEnumerator HalfSkill()
	{
		_characterActor.GetAct<CharacterStatAct>().Half += 30;
		yield return new WaitForSeconds(0.5f);
		_characterActor.GetAct<CharacterStatAct>().Half -= 30;
	}
}
