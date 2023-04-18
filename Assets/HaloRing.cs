using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HaloRing : MonoBehaviour
{
    [SerializeField]
    private GameObject[] obj;

    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        this.transform.Rotate(Vector3.forward * Time.deltaTime * 10);
        obj[0].transform.localRotation = this.transform.localRotation;
		obj[1].transform.localRotation = this.transform.localRotation;
	}
}
