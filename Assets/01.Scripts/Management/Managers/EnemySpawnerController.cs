using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Core;
using Actors.Characters.Enemy;

[System.Serializable]
public class SpawnerType
{
    public EnemyType type;
    public Vector3 startPos;
}

public class EnemySpawnerController : MonoBehaviour
{
    [SerializeField]
    private List<SpawnerType> spawnEnemys;

    private HashSet<GameObject> enemys = new HashSet<GameObject>();

    public HashSet<GameObject> Enemys => enemys;

    private void Start()
    {
        SpawnEnemys();
    }

    private void SpawnEnemys()
    {
        foreach (SpawnerType enemy in spawnEnemys)
        {
            GameObject enemyObj = null;
            switch (enemy.type)
            {
                case EnemyType.OldShade:
                    enemyObj = Define.GetManager<ResourceManager>().Instantiate("OldShade");
                    break;
            }
            if (enemyObj != null)
            {
                enemyObj.transform.position = enemy.startPos;
                enemys.Add(enemyObj);
            }
        }
    }

}