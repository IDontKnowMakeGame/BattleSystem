using System;
using System.Collections;
using System.Collections.Generic;
using Manager;
using Unit.Player;
using Unity.VisualScripting;
using UnityEngine;
using UnitBase = Unit.UnitBase;

public class PlayerBase : UnitBase
{
    
    [SerializeField] private PlayerMove playerMove;
    [SerializeField] private PlayerAttack playerAttack;
    [SerializeField] private PlayerStats playerStats;
    [SerializeField] private PlayerWeapon playerWeapon;

    protected override void Init()
    {
        playerMove = AddBehaviour<PlayerMove>(playerMove);
        playerAttack = AddBehaviour<PlayerAttack>(playerAttack);
        playerStats = AddBehaviour<PlayerStats>(playerStats);
        playerWeapon = AddBehaviour<PlayerWeapon>(playerWeapon);
    }

    protected override void Update()
    {
        base.Update();
    }
}
