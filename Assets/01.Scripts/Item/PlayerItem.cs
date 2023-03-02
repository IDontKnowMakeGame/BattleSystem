using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Units.Behaviours.Unit;
using System;
using Managements.Managers;

enum ItemID
{
    Torch = 001,
    Shield = 002,
    HP =  101
}

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

    private Dictionary<int, UsableItem> keyItems;
    private Dictionary<int, UsableItem> idItems;

    private const int KEYNULL = 987654321;

    public override void Awake()
    {
        keyItems = new Dictionary<int, UsableItem>();
        idItems = new Dictionary<int, UsableItem>();

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

        InputManager.OnItemPress += UseItem;

        // ID ¼¼ÆÃ
        idItems.Add((int)ItemID.Torch, playerTorch);
        idItems.Add((int)ItemID.Shield, playerShield);
        idItems.Add((int)ItemID.HP, playerPortion);

        keyItems.Add(2, playerTorch);
        keyItems.Add(1, playerShield);
        keyItems.Add(3, playerPortion);
    }

    public override void Update()
    {
        playerPortion.Update();
        playerTorch.Update();
        playerShield.Update();

        base.Update();
    }

    public void ItemAddOrClear(int key, int id = KEYNULL)
    {
        if(keyItems.ContainsKey(key))
        {
            keyItems = null;
        }

        if (id != KEYNULL && idItems.ContainsKey(id))
            keyItems[key] = idItems[id];
    }

    public void UseItem(int key)
    {
        if(keyItems.ContainsKey(key))
        {
            keyItems[key].Use();
        }
    }
}
