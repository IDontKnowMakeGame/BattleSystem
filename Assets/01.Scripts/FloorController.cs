using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum Enemys
{
    enemy01,
    enemy02,
    enemy03
}

public class FloorController : MonoBehaviour
{
    [SerializeField]
    private List<FloorSO> floors;
    [SerializeField]
    public List<GameObject> enemys;

    [SerializeField]
    private int currentFloor = 1;

    private void Start()
    {
        SpawnUnit();
    }

    private void SpawnUnit()
    {
        foreach(FloorData data in floors[currentFloor - 1].FloorData)
        {
            Instantiate(enemys[(int)data.enemy], data.pos, Quaternion.identity);
        }
    }
}
