using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class MolotovCocktail : MonoBehaviour
{
    private Rigidbody _rigid;
    public GameObject explosition;
    public GameObject flames;

    Vector3 startPos, endPos;

    private bool isPlay = false;

    private float anim;

    private void Start()
    {
        _rigid = this.GetComponent<Rigidbody>();

        startPos = transform.position;
        endPos = (transform.position + new Vector3(-3, 0, 0)).SetY(0);
    }

    void Update()
    {
        if(Input.GetKeyDown(KeyCode.N))
        {
            isPlay = true;
        }

        if (isPlay)
        {
            transform.Rotate(new Vector3(20, 0, 0));
            Move();
        }

        if (transform.position.y < 0)
        {
            Instantiate(explosition, transform.position.SetY(0.5f), Quaternion.identity);
            Instantiate(flames, transform.position.SetY(0.5f), Quaternion.identity);
            Destroy(gameObject);
        }
    }

    private void Move()
    {
        anim += Time.deltaTime;

        transform.position = Parabola(startPos, endPos, 1.4f, anim / 1f);
    }


    public Vector3 Parabola(Vector3 start, Vector3 end, float height, float t)
    {
        Func<float, float> f = x => -4 * height * x * x + 4 * height * x;

        var mid = Vector3.Lerp(start, end, t);

        return new Vector3(mid.x, f(t) + Mathf.Lerp(start.y, end.y, t), mid.z);
    }
}
