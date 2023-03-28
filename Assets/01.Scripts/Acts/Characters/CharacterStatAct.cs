using Actors.Bases;
using Actors.Characters;
using Actors.Characters.Player;
using Acts.Base;
using Core;
using Data;
using System;
using DG.Tweening;
using UnityEngine;
using UnityEngine.SceneManagement;
using Actors.Characters.Enemy;
using Acts.Characters.Player;

[Serializable]
public class CharacterStat
{
	public float maxHP;
	public float hp;
	public float atk;
	public float ats;
	public float afs;
	public float speed;

	public CharacterStat ChangeStat(ItemInfo info)
	{
		this.atk = info.Atk;
		this.ats = info.Ats;
		this.afs = info.Afs;
		if (info.Weight > 0)
			this.speed = ItemInfo.WeightToSpeed(info.Weight);
		else
			this.speed = ItemInfo.WeightToSpeed(1);
		return this;
	}

	public void CopyStat(CharacterStat stat)
	{
		this.maxHP = stat.maxHP;
		this.hp = stat.hp;
		this.atk = stat.atk;
		this.ats = stat.ats;
		this.afs = stat.afs;
		this.speed = stat.speed;
	}
}

public enum StatType
{
	MAXHP,
	ATK,
	ATS,
	AFS,
	SPEED
}

[Serializable]
public class CharacterStatAct : Act, IDmageAble
{
	[SerializeField]
	private CharacterStat _basicStat;

	public CharacterStat BaseStat => _basicStat;

	public CharacterStat ChangeStat
	{
		get
		{
			if (_actor.currentWeapon == null)
			{
				_changeStat.speed = _basicStat.speed;
				return _changeStat;
			}

			return _changeStat;
		}
	}

	public float Half { get; set; }

	[SerializeField]
	protected CharacterStat _changeStat = new CharacterStat();
	private CharacterActor _actor;
	public override void Start()
	{
		_actor = ThisActor as CharacterActor;
		_changeStat.CopyStat(_basicStat);

		if (_actor.currentWeapon != null)
			StatChange();

		_changeStat.hp = _changeStat.maxHP;
	}
	public virtual void StatChange()
	{
		CharacterEquipmentAct equipement = _actor.GetAct<CharacterEquipmentAct>();

		ItemInfo info = equipement.EquipemntStat;
		_changeStat.ChangeStat(info);
		_changeStat.maxHP = _basicStat.maxHP + info.Hp;
	}
	public virtual void Heal(int hp)
	{
		ChangeStat.hp += hp;

		if (ChangeStat.hp >= ChangeStat.maxHP)
			ChangeStat.hp = ChangeStat.maxHP;


	}
	public void Damage(float damage, Actor actor)
	{
		ChangeStat.hp -= damage - (damage * (Half / 100));
		if (ChangeStat.hp <= 0)
		{
			if (actor is PlayerActor)
			{
				PlayerActor player = actor as PlayerActor;
				Define.GetManager<DataManager>().AddWeaponClassKillData(player.currentWeapon.info.Class);

				player.GetAct<PlayerEquipment>().CurrentWeapon.LoadWeaponClassLevel();
			}

			Die();
		}
		if (ThisActor is PlayerActor)
		{
			float value = (ChangeStat.hp / BaseStat.hp) * 100;

			ThisActor.GetAct<PlayerBuff>().ChangeAnger(1);

			UIManager.Instance.InGame.ChangeCurrentHP((int)value);
			EventParam eventParam = new EventParam();
			eventParam.intParam = 0;
			Define.GetManager<EventManager>().TriggerEvent(EventFlag.PlayTimeLine, eventParam);
		}
		if (ThisActor is EnemyActor)
		{
			float value = (ChangeStat.hp / BaseStat.hp) * 100;
			UIManager.Instance.BossBar.ChangeBossBarValue((int)value);
			EventParam eventParam = new EventParam();
			eventParam.intParam = 1;
			Define.GetManager<EventManager>().TriggerEvent(EventFlag.PlayTimeLine, eventParam);
		}
	}
	public void Die()
	{
		if (ThisActor is PlayerActor)
		{
			DOTween.KillAll();
			SceneManager.LoadScene("Lobby");
		}
	}

	#region FAO
	public void Dev(StatType type, float times)
	{
		switch (type)
		{
			case StatType.MAXHP:
				ChangeStat.maxHP /= times;
				break;
			case StatType.ATK:
				ChangeStat.atk /= times;
				break;
			case StatType.ATS:
				ChangeStat.ats /= times;
				break;
			case StatType.AFS:
				ChangeStat.afs /= times;
				break;
			case StatType.SPEED:
				ChangeStat.speed /= times;
				break;
		}
	}

	public void Multi(StatType type, float times)
	{
		switch (type)
		{
			case StatType.MAXHP:
				ChangeStat.maxHP *= times;
				break;
			case StatType.ATK:
				ChangeStat.atk *= times;
				break;
			case StatType.ATS:
				ChangeStat.ats *= times;
				break;
			case StatType.AFS:
				ChangeStat.afs *= times;
				break;
			case StatType.SPEED:
				ChangeStat.speed *= times;
				break;
		}
	}

	public void Plus(StatType type, float add)
	{
		switch (type)
		{
			case StatType.MAXHP:
				ChangeStat.maxHP += add;
				break;
			case StatType.ATK:
				ChangeStat.atk += add;
				break;
			case StatType.ATS:
				ChangeStat.ats += add;
				break;
			case StatType.AFS:
				ChangeStat.afs += add;
				break;
			case StatType.SPEED:
				int weight = ItemInfo.SpeedToWeight(ChangeStat.speed);
				if (weight - add < 9)
					ChangeStat.speed = ItemInfo.WeightToSpeed(weight - (int)add);
				else
					ChangeStat.speed = ItemInfo.WeightToSpeed(1);
				break;
		}
	}

	public void Sub(StatType type, float min)
	{
		switch (type)
		{
			case StatType.MAXHP:
				ChangeStat.maxHP -= min;
				break;
			case StatType.ATK:
				ChangeStat.atk -= min;
				break;
			case StatType.ATS:
				ChangeStat.ats -= min;
				break;
			case StatType.AFS:
				ChangeStat.afs -= min;
				break;
			case StatType.SPEED:
				int weight = ItemInfo.SpeedToWeight(ChangeStat.speed);
				if (weight - min > 0)
					ChangeStat.speed = ItemInfo.WeightToSpeed(weight - (int)min);
				else
					ChangeStat.speed = ItemInfo.WeightToSpeed(1);
				break;
		}
	}
	#endregion
}
