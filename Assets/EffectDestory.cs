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

	[SerializeField]
	private bool _shake = false;

	private void OnEnable()
	{
		if (_isTimeDestroy)
			StartCoroutine(Destroy(_time));
	}

	public void OnTriggerEnter(Collider other)
	{
		if (_shake)
			Define.GetManager<EventManager>().TriggerEvent(EventFlag.PlayTimeLine, new EventParam() { stringParam = "Damaged", intParam = 2 });
		Define.GetManager<ResourceManager>().Destroy(obj);
	}

	private IEnumerator Destroy(float timer)
	{
		yield return new WaitForSeconds(timer);
		if (_shake)
			Define.GetManager<EventManager>().TriggerEvent(EventFlag.PlayTimeLine, new EventParam() { stringParam = "Damaged", intParam = 2 });
		Define.GetManager<ResourceManager>().Destroy(obj);
	}
}
