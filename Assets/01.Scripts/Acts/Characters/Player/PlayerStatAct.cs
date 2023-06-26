using Core;
using System.Collections;
using System.Collections.Generic;
using Actors.Bases;
using Actors.Characters;
using Actors.Characters.Player;
using Acts.Characters.Player;
using Blocks;
using ETC;
using UnityEngine;
using Data;
using Acts.Characters;
using DG.Tweening;
using Managements;

[System.Serializable]
public class PlayerStatAct : CharacterStatAct
{
	private EventParam statParam;

	[SerializeField]
	private BloodController bloodController;

	private PlayerAnimation _playerAnimation;

    public override void Start()
	{
		base.Start();

		// HP 포션 5개로 초기화
		SaveItemData currentData = Define.GetManager<DataManager>().LoadItemFromInventory(Data.ItemID.HPPotion);
		currentData.currentCnt = 5;
		UIManager.Instance.InGame.SetItemPanelCnt(Data.ItemID.HPPotion);
		Define.GetManager<DataManager>().ChangeItemInfo(currentData);

		_playerAnimation = ThisActor.GetAct<PlayerAnimation>();
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
		if (ChangeStat.hp <= 0) return;

		base.Damage(damage, actor);
		
		if (actor is EmptyBlock)
		{
			if (ChangeStat.hp <= 0)
				return;
			Fall();
			return;
		}
		
		if (ThisActor is PlayerActor)
		{
			statParam.unit = actor;
			Define.GetManager<EventManager>().TriggerEvent(EventFlag.PollutionHalo, statParam);

			bloodController.StartBlood();
			ThisActor.GetAct<PlayerBuff>().ChangeAnger(1);

			EventParam param = new EventParam();
			param.boolParam = false;
			Define.GetManager<EventManager>().TriggerEvent(EventFlag.HaloOfEreshkigal, param);

			if (ThisActor.GetAct<PlayerUseAbleItem>().HPPotion.UsePortion)
				ThisActor.GetAct<PlayerUseAbleItem>().HPPotion.ResetPotion();

			UIManager.Instance.InGame.ChangeCurrentHP(PercentHP());
			EventParam eventParam = new EventParam();
			eventParam.stringParam = "Damaged";
            Define.GetManager<EventManager>().TriggerEvent(EventFlag.PlayTimeLine, eventParam);
		}
	}

	protected override void Fall()
	{
		GameManagement.Instance.RemoveInputManagers(); 

		_playerAnimation.ChangeWeaponClips((int)ItemID.None);
		_playerAnimation.Play("Fall");

		ThisActor.transform.GetChild(0).DOMoveY(-1f, 0.5f);

        UIManager.Instance.DeathPanel.Show();

        var dieClip = _playerAnimation.GetClip("Fall");
		dieClip.SetEventOnFrame(dieClip.fps - 2, () =>
		LoadingSceneController.Instnace.StartCoroutine(LoadingSceneController.Instnace.LoadScene("Lobby", 4f)));
		dieClip.SetEventOnFrame(dieClip.fps - 1, base.Die);

		EventParam param = new EventParam();
		param.boolParam = false;
		param.stringParam = "Die";
		Define.GetManager<EventManager>().TriggerEvent(EventFlag.HaloOfEreshkigal, param);
		// HP 포션 5개로 초기화
		SaveItemData currentData = Define.GetManager<DataManager>().LoadItemFromInventory(Data.ItemID.HPPotion);
		currentData.currentCnt = 5;
		UIManager.Instance.InGame.SetItemPanelCnt(Data.ItemID.HPPotion);
		Define.GetManager<DataManager>().ChangeItemInfo(currentData);
	}

	public override void Die()
	{
		GameManagement.Instance.RemoveInputManagers(); 

		_actor.AddState(CharacterState.Die);
		_playerAnimation.ChangeWeaponClips((int)ItemID.None);
		_playerAnimation.Play("Die");

		var dieClip = _playerAnimation.GetClip("Die");
		dieClip.SetEventOnFrame(dieClip.fps - 2, () =>
		PlayerDeath.Instance.FocusCenter());
		dieClip.SetEventOnFrame(dieClip.fps - 1, base.Die);


		EventParam param = new EventParam();
		param.boolParam = false;
		param.stringParam = "Die";
		Define.GetManager<EventManager>().TriggerEvent(EventFlag.HaloOfEreshkigal, param);
		Debug.Log(ThisActor);
	}
}
