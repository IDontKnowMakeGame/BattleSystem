using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using Units.Base.Player;
using Units.Behaviours.Unit;

public enum ArrowType
{
	OldArrow,
}
public class BaseArrow : MonoBehaviour
{
	[SerializeField]
	private ArrowType _arrowType;

	public Arrow ThisArrow => _thisArrow;

	private Arrow _thisArrow 
	{ 
		get
		{
			return _arrows[_arrowType];
		}
		set
		{
			_arrows[_arrowType] = value;
		}
	}

	private Dictionary<ArrowType, Arrow> _arrows;

	public Vector3 dir;
	public Vector3 pos;
	public float speed;
	public float damage;

	private Vector3 goalPos;

	private EnemyBase _enemyBase;
	private void Start()
	{
		goalPos = this.transform.position + (dir * 5);
		this.gameObject.transform.DOMove(goalPos, speed);

		_arrows.Add(ArrowType.OldArrow, new OldArrow());

		foreach (var a in _arrows)
		{
			a.Value.Start();
		}
	}

	private void Update()
	{

	}

	private void OnTriggerEnter(Collider other)
	{
		var units = other.GetComponent<Units.Base.Units>();
		if(units as EnemyBase && !_thisArrow.isStick)
		{
			_enemyBase = units as EnemyBase;
			_enemyBase.GetBehaviour<UnitStat>().Damaged(damage);
			this.gameObject.transform.DOKill();
			_thisArrow.Stick(_enemyBase);
		}
		else if(units as PlayerBase && _thisArrow.isStick)
		{
			_thisArrow.PullOut(units as PlayerBase);
		}
	}
}
