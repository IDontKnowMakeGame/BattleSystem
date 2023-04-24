using Actors.Bases;
using Actors.Characters;
using Actors.Characters.Player;
using Acts.Base;
using Core;
using Data;
using System;
using UnityEngine;
using Acts.Characters;
using static UnityEngine.CullingGroup;
using System.Collections.Generic;
using Unity.VisualScripting;

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
public class CharacterStatAct : Act
{
	[SerializeField]
	private CharacterStat _basicStat;

	public CharacterStat BaseStat => _basicStat;

	[SerializeField]
	protected CharacterStat _changeStat = new CharacterStat();

	public CharacterStat ChangeStat
	{
		get
		{
			if (_actor.currentWeapon == null)
			{
				_changeStat.speed = _basicStat.speed;
				return _changeStat;
			}

			StatChange();

			if (_drainageAtk.Count > 0 || _percentStat > 0)
				_changeStat.atk = (_changeStat.atk * _drainageStat) + ((_changeStat.atk * _drainageStat) * (_percentStat / 100));
			else
				_changeStat.atk = (_changeStat.atk * 1) + ((_changeStat.atk * 1) * (_percentStat / 100));

			return _changeStat;
		}
	}

	private float _drainageStat
	{
		get
		{
			float value = 1;
			foreach (var a in _drainageAtk)
			{
				value *= a.Value;
			}

			return value;
		}
	}
	private float _percentStat = 0;

	public float Half { get; set; }

	private CharacterActor _actor;
	private CharacterRender _render;
	private CharacterEquipmentAct _eqipment;

	private Dictionary<string, float> _drainageAtk = new Dictionary<string, float>();

	public override void Start()
	{
		_eqipment = ThisActor.GetAct<CharacterEquipmentAct>();
		_render = ThisActor.GetAct<CharacterRender>();
		_actor = ThisActor as CharacterActor;

		_changeStat.CopyStat(_basicStat);

		if (_actor.currentWeapon != null)
			StatChange();

		_changeStat.hp = _changeStat.maxHP;
	}

	public virtual void AddDrainageAtk(string saveName, float plusValue)
	{
		float repository = 0f;
		if (_drainageAtk.TryGetValue(saveName, out repository))
		{
			repository *= plusValue;
			_drainageAtk.Add(saveName, repository);
		}
		else
		{
			_drainageAtk.Add(saveName, plusValue);
		}
	}

	public virtual void DelDrainageAtk(string saveName)
	{
		float repository = 0f;
		if (_drainageAtk.TryGetValue(saveName, out repository))
			_drainageAtk.Remove(saveName);
	}
	public virtual void PercentAtk(float percentValue) => _percentStat += percentValue;

	public virtual void StatChange()
	{
		ItemInfo info = _eqipment.CurrentWeapon.WeaponInfo;
		_changeStat.ChangeStat(info);
		_changeStat.maxHP = _changeStat.maxHP + info.Hp;
	}
	public virtual void Heal(int hp)
	{
		ChangeStat.hp += hp;

		if (ChangeStat.hp >= ChangeStat.maxHP)
			ChangeStat.hp = ChangeStat.maxHP;


	}
	public virtual void Damage(float damage, Actor actor)
	{
		ChangeStat.hp -= damage - (damage * (Half / 100));
		_render.Blink();

		Debug.Log(ThisActor.name);
		GameObject blood = Define.GetManager<ResourceManager>().Instantiate("Blood");
		blood.transform.position = ThisActor.transform.position;
		blood.GetComponent<ParticleSystem>().Play();

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
	}

	public int PercentHP()
	{
		return (int)((ChangeStat.hp / ChangeStat.maxHP) * 100);
	}

	public virtual void Die()
	{
		var particle = Define.GetManager<ResourceManager>().Instantiate("DeathParticle", ThisActor.transform);
		particle.transform.localPosition = Vector3.zero;
		var anchorTrm = ThisActor.transform.Find("Anchor");
		var modelTrm = anchorTrm.Find("Model");
		var scale = modelTrm.localScale;
		var rotation = anchorTrm.transform.rotation;
		var particleAnchorTrm = particle.transform.Find("Anchor");
		var particleModelTrm = particleAnchorTrm.Find("Model");
		particleAnchorTrm.rotation = rotation;
		particleModelTrm.localScale = scale;
		particle.transform.SetParent(null);

		ThisActor.gameObject.SetActive(false);
	}

	#region FAO
	public void Dev(StatType type, float times)
	{
		switch (type)
		{
			case StatType.MAXHP:
				_changeStat.maxHP /= times;
				break;
			case StatType.ATK:
				_changeStat.atk /= times;
				break;
			case StatType.ATS:
				_changeStat.ats /= times;
				break;
			case StatType.AFS:
				_changeStat.afs /= times;
				break;
			case StatType.SPEED:
				_changeStat.speed /= times;
				break;
		}
	}

	public void Multi(StatType type, float times)
	{
		switch (type)
		{
			case StatType.MAXHP:
				_changeStat.maxHP *= times;
				break;
			case StatType.ATK:
				_changeStat.atk *= times;
				break;
			case StatType.ATS:
				_changeStat.ats *= times;
				break;
			case StatType.AFS:
				_changeStat.afs *= times;
				break;
			case StatType.SPEED:
				_changeStat.speed *= times;
				break;
		}
	}

	public void Plus(StatType type, float add)
	{
		switch (type)
		{
			case StatType.MAXHP:
				_changeStat.maxHP += add;
				break;
			case StatType.ATK:
				_changeStat.atk += add;
				break;
			case StatType.ATS:
				_changeStat.ats += add;
				break;
			case StatType.AFS:
				_changeStat.afs += add;
				break;
			case StatType.SPEED:
				int weight = ItemInfo.SpeedToWeight(ChangeStat.speed);
				if (weight - add < 9)
					_changeStat.speed = ItemInfo.WeightToSpeed(weight - (int)add);
				else
					_changeStat.speed = ItemInfo.WeightToSpeed(1);
				break;
		}
	}

	public void Sub(StatType type, float min)
	{
		switch (type)
		{
			case StatType.MAXHP:
				_changeStat.maxHP -= min;
				break;
			case StatType.ATK:
				_changeStat.atk -= min;
				break;
			case StatType.ATS:
				_changeStat.ats -= min;
				break;
			case StatType.AFS:
				_changeStat.afs -= min;
				break;
			case StatType.SPEED:
				int weight = ItemInfo.SpeedToWeight(ChangeStat.speed);
				if (weight - min > 0)
					_changeStat.speed = ItemInfo.WeightToSpeed(weight - (int)min);
				else
					_changeStat.speed = ItemInfo.WeightToSpeed(1);
				break;
		}
	}
	#endregion
}
