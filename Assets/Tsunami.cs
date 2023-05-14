using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.VFX;

public class Tsunami : MonoBehaviour
{
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.M))
        {
            GetComponent<VisualEffect>().Stop();
        }
    }
}
