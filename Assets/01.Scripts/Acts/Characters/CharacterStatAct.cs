using Actors.Bases;
using Actors.Characters;
using Actors.Characters.Player;
using Acts.Base;
using Core;
using Data;
using System;
using UnityEngine;
using Acts.Characters;
using System.Collections.Generic;
using Acts.Characters.Player;
using Actors.Characters.Enemy;
using Blocks;

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
	Weight,
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
			_changeStat.atk += _changeStats[StatType.ATK];
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

	public float Half { get { return half; } set { half = value; } }

	private float half = 0;

	protected CharacterActor _actor;
	private CharacterRender _render;
	private CharacterEquipmentAct _eqipment;

	private Dictionary<string, float> _drainageAtk = new Dictionary<string, float>();
	private Dictionary<StatType, float> _changeStats = new Dictionary<StatType, float>();

	public override void Awake()
	{
		base.Awake();
		foreach(StatType stat in Enum.GetValues(typeof(StatType)))
		{
			_changeStats.Add(stat, 0);
		}
	}

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
		if (_eqipment == null) return;
		ItemInfo info = _eqipment.CurrentWeapon.WeaponInfo;
		_changeStat.ChangeStat(info);
		_changeStat.maxHP = _basicStat.maxHP + _changeStats[StatType.MAXHP];
		_changeStat.ats = _changeStat.ats + _changeStats[StatType.ATS];
		_changeStat.afs = _changeStat.ats + _changeStats[StatType.AFS];
		_changeStat.speed = ItemInfo.WeightToSpeed((int)(info.Weight + _changeStats[StatType.Weight])) + _changeStats[StatType.SPEED];
	}
	public virtual void Heal(int hp)
	{
		ChangeStat.hp += hp;

		if (ChangeStat.hp >= ChangeStat.maxHP)
			ChangeStat.hp = ChangeStat.maxHP;


	}
	public virtual void Damage(float damage, Actor actor)
	{

		if (actor is EmptyBlock && ChangeStat.hp > 0)
		{
			ChangeStat.hp = 0;
			Fall();
			return;
		}
		
		ChangeStat.hp -= damage - (damage * (Half / 100));
		
		if (ChangeStat.hp <= 0)
		{
			if (actor is PlayerActor)
			{
				PlayerActor player = actor as PlayerActor;
				Debug.Log("적 죽임");
				EventParam param = new EventParam();
				param.boolParam = true;
				Define.GetManager<EventManager>().TriggerEvent(EventFlag.HaloOfEreshkigal, param);
				Define.GetManager<DataManager>().AddWeaponClassKillData(player.currentWeapon.info.Class);
				player.GetAct<PlayerEquipment>().CurrentWeapon.LoadWeaponClassLevel();
			}
			Die();
		}
		
		_render.Blink();
		GameObject blood = Define.GetManager<ResourceManager>().Instantiate("Blood");
		blood.transform.position = ThisActor.transform.position;
		blood.GetComponent<ParticleSystem>().Play(); 
	}

	public int PercentHP()
	{
		return (int)((ChangeStat.hp / ChangeStat.maxHP) * 100);
	}

	protected virtual void Fall()
	{
		var anime = ThisActor.GetAct<PlayerAnimation>();
			anime.ChangeWeaponClips((int)ItemID.None);
		var clip = anime.GetClip("Fall");
		_actor.AddState(CharacterState.Die);
		clip.OnExit = Die;
		anime.Play("Fall");
	}

	public virtual void Die()
	{
		ThisActor.RemoveAct<CharacterMove>();
		_actor.AddState(CharacterState.Die);
		var particle = Define.GetManager<ResourceManager>().Instantiate("DeathParticle", ThisActor.transform);
		particle.transform.position = ThisActor.transform.position;
		var anchorTrm = ThisActor.transform.Find("Anchor");
		var modelTrm = anchorTrm.Find("Model");
		var scale = modelTrm.localScale;
		var rotation = anchorTrm.transform.rotation;
		var particleAnchorTrm = particle.transform.Find("Anchor");
		var particleModelTrm = particleAnchorTrm.Find("Model");
		particleAnchorTrm.rotation = rotation;
		particleModelTrm.localScale = scale;
        particle.transform.SetParent(null);
		InGame.GetBlock(ThisActor.Position).RemoveActorOnBlock();
		ThisActor.gameObject.SetActive(false);
	}

	#region FAO
	public void Dev(StatType type, float times) => _changeStats[type] /= times;

	public void Multi(StatType type, float times)=>_changeStats[type] *= times;

	public void Plus(StatType type, float add)
	{
		if (StatType.Weight == type)
		{
			ItemInfo info = _eqipment.CurrentWeapon.WeaponInfo;
			if (info.Weight + add + _changeStats[type] <= 9)
				_changeStats[type] = _changeStats[type] + add;
			return;
		}
		else if (StatType.SPEED == type)
			if (ChangeStat.speed + add > ItemInfo.WeightToSpeed(9))
				return;

		_changeStats[type] += add;
	}

	public void Sub(StatType type, float min)
	{
		if (StatType.Weight == type)
		{
			int weight = _eqipment.CurrentWeapon.WeaponInfo.Weight;
			if (weight + (_changeStats[type]-min) > 0)
				_changeStats[type] = _changeStats[type] - min;

			return;
		}
		else if (StatType.SPEED == type)
		{
			if (ChangeStat.speed - min < ItemInfo.WeightToSpeed(1))
				return;
		}
		_changeStats[type] -= min;
	}
	#endregion
}
