using System;
using System.Collections;
using System.Collections.Generic;
using Manager;
using Unit;
using Unit.Player;
using UnityEngine;
using UnityEngine.PlayerLoop;

public class PlayerBase : UnitBase
{
    
    [SerializeField] private PlayerMove playerMove = null;
    [SerializeField] private PlayerAttack playerAttack = null;
    [SerializeField] private PlayerStats playerStats = null;
    [SerializeField] private PlayerWeapon playerWeapon = null;

    protected override void Init()
    {
        playerMove = AddBehaviour<PlayerMove>(playerMove);
        playerAttack = AddBehaviour<PlayerAttack>(playerAttack);
        playerStats = AddBehaviour<PlayerStats>(playerStats);
        playerWeapon = AddBehaviour<PlayerWeapon>(playerWeapon);
    }

    protected override void Awake()
    {
        base.Awake();
    }
    
    protected override void Update()
    {
        if(GameManagement.Instance.GetManager<InputManager>().GetKeyDownInput(InputManager.InputSignal.Skill))
            playerWeapon.UseSkill();
        base.Update();
    }
}
