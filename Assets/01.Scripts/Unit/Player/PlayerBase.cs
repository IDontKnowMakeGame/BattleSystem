using System.Collections;
using System.Collections.Generic;
using Unit;
using Unit.Player;
using UnityEngine;
using UnityEngine.PlayerLoop;

public class PlayerBase : UnitBase
{
    [SerializeField] private PlayerSkill _playerSkill = null;
    [SerializeField] private PlayerMove playerMove = null;
    protected override void Init()
    {
        playerMove = AddBehaviour<PlayerMove>(playerMove);
	}

    protected override void Awake()
    {
        base.Awake();
    }
}
