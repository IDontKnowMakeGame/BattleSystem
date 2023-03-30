using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Core;
using Data;
using Actors.Characters;

[System.Serializable]
public class HPPotion : UseAbleItem
{
    private int _cnt;
    private float _timer = 0f;

    private float accTimer = 0f;

    private bool _useHP = false;
    public bool UsePortion => _useHP;

    [SerializeField]
    private float maxTimer = 0.015f;
    [SerializeField]
    private ParticleSystem holyParticle;

    private PlayerStatAct _playerStatAct;

    public override void SettingItem()
    {
        ResetPortion();
        _playerStatAct = InGame.Player.GetAct<PlayerStatAct>();
    }

    public override void UseItem()
    {
        if (!_useHP)
        {
            InGame.Player.AddState(CharacterState.StopMove);
            _useHP = true;
            holyParticle.Play();
        }
    }

    public override void UpdateItem()
    {
        if (_useHP)
            IncreaseHpPortion();

    }

    public void IncreaseHpPortion()
    {
        if (_cnt > 100 || _playerStatAct.ChangeStat.hp >= _playerStatAct.ChangeStat.maxHP)
        {
            ResetPortion();
            return;
        }

        _timer += Time.deltaTime;

        if (_timer >= maxTimer)
        {
            Debug.Log(_timer);
            accTimer += _timer;
            _timer = 0;
            _playerStatAct.Heal(1);
            UIManager.Instance.InGame.ChangeCurrentHP(_playerStatAct.PercentHP());
            _cnt++;
        }
    }

    public void ResetPortion()
    {
        Debug.Log("AccTimer:" + accTimer);
        Debug.Log("cnt" + _cnt);
        accTimer = 0f;
        InGame.Player.RemoveState(CharacterState.StopMove);
        _useHP = false;
        _cnt = 0;
        _timer = 0;
    }
}

