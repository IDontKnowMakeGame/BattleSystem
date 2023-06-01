using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.VFX;

public class HitEffectPlay : MonoBehaviour
{
	[SerializeField]
	private VisualEffect veg;

	private void Start()
	{
		veg.Play();
	}
}
