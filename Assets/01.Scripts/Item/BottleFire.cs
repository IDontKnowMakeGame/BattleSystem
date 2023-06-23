using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Managements.Managers;
using Core;

public class BottleFire : MonoBehaviour
{
	private ParticleSystem particleSystem;

	void Start()
	{
		particleSystem = GetComponent<ParticleSystem>();
	}
	// Update is called once per frame
	void Update()
	{
		if (!particleSystem.IsAlive(true))
		{
			Define.GetManager<MapManager>().GetBlock(transform.position.SetY(0)).isFire = false;
			this.gameObject.SetActive(false);
			Define.GetManager<ResourceManager>().Destroy(this.gameObject);

		}
	}
}
