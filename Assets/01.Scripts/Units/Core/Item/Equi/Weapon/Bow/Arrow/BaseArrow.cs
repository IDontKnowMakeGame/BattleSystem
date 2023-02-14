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
	private string arrowType;

	public Arrow ThisArrow => _thisArrow;

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

	private Dictionary<string, Arrow> _arrows = new Dictionary<string, Arrow>();

	private ArrowStat _arrowStat;

	public bool isPull = false;

	private Vector3 goalPos;

	private EnemyBase _enemyBase;
	private PlayerBase _playerBase;
	private void Awake()
	{
		_arrows.Add("OldBowArrow", new OldArrow());
		foreach (var a in _arrows)
		{
			a.Value.Start();
			a.Value.thisObject = this.gameObject;
		}
	}
	private void OnTriggerEnter(Collider other)
	{
		var units = other.GetComponent<Units.Base.Units>();
		if (units as EnemyBase || units as PlayerBase)
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

	public void InitArrow(float speed, float damage, Vector3 pos, Vector3 dir, string name)
	{
		arrowType = name;
		_arrowStat = new ArrowStat(dir, pos, speed, damage);
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
				Debug.Log(_thisArrow);
				_thisArrow.Stick(GameManagement.Instance.GetManager<MapManager>().GetBlock(this.transform.position), _arrowStat.dir);
				AddKey();
				_thisArrow.isStick = true;
			}
		);
	}
	private void Pull()
	{
		Debug.Log(_thisArrow.isStick);
		if (isPull && _thisArrow.isStick &&  _playerBase.GetBehaviour<UnitEquiq>().CurrentWeapon is BaseBow)
		{
			Debug.Log(">");
			isPull = false;
			_thisArrow.PullOut(_playerBase);
			if (_enemyBase is EnemyBase)
			{
				_enemyBase.GetBehaviour<UnitStat>().Damaged(_arrowStat.damage / 2, InGame.PlayerBase);
				_enemyBase = null;
			}

			DelKey();
		}
	}
	private void StickOrPull(Units.Base.Units units)
	{
		if (units != null && !_thisArrow.isStick)
		{
			_enemyBase = units as EnemyBase;
			_enemyBase.GetBehaviour<UnitStat>()?.Damaged(_arrowStat.damage, InGame.PlayerBase);
			_thisArrow.Stick(units, _arrowStat.dir);
			AddKey();
			this.gameObject.transform.DOKill();
		}
		else if (units as PlayerBase && _thisArrow.isStick)
		{
			isPull = true;
			_playerBase = units as PlayerBase;
		}
	}

	private void AddKey()
	{
		Define.GetManager<InputManager>().AddInGameAction(InputTarget.SubSkillKey, InputStatus.Press, Pull);
	}
	private void DelKey()
	{
		Define.GetManager<InputManager>().RemoveInGameAction(InputTarget.SubSkillKey, InputStatus.Press, Pull);
	}
}
