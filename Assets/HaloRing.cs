using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HaloRing : MonoBehaviour
{
    [SerializeField]
    private GameObject[] _obj;


    [SerializeField]
    private float _speed;
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        this.transform.Rotate(Vector3.forward * Time.deltaTime * _speed);
	}
}
