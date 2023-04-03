using Core;
using System.Collections;
using System.Collections.Generic;
using Actors.Bases;
using Actors.Characters.Player;
using Acts.Characters.Player;
using UnityEngine;

[System.Serializable]
public class PlayerStatAct : CharacterStatAct
{
	public override void Start()
	{
		base.Start();
		UIManager.Instance.InGame.ChanageMaxHP((int)_changeStat.maxHP / 10);
		Define.GetManager<EventManager>().StartListening(EventFlag.ChangeStat, StatChange);
	}

	public void StatChange(EventParam eventParam)
	{
		base.StatChange();
		UIManager.Instance.InGame.ChanageMaxHP((int)_changeStat.maxHP / 10);
	}

	public override void Damage(float damage, Actor actor)
	{
		base.Damage(damage, actor);
		
		if (ThisActor is PlayerActor)
		{
			ThisActor.GetAct<PlayerBuff>().ChangeAnger(1);

			if (ThisActor.GetAct<PlayerUseAbleItem>().HPPotion.UsePortion)
				ThisActor.GetAct<PlayerUseAbleItem>().HPPotion.ResetPotion();

			UIManager.Instance.InGame.ChangeCurrentHP(PercentHP());
			EventParam eventParam = new EventParam();
			eventParam.stringParam = "Damaged";
            Define.GetManager<EventManager>().TriggerEvent(EventFlag.PlayTimeLine, eventParam);
		}
	}
}
