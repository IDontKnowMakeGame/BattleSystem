using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using Units.Base.Player;
using Units.Behaviours.Unit;
using Core;
using Managements.Managers;
using System;
public enum ArrowType
{
	OldArrow
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

	private Vector3 _dir;
	private Vector3 _pos;
	private float _speed = 10;
	private float _damage;

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

	private void Update()
	{
		Pull();
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

	public void InitArrow(float speed, float damage, Vector3 pos, Vector3 dir, string name)
	{
		_speed = speed;
		_damage = damage;
		_pos = pos;
		_dir = dir;
		arrowType = name;
	}
	private void Shoot()
	{
		this.transform.position = _pos + Vector3.up;

		int count = 6;
		goalPos = this.transform.position + (_dir * 5);

		var map = Define.GetManager<MapManager>();

		if (map.GetBlock(goalPos) == null || !map.GetBlock(goalPos).isWalkable)
		{
			while (map.GetBlock(goalPos) == null)
			{
				count--;
				goalPos -= _dir;
			}
		}
		float time = count / _speed;
		this.transform.DOMove(goalPos, time).OnComplete(
			() =>
			{
				_thisArrow.isStick = true;
			}
		);
	}

	private void Pull()
	{
		if (UnityEngine.Input.GetKeyDown(KeyCode.V) && isPull && _thisArrow.isStick)
		{
			isPull = false;
			_thisArrow.PullOut(_playerBase);
		}
	}

	private void StickOrPull(Units.Base.Units units)
	{
		if (units as EnemyBase && !_thisArrow.isStick)
		{
			_enemyBase = units as EnemyBase;
			_enemyBase.GetBehaviour<UnitStat>().Damaged(_damage, InGame.PlayerBase);
			_thisArrow.Stick(_enemyBase);
			this.gameObject.transform.DOKill();
		}
		else if (units as PlayerBase && _thisArrow.isStick)
		{
			isPull = true;
			_playerBase = units as PlayerBase;
		}
	}
}
