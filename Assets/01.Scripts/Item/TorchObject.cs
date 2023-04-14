using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Actors.Characters.NPC;
using Core;

public class TorchObject : InteractionActor
{
    [SerializeField] private GameObject torchLight;
    protected override void Start()
    {

    }

    public override void Interact()
    {
        if (InGame.Player.Position.IsNeighbor(Position) == false) return;

        torchLight.SetActive(true);
    }
}
