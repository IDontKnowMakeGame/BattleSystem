using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InterectionObject : MonoBehaviour
{
    protected virtual void TriggerEnter()
    {

    }
    protected virtual void TriggerEixt()
    {

    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            Debug.Log("InPlayer");
            TriggerEnter();
        }
    }
    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            Debug.Log("OutPlayer");
            TriggerEixt();
        }
    }
}
