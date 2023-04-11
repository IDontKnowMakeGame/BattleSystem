using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DragonRealm : MonoBehaviour
{
	private float _duration = 0;
	private float _decrease = 0;
	private bool _isCoolTime = false;
	private float _currentTimer = 0;

	public void Init(float duration, float decrease)
	{
		_duration = duration;
		_decrease = decrease;
		_isCoolTime = true;
	}

	private void Update()
	{
		if (!_isCoolTime)
			return;

		if(_currentTimer < _duration)
		{
			_currentTimer += Time.deltaTime;
		}
		else
		{
			_currentTimer = 0;
			_isCoolTime = false;
		}
	}

	private void OnTriggerEnter(Collider other)
	{
		if(other.CompareTag("Player"))
		{
			other.GetComponent<CharacterStatAct>().Half += _decrease;
		}
	}

	private void OnTriggerExit(Collider other)
	{
		if (other.CompareTag("Player"))
		{
			other.GetComponent<CharacterStatAct>().Half -= _decrease;
		}
	}
}
