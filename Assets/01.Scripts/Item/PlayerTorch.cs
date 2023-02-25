using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Core;

[System.Serializable]
public class PlayerTorch : UsableItem
{
    private Torch torch;

    public void Start()
    {
        torch = InGame.PlayerBase.GetComponentInChildren<Torch>();
        torch.ChangeTorchLight(false);
    }

    protected override void Use()
    {
        if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            torch.ChangeTorchLight(!torch.TorchLight.enabled);
        }
    }
}
