using Managements.Managers;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BrokenSword : BaseStraightSword
{
	public override void ChangeKey()
	{
		base.ChangeKey();
		InputManager.OnSkillPress -= Skill;
	}
	protected override void Skill()
	{

	}
	
	public override void Awake()
	{
		base.Awake();
		GetWeaponStateData("brokenSword");
	}
}
