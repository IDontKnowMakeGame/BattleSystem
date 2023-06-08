using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class MolotovCocktail : MonoBehaviour
{
    private Rigidbody _rigid;

    private void Start()
    {
        _rigid = this.GetComponent<Rigidbody>();
    }

    void Update()
    {
        if(Input.GetKeyDown(KeyCode.N))
        {
            Vector3 dir = Vector3.left + Vector3.up;
            _rigid.AddForce(dir * 3.5f, ForceMode.Impulse);
            _rigid.useGravity = true;
        }

        if (transform.position.y < 0)
            Destroy(gameObject);
    }
}
