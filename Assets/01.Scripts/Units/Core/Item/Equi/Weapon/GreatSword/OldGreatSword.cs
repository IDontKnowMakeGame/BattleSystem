using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Managements.Managers;
public class OldGreatSword : BaseGreatSword
{
	public override void Awake()
	{
		base.Awake();
		GetWeaponStateData("oldGreatSword");
	}
	public override void Start()
	{
		base.Start();
		_inputManager.ChangeInGameAction(InputTarget.Skill, InputStatus.Press, () => Skill(Vector3.zero));
	}

	protected override void Skill(Vector3 vec)
	{
		if (_isCoolTime || isSkill)
			return;

		_thisBase.StartCoroutine(HoldOn());
	}

	private IEnumerator HoldOn()
	{
		isSkill = true;
		_unitStat.Half = 30;
		yield return new WaitForSeconds(1.5f);
		_unitStat.Half = 0;
		isSkill = false;
		_isCoolTime = true;
	}
}
