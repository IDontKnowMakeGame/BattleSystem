using Core;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class PlayerStatAct : CharacterStatAct
{
	public override void Start()
	{
		base.Start();
		UIManager.Instance.InGame.ChanageMaxHP((int)_changeStat.hp / 10);
		Define.GetManager<EventManager>().StartListening(EventFlag.ChangeStat, StatChange);
	}

	public void StatChange(EventParam eventParam)
	{
		Debug.Log("?");
		base.StatChange();
		UIManager.Instance.InGame.ChanageMaxHP((int)_changeStat.hp / 10);
	}
}
