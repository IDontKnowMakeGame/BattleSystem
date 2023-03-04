using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[System.Serializable]
public class FloorData
{
    public Enemys enemy;
    public Vector3 pos;
}

[CreateAssetMenu(fileName = "FloorData", menuName = "Scriptable Object/FloorData")]
public class FloorSO : ScriptableObject
{
    [SerializeField]
    private List<FloorData> floorData;

    public List<FloorData> FloorData => floorData;
}
