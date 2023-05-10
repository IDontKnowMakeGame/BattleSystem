using Core;
using System.Collections;
using System.Collections.Generic;
using Actors.Bases;
using Actors.Characters.Player;
using Acts.Characters.Player;
using Blocks;
using ETC;
using UnityEngine;
using Data;
using Acts.Characters;
using DG.Tweening;

[System.Serializable]
public class PlayerStatAct : CharacterStatAct
{
	private EventParam statParam;

	[SerializeField]
	private BloodController bloodController;

	private PlayerAnimation _playerAnimation;

    public override void Awake()
    {
        base.Awake();
		_playerAnimation = ThisActor.GetAct<PlayerAnimation>();
	}

    public override void Start()
	{
		base.Start();
		UIManager.Instance.InGame.ChanageMaxHP((int)ChangeStat.maxHP / 10);
		UIManager.Instance.InGame.ChangeCurrentHP(PercentHP());
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
		
		if (actor is EmptyBlock)
		{
			Die();
			return;
		}
		
		if (ThisActor is PlayerActor)
		{
			statParam.unit = actor;
			Define.GetManager<EventManager>().TriggerEvent(EventFlag.PollutionHalo, statParam);

			bloodController.StartBlood();
			ThisActor.GetAct<PlayerBuff>().ChangeAnger(1);

			if (ThisActor.GetAct<PlayerUseAbleItem>().HPPotion.UsePortion)
				ThisActor.GetAct<PlayerUseAbleItem>().HPPotion.ResetPotion();

			UIManager.Instance.InGame.ChangeCurrentHP(PercentHP());
			EventParam eventParam = new EventParam();
			eventParam.stringParam = "Damaged";
            Define.GetManager<EventManager>().TriggerEvent(EventFlag.PlayTimeLine, eventParam);
		}
	}

	public override void Die()
	{
		ThisActor.RemoveAct<CharacterMove>();
		_playerAnimation.ChangeWeaponClips((int)ItemID.None);
		_playerAnimation.Play("Die");
		var dieClip = _playerAnimation.GetClip("Die");
		dieClip.OnExit += base.Die;
		dieClip.OnExit += PlayerDeath.Instance.FocusCenter;

		// HP 포션 5개로 초기화
		SaveItemData currentData = Define.GetManager<DataManager>().LoadItemFromInventory(Data.ItemID.HPPotion);
		currentData.currentCnt = 5;
		UIManager.Instance.InGame.SetItemPanelCnt(Data.ItemID.HPPotion);
		Define.GetManager<DataManager>().ChangeItemInfo(currentData);
		ThisActor.RemoveAct<CharacterStatAct>();
	}
}
