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

    private void Start()
    {

    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.M))
        {
            /*
            foreach (GameObject obj in enemys)
            {
                Destroy(obj);
            }
            enemys.Clear();
            */
            Destroy(NearEnemy().obj);
        }
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
