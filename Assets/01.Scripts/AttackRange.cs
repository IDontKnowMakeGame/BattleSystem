using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Core;
using System.Linq;

public struct MinDistanceObj
{
    public GameObject obj;
    public float distance;

    public MinDistanceObj(GameObject _obj, float _distance)
    {
        obj = _obj;
        distance = _distance;
    }
}

public class AttackRange : MonoBehaviour
{
    HashSet<GameObject> enemys;

    private void Awake()
    {
        enemys = new HashSet<GameObject>();
    }
    private void OnTriggerEnter(Collider other)
    {
        if(!enemys.Contains(other.gameObject))
            enemys.Add(other.gameObject);
    }

    private void OnTriggerExit(Collider other)
    {
        if (enemys.Contains(other.gameObject))
            enemys.Remove(other.gameObject);
    }

    public List<GameObject> AllEnemy()
    {
        List<GameObject> currentEnemys = enemys.ToList();

        return currentEnemys;
    }

    public MinDistanceObj NearEnemy()
    {
        float minDistnace = float.MaxValue;
        GameObject temp = null;
        foreach (GameObject obj in enemys)
        {
            float distance = obj.transform.position.DistanceFlat(InGame.PlayerBase.transform.position);
            if (distance < minDistnace)
            {
                minDistnace = distance;
                temp = obj;
            }
        }
        return new MinDistanceObj(temp, minDistnace);
    }

    public void EnemysClear()
    {
        if(enemys.Count > 0)
            enemys.Clear();
    }
}
