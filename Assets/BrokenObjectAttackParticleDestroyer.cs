using Core;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BrokenObjectAttackParticleDestroyer : MonoBehaviour
{
	private ParticleSystem[] particle;

	private void Awake()
	{
		particle = GetComponentsInChildren<ParticleSystem>();
	}

	private void Update()
	{
		foreach(ParticleSystem p in particle)
		{
			if (p.IsAlive())
				return;
		}

		Debug.Log("Destroy");
		Define.GetManager<ResourceManager>().Destroy(this.gameObject);
	}
}
