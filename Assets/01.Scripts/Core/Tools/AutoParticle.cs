using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Manager;

[RequireComponent(typeof(ParticleSystem))]
public class AutoParticle : MonoBehaviour
{
	public bool OnlyDeactivate;

	void OnEnable()
	{
		StartCoroutine("CheckIfAlive");
	}

	IEnumerator CheckIfAlive()
	{
		while (true)
		{
			yield return new WaitForSeconds(0.05f);
			if (!GetComponent<ParticleSystem>().IsAlive(true))
			{
				if (OnlyDeactivate)
				{
					GameManagement.Instance.GetManager<ResourceManagers>().Destroy(this.gameObject);
				}
				else
					GameObject.Destroy(this.gameObject);
				break;
			}
		}
	}
}
