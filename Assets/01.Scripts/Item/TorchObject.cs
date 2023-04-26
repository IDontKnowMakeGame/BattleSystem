using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Actors.Characters.NPC;
using Core;
using System;

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
            torchLight.SetActive(true);
        }
    }

    public override void Interact()
    {
        if (InGame.Player.Position.IsNeighbor(Position) == false || isOn) return;

        torchLight.SetActive(true);
        Define.GetManager<DataManager>().OnCrital(Int32.Parse(gameObject.name));
    }
}
