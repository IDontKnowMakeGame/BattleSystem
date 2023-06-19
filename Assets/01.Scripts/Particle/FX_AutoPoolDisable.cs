using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Core;

[RequireComponent(typeof(ParticleSystem))]
public class FX_AutoPoolDisable : MonoBehaviour
{
	void OnEnable()
	{
		StartCoroutine("CheckIfAlive");
	}

	IEnumerator CheckIfAlive()
	{
		while (true)
		{
			yield return new WaitForSeconds(0.5f);
			if (!GetComponent<ParticleSystem>().IsAlive(true))
			{
				this.gameObject.SetActive(false);
				Define.GetManager<ResourceManager>().Destroy(this.gameObject);
				break;
			}
		}
	}
}
