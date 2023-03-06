using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using Units.Base.Player;
using Units.Behaviours.Unit;
using Core;
using Managements.Managers;
using Managements;
using System;
using Units.Base.Unit;

public enum ArrowType
{
	OldArrow
}

public class ArrowStat
{
	public Vector3 dir;
	public Vector3 pos;
	public float speed = 10;
	public float damage;

	public ArrowStat(Vector3 direction, Vector3 position, float speeds, float damages)
	{
		dir = direction;
		pos = position;
		speed = speeds;
		damage = damages;
	}
}
public class BaseArrow : MonoBehaviour
{
	public Arrow ThisArrow => _thisArrow;
	public bool isPull = false;

	private ArrowType arrowType;
	private Arrow _thisArrow
	{
		get
		{
			return _arrows[arrowType];
		}
		set
		{
			_arrows[arrowType] = value;
		}
	}

	private Dictionary<ArrowType, Arrow> _arrows = new Dictionary<ArrowType, Arrow>();
	private ArrowStat _arrowStat;
	private Vector3 goalPos;

	private Units.Base.Units _unit;
	private UnitBase _thisUnit;
	private void Awake()
	{
		_arrows.Add(ArrowType.OldArrow, new OldArrow());

		_thisArrow.Start();
		_thisArrow.thisObject = this.gameObject;
	}
	private void OnTriggerEnter(Collider other)
	{
		var units = other.GetComponent<Units.Base.Units>();
		StickOrPull(units);
	}

	private void OnTriggerExit(Collider other)
	{
		var units = other.GetComponent<Units.Base.Units>();
		if (units as PlayerBase && !isPull)
		{
			isPull = false;
		}
	}
	public void ShootArrow() => Shoot();

	public void InitArrow(float speed, float damage, Vector3 pos, Vector3 dir, ArrowType name, UnitBase unit)
	{
		arrowType = name;
		_arrowStat = new ArrowStat(dir, pos, speed, damage);
		_thisUnit = unit;
	}
	private void Shoot()
	{
		this.transform.position = _arrowStat.pos + Vector3.up;

		int count = 6;
		goalPos = this.transform.position + (_arrowStat.dir * 5);

		var map = Define.GetManager<MapManager>();

		if (map.GetBlock(goalPos) == null || !map.GetBlock(goalPos).isWalkable)
		{
			while (map.GetBlock(goalPos) == null)
			{
				count--;
				goalPos -= _arrowStat.dir;
			}
		}
		float time = count / _arrowStat.speed;
		this.transform.DOMove(goalPos, time).OnComplete(
			() =>
			{
				_thisArrow.Stick(GameManagement.Instance.GetManager<MapManager>().GetBlock(this.transform.position), _arrowStat.dir);
				AddKey();
				_thisArrow.isStick = true;
			}
		);
	}
	private void Pull()
	{
		if (isPull && _thisArrow.isStick &&  InGame.PlayerBase.GetBehaviour<UnitEquiq>().CurrentWeapon is BaseBow)
		{
			isPull = false;
			_thisArrow.PullOut(InGame.PlayerBase);

			if (_thisArrow.StickObject is UnitBase)
			{
				UnitBase unit = _thisArrow.StickObject as UnitBase;
				unit.GetBehaviour<UnitStat>().Damaged(_arrowStat.damage / 2, InGame.PlayerBase);
			}

			DelKey();
		}
	}
	private void StickOrPull(Units.Base.Units units)
	{
		if (!units)
			return;

		if (!_thisArrow.isStick && _thisUnit.GetHashCode() != units.GetHashCode())
		{
			UnitBase baseUnit = units as UnitBase;
			baseUnit.GetBehaviour<UnitStat>()?.Damaged(_arrowStat.damage, _thisUnit);
			_thisArrow.Stick(units, _arrowStat.dir);
			AddKey();
			this.gameObject.transform.DOKill();
		}
		else if (units as PlayerBase && _thisArrow.isStick)
		{
			isPull = true;
		}
	}

	private void AddKey()
	{
		InputManager.OnSubPress += Pull;
	}
	private void DelKey()
	{
		InputManager.OnSubPress -= Pull;
	}
}
