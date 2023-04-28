using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EffectRotate : MonoBehaviour
{
	[SerializeField]
	EffectObject effect;

	void Update()
    {
		//-90ÇØ¶ó
		//Vector3 vec = effect.targetObject.transform.position - this.transform.position;
		//
		//this.transform.localRotation = Quaternion.LookRotation(vec);
	}
}
