using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Core;
using Managements.Managers;

public class TorchController : MonoBehaviour
{
    void Start()
    {
        ChangeWarmTiles();

        MapManager _map = Define.GetManager<MapManager>();
        bool mode = _map.GetBlock(InGame.Player.Position.SetY(0)).isWarm;
        InGame.Player.GetAct<PlayerFlooding>().ChangeWarmMode(mode);

    }

    public void ChangeWarmTiles()
    {
        MapManager _map = Define.GetManager<MapManager>();

        for (int z = -1; z <= 1f; z++)
        {
            for (int x = -1; x <= 1f; x++)
            {
                float exploreZ = transform.position.z + z;
                float exploreX = transform.position.x + x;

                if (_map.GetBlock(new Vector3(exploreX, 0, exploreZ)) != null)
                {
                    _map.GetBlock(new Vector3(exploreX, 0, exploreZ)).isWarm = true;
                }
            }
        }
    }
}
