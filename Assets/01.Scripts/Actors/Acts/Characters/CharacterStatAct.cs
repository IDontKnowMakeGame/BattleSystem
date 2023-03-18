using Actors.Bases;
using Actors.Characters;
using Acts.Base;
using Data;
using System;
using UnityEngine;

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

	private CharacterStat _changeStat =new CharacterStat();
	private CharacterActor _actor;
	public override void Start()
	{
		_actor = ThisActor as CharacterActor;
		_changeStat.CopyStat(_basicStat);
		_changeStat.ChangeStat(_actor.currentWeapon.info);
	}

	public void Damage(float damage, Actor actor)
	{

	}

	public void Die()
	{

	}
}
