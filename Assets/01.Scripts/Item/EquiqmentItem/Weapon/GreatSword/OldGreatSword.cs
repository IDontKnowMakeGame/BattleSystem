using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class OldGreatSword : GreatSword
{
	public override void Skill()
	{
		_characterActor.StartCoroutine(HalfSkill());
	}

	private IEnumerator HalfSkill()
	{
		_playerActor.AddAct<CharacterStatAct>().Half += 30;
		yield return new WaitForSeconds(0.5f);
		_playerActor.AddAct<CharacterStatAct>().Half -= 30;
	}
}
