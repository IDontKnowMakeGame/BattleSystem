using Core;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EffectDestory : MonoBehaviour
{
	[SerializeField]
	private GameObject obj;

	public void OnTriggerEnter(Collider other)
	{
		Define.GetManager<ResourceManager>().Destroy(obj);
	}
}
