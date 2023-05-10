using Core;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParticleReturn : MonoBehaviour
{
    private ParticleSystem[] system;

    IEnumerator co;
    // Start is called before the first frame update
    void Start()
    {
        system = GetComponentsInChildren<ParticleSystem>();
        co = corutine();
        StartCoroutine(co);
	}

    IEnumerator corutine()
    {
        yield return new WaitForSeconds(0.5f);
        Define.GetManager<ResourceManager>().Destroy(this.gameObject);
    }

    public void OnDisable()
    {
        StopCoroutine(co);
	}
}
