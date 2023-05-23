using Blocks;
using Core;
using Managements.Managers;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using Tool.Map.Rooms;
using UnityEngine;
using UnityEngine.XR;

public class RoomSquall : MonoBehaviour
{
	[Header("비 내리는지 안내리는지 판단해주는 타이머")]
	[SerializeField]
	private float _timer = 0;
	[Header("비가 내리기 전 기다리는 시간")]
	[SerializeField]
	private float _waitTimer = 0;

	[Space]

	[Header("스콜이 나오는 초")]
	[SerializeField]
	private float _waitSquallCreateTime = 0;
	[Header("한번에 나오는 스콜 개수")]
	[SerializeField]
	private int _count = 0;

	[Space]

	[Header("스콜 나오는 프리팹")]
	[SerializeField]
	private string _objName;

	private float _currentTimer = 0;
	private float _waitCurrentTimer = 0;
	private float _waitSquallCreateTimer = 0;

	private bool isSquall = false;

	private List<int> _randomList = new List<int>();

	public bool onNaturalSquall = false;

	public bool awakeNaturalSquall = true;

	private void Start()
	{
		SquallInit(_objName, awakeNaturalSquall);
	}

	void Update()
	{
		if (!onNaturalSquall)
		{
			if (isSquall)
				SquallCreate();
		}
		else
			NaturalSquall();
	}

	public void SquallInit(string name, bool isNatural = false)
	{
		_objName = name;
		onNaturalSquall = isNatural;
		isSquall = isNatural;
	}

	public void SquallOnOff(bool squall) => isSquall = squall;

	private void NaturalSquall()
	{
		if (_timer > _currentTimer)
		{
			_currentTimer += Time.deltaTime;
		}
		else
		{
			_currentTimer = 0;
			_waitCurrentTimer = 0;
			int sq = Random.Range(0, 2);
			isSquall = sq == 0 ? true : false;
		}


		Squall();
	}
	private void Squall()
	{
		if (isSquall)
		{
			if (_waitTimer > _waitCurrentTimer)
			{
				_waitCurrentTimer += Time.deltaTime;
			}
			else
			{
				SquallCreate();
			}
		}
	}
	private void SquallCreate()
	{
		if (_waitSquallCreateTime > _waitSquallCreateTimer)
		{
			_waitSquallCreateTimer += Time.deltaTime;
		}
		else
		{
			_waitSquallCreateTimer = 0;
			Transform room = InGame.GetBlock(InGame.Player.Position).gameObject.transform.parent;
			Block[] blocks = room.GetComponentsInChildren<Block>();
			int count = blocks.Length;
			RandomListInit(count);
			RandomRoom(count);
			Debug.Log(count);
			Debug.Log(_randomList);
			for (int i = 0; i < _count; i++)
			{
				GameObject obj = Define.GetManager<ResourceManager>().Instantiate(_objName);
				obj.transform.position = blocks[_randomList[i]].transform.position + Vector3.up * 6;
			}
		}
	}
	private void RandomListInit(int count)
	{
		_randomList.Clear();

		for (int i = 0; i < count; i++)
		{
			_randomList.Add(i);
		}
	}
	private void RandomRoom(int count)
	{
		for (int j = 0; j < 100; j++)
		{
			int range1 = Random.Range(0, count);
			int range2 = Random.Range(0, count);

			int temp = _randomList[range1];
			_randomList[range1] = _randomList[range2];
			_randomList[range2] = temp;
		}
	}
}
