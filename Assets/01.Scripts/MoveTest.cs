using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveTest : MonoBehaviour
{
    [SerializeField]
    private Transform root;

    [SerializeField]
    private Transform posRoot;

    [SerializeField]
    private GameObject block;

    [ContextMenu("Spawn")]
    public void Spawn()
    {
        Debug.Log("End");
        foreach(Transform t in posRoot)
        {
           GameObject b = Instantiate(block, t.position, Quaternion.identity);
            b.transform.parent = root;
            b.name = t.name;
        }
    }
}
