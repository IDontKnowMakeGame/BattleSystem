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
    [SerializeField]
    private int _count = 0;
    [SerializeField]
    private float _timer = 0;
    [SerializeField]
    private float _waitTimer = 0;

    private float _currentTimer = 0;
    private float _waitCurrentTimer = 0;

    private bool _isSquall = false;

    private List<int> _randomList = new List<int>();

	private void Start()
	{

	}

	void Update()
    {
        if (_timer > _currentTimer)
        {
            _currentTimer += Time.deltaTime;
        }
        else
        {
            _currentTimer = 0;
			_isSquall = Random.Range(0, 1) == 0 ? true : false;
		}


        if(_isSquall) 
        {
            if(_waitTimer > _waitCurrentTimer)
            {
                _waitCurrentTimer += Time.deltaTime;
			}
            else
            {
                GameObject room = InGame.GetBlock(InGame.Player.Position).Parent.gameObject;
                GameObject[] blocks = room.GetComponentsInChildren<GameObject>();
				int count = blocks.Length;
                RandomListInit(count);
                RandomRoom(count);
				for (int i = 0; i< _count; i++)
                {
					//= blocks[_randomList[i]].transform.position + Vector3.up * 6;
				}
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
