using Core;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EffectDestory : MonoBehaviour
{
	[SerializeField]
	private GameObject obj;

	[SerializeField]
	private bool _isTimeDestroy = false;

	[SerializeField]
	private float _time = 0;

	private void OnEnable()
	{
		if (_isTimeDestroy)
			StartCoroutine(Destroy(_time));
	}

	public void OnTriggerEnter(Collider other)
	{
		Define.GetManager<ResourceManager>().Destroy(obj);
	}

	private IEnumerator Destroy(float timer)
	{
		yield return new WaitForSeconds(timer);
		Define.GetManager<ResourceManager>().Destroy(obj);
	}
}
