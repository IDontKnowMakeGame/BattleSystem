using Core;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WatterEffectReturn : MonoBehaviour
{
	[SerializeField]
	private float timer;

	private void OnEnable()
	{
		Invoke("Time", timer);
	}

	private void Time()
	{
		Define.GetManager<ResourceManager>().Destroy(this.gameObject);
	}
}
