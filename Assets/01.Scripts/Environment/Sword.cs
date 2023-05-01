using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Sword : MonoBehaviour
{
    [SerializeField]
    private float power = 200f;

    private Rigidbody rigidbody;

    private void Start()
    {
        rigidbody = GetComponent<Rigidbody>();
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.X))
        {
            //rigidbody.constraints = ~(RigidbodyConstraints.FreezeAll);


            rigidbody.AddExplosionForce(power, transform.position, 20f, 20f);
        }
    }
}
