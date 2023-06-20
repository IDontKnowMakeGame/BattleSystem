using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Core;
using Blocks;
using Managements.Managers;

public class TorchController : MonoBehaviour
{
    MapManager _map;

    [SerializeField]
    private float downSpeed = 10f;

    void Start()
    {
        ChangeWarmTiles();

        _map = Define.GetManager<MapManager>();
        if (_map == null || _map.GetBlock(InGame.Player.Position.SetY(0)) == null) return; 
        bool mode = _map.GetBlock(InGame.Player.Position.SetY(0)).isWarm;
        InGame.Player.GetAct<PlayerFlooding>().ChangeWarmMode(mode);

    }

    private void Update()
    {
        var block = _map.GetBlock(transform.position.SetY(0));
        if (block is EmptyBlock)
        {
            if(block is FallingBlock)
            {
                var fallBlock = block as FallingBlock;
                if (fallBlock.IsFalling == false)
                    return;
            }
            transform.Translate(0, -downSpeed * Time.deltaTime, 0);
        }
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
