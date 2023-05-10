using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Core;
using Actors.Characters.Enemy;
using Actors.Characters;

[System.Serializable]
public class SpawnerType
{
    public EnemyType type;
    public Vector3 startPos;
}

public class UnitSpawnerController : MonoBehaviour
{
    [SerializeField]
    private List<SpawnerType> spawnUnits;

    private HashSet<CharacterActor> units = new HashSet<CharacterActor>();

    public HashSet<CharacterActor> Units => units;

    private void Start()
    {
        SpawnEnemys();

        units.Add(InGame.Player);
    }

    private void SpawnEnemys()
    {
        foreach (SpawnerType enemy in spawnUnits)
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
                units.Add(enemyObj.GetComponent<CharacterActor>());
            }
        }
    }

}