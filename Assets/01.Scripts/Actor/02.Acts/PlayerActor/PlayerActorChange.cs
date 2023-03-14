using Actor.Bases;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerActorChange : ActorChange
{
	private PlayerController playerController = null;
	public override void Change()
	{
		base.Change();

		if (playerController == null)
			playerController = _actController as PlayerController;

		playerController.OnSkill = _actController.weapon.Skill;
	}
}
