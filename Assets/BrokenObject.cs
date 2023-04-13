using Core;
using Managements;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BrokenObject : MonoBehaviour
{
	[SerializeField]
	private string _brokenObjectName;

	[SerializeField]
	private int _hp;

	private int _currentHp;

	private void Start()
	{
		_currentHp = _hp;
	}

	public void Damage(int damage)
	{
		_currentHp -= damage;
		if(_currentHp <= 0)
		{
			GameObject obj = GameManagement.Instance.GetManager<ResourceManager>().Instantiate(_brokenObjectName);
			obj.transform.position = this.transform.position;
			Destroy(gameObject);
		}
	}
}
