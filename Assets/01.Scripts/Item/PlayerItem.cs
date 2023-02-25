using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Units.Behaviours.Unit;

[System.Serializable]
public class PlayerItem : UnitBehaviour
{
    [SerializeField]
    private PlayerPortion playerPortion;
    [SerializeField]
    private PlayerTorch playerTorch;
    public PlayerPortion PlayerPortion => playerPortion;
    public PlayerTorch PlayerTorch => playerTorch;

    public override void Awake()
    {
        playerPortion = new PlayerPortion();
        playerTorch = new PlayerTorch();

        base.Awake();
    }

    public override void Start()
    {
        playerTorch.Start();

        base.Start();
    }

    public override void Update()
    {
        playerPortion.Update();
        playerTorch.Update();

        base.Update();
    }
}
