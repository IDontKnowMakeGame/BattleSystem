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

[Serializable]
public class CharacterStat
{
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
		this.speed = ItemInfo.WeightToSpeed(info.Weight);
		return this;
	}

	public void CopyStat(CharacterStat stat)
	{
		this.hp = stat.hp;
		this.atk = stat.atk;
		this.ats = stat.ats;
		this.afs = stat.afs;
		this.speed = stat.speed;
	}
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
				return _basicStat;

			_changeStat.ChangeStat(_actor.currentWeapon.info);
			return _changeStat;
		}
	}

	public float Half { get; set; }

	[SerializeField]
	private CharacterStat _changeStat =new CharacterStat();
	private CharacterActor _actor;
	public override void Start()
	{
		_actor = ThisActor as CharacterActor;
		_changeStat.CopyStat(_basicStat);
		if(_actor.currentWeapon != null)
			_changeStat.ChangeStat(_actor.currentWeapon.info);

		if (ThisActor is PlayerActor)
		{
            UIManager.Instance.InGame.ChanageMaxHP((int)_basicStat.hp / 10);
        }
			
	}

	public void Damage(float damage, Actor actor)
	{
		ChangeStat.hp -= damage - (damage * (Half/100));
		if(ChangeStat.hp <= 0)
		{
			if(actor is PlayerActor)
			{
				PlayerActor player = actor as PlayerActor;
				Define.GetManager<DataManager>().AddWeaponClassKillData(player.currentWeapon.info.Name);

				player.GetAct<PlayerEquipment>().CurrentWeapon.LoadWeaponClassLevel();
			}
			
			Die();
		}
        if (ThisActor is PlayerActor)
        {
            float value = (ChangeStat.hp / BaseStat.hp) * 100;
            UIManager.Instance.InGame.ChangeCurrentHP((int)value);
			Define.GetManager<EventManager>().TriggerEvent(EventFlag.PlayTimeLine, new EventParam());
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
}
