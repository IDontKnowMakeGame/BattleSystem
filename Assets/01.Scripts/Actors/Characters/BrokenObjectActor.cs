  using Actors.Characters;
using Actors.Characters.Enemy;
using Acts.Characters;
using Acts.Characters.Enemy;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BrokenObjectActor : EnemyActor
{
	[SerializeField]
	private BrokenObjectStatAct _brokenStat;
	protected override void Init()
	{
		base.Init();
		RemoveAct<EnemyStatAct>();
		RemoveAct<EnemyAnimation>();
		RemoveAct<CharacterEquipmentAct>();
		RemoveAct<CharacterRender>();
		AddAct(_brokenStat);
	}
}
