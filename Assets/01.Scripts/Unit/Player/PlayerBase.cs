using System.Collections;
using System.Collections.Generic;
using Unit;
using Unit.Player;
using UnityEngine;
using UnityEngine.PlayerLoop;

public class PlayerBase : UnitBase
{
    
    [SerializeField] private PlayerMove playerMove = null;
    [SerializeField] private PlayerAttack playerAttack = null;
    [SerializeField] private PlayerStats playerStats = null;
    
    protected override void Init()
    {
        playerMove = AddBehaviour<PlayerMove>(playerMove);
        playerAttack = AddBehaviour<PlayerAttack>(playerAttack);
        playerStats = AddBehaviour<PlayerStats>(playerStats);
    }

    protected override void Awake()
    {
        base.Awake();
    }
}
