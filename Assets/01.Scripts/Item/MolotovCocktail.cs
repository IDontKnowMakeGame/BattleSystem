using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class MolotovCocktail : MonoBehaviour
{
    private Rigidbody _rigid;
    public GameObject explosition;
    public GameObject flames;

    private bool isPlay = false;

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
            isPlay = true;
        }

        if(isPlay)
            transform.Rotate(new Vector3(20, 0, 0));

        if (transform.position.y < 0)
        {
            Instantiate(explosition, transform.position.SetY(0.5f), Quaternion.identity);
            SpawnFlames();
            Destroy(gameObject);
        }
    }

    private void SpawnFlames()
    {
        for(int i = -1; i <= 1; i++)
        {
            Instantiate(flames, transform.position.SetY(0.5f).SetX(transform.position.x + i), Quaternion.identity);
        }

    }
}
