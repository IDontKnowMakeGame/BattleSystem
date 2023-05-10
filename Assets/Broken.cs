using Managements;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Broken : MonoBehaviour
{
	private Rigidbody[] objs;

	private void Awake()
	{
		objs = GetComponentsInChildren<Rigidbody>();
	}

	public void Brokens(Vector3 dir, float power = 2)
	{
		if (dir != Vector3.zero)
			for (int i = 0; i < objs.Length; i++)
			{
				objs[i].AddForce(-dir * 5, ForceMode.Impulse);
			}

		GameObject obj = GameManagement.Instance.GetManager<ResourceManager>().Instantiate("BrokenObjectAttackParticle");
		obj.transform.position = this.transform.position;
	}
}
