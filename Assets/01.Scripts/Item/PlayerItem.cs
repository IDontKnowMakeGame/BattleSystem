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
    [SerializeField]
    private PlayerShield playerShield;

    public PlayerPortion PlayerPortion => playerPortion;
    public PlayerTorch PlayerTorch => playerTorch;
    public PlayerShield PlayerShield => playerShield;

    public override void Awake()
    {
        playerPortion = new PlayerPortion();
        playerTorch = new PlayerTorch();
        playerShield = new PlayerShield();

        base.Awake();
    }

    public override void Start()
    {
        playerTorch.Start();
        playerPortion.Start();
        playerShield.Start();

        base.Start();
    }

    public override void Update()
    {
        playerPortion.Update();
        playerTorch.Update();
        playerShield.Update();

        base.Update();
    }
}
