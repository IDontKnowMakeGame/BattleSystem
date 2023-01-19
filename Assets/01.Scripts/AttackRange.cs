using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class AttackRange : MonoBehaviour
{
    Dictionary<GameObject, int> enemys;

    private void Awake()
    {
        enemys = new Dictionary<GameObject, int>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if(!enemys.ContainsKey(other.gameObject))
            // To Do Distance 거리 계산 필요
            enemys[other.gameObject] = 1;
    }

    private void OnTriggerExit(Collider other)
    {
        if (enemys.ContainsKey(other.gameObject))
            enemys.Remove(other.gameObject);
    }
}
