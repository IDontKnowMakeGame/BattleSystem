using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Actors.Characters.NPC;
using Core;
using Managements.Managers;
using System;
using Acts.Characters;

public class TorchObject : InteractionActor
{
    [SerializeField] private GameObject torchLight;

    private bool isOn = false;
    protected override void Start()
    {
        Floor floor = DataManager.MapData_.currentFloor;
        isOn = Define.GetManager<DataManager>().IsOnCrital(Int32.Parse(gameObject.name), floor);
        if (isOn)
        {
            ChangeWarmTiles();
            torchLight.SetActive(true);
            return;
        }

        characterDetect.EnterDetect += ShowDetect;
        characterDetect.ExitDetect += HideDetect;
    }
    public void ShowDetect(Vector3 vec)
    {
        Debug.Log($"{gameObject.name}Enter Cristal Zone");
        UIManager.Instance.InGame.ShowInteraction();
    }
    public void HideDetect(Vector3 vec)
    {
        UIManager.Instance.InGame.HideInteraction();
    }
    public override void Interact()
    {
        if (InGame.Player.Position.IsNeighbor(Position) == false || isOn) return;

        base.Interact();
        ChangeWarmTiles();
        torchLight.SetActive(true);
        Define.GetManager<DataManager>().OnCrital(Int32.Parse(gameObject.name));
        characterDetect.EnterDetect -= ShowDetect;
        characterDetect.ExitDetect -= HideDetect;
        HideDetect(Vector3.zero);
    }

    public void ChangeWarmTiles()
    {
        MapManager _map = Define.GetManager<MapManager>();

        for (int z = -1; z <= 1f; z++)
        {
            for(int x = -1; x <= 1f; x++)
            {
                float exploreZ = transform.position.z + z;
                float exploreX = transform.position.x + x;

                if(_map.GetBlock(new Vector3(exploreX, 0, exploreZ)) != null)
                {
                    _map.GetBlock(new Vector3(exploreX, 0, exploreZ)).isWarm = true;
                }
            }
        }
    }
}
