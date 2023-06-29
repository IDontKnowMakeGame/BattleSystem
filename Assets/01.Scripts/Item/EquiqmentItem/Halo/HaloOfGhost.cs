using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Actors.Characters;
using Core;

public class HaloOfGhost : Halo
{
    private bool check = false;
    public override void Equiqment(CharacterActor actor)
    {
        PlayerStatAct _playerStatAct = InGame.Player.GetAct<PlayerStatAct>();
        if (_playerStatAct.ChangeStat.hp >= _playerStatAct.ChangeStat.maxHP)
            check = true;
        else
            check = false;

        _playerStatAct.Plus(StatType.MAXHP, 100);

        InGame.Player.GetAct<PlayerStatAct>().StatChange();
        if(check)
        {
            _playerStatAct.Heal(100);
        }

        UIManager.Instance.InGame.ChanageMaxHP((int)InGame.Player.GetAct<PlayerStatAct>().ChangeStat.maxHP / 10);
        UIManager.Instance.InGame.ChangeCurrentHP(InGame.Player.GetAct<PlayerStatAct>().PercentHP());
        Debug.Log(_playerStatAct.ChangeStat.maxHP);
    }

    public override void UnEquipment(CharacterActor actor)
    {
        Debug.Log("UnEquipement");
        PlayerStatAct _playerStatAct = InGame.Player.GetAct<PlayerStatAct>();
        _playerStatAct.Sub(StatType.MAXHP, 100);

        InGame.Player.GetAct<PlayerStatAct>().StatChange();
        if(check)
        {
            _playerStatAct.Heal(0);
        }

        UIManager.Instance.InGame.ChanageMaxHP((int)InGame.Player.GetAct<PlayerStatAct>().ChangeStat.maxHP / 10);
        UIManager.Instance.InGame.ChangeCurrentHP(InGame.Player.GetAct<PlayerStatAct>().PercentHP());
    }
}
