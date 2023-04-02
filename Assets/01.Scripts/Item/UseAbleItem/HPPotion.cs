using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Core;
using Data;
using Actors.Characters;

[System.Serializable]
public class HPPotion : UseAbleItem
{
    private float maxHealth = 100f;
    private float currentHealth = 0f;
    private float healRate = 70f; // HP per second(100 / 1.5 = 66.67)
    private float healDuration = 1.5f;
    private float lastHP = 0f;

    private bool _useHP = false;
    public bool UsePortion => _useHP;

    [SerializeField]
    private ParticleSystem holyParticle;

    private PlayerStatAct _playerStatAct;

    private float timer = 0f;


    public override void SettingItem()
    {
        ResetPotion();
        _playerStatAct = InGame.Player.GetAct<PlayerStatAct>();
    }

    public override void UseItem()
    {
        if(!_useHP)
        {
            //InGame.Player.AddState(CharacterState.StopMove);
            _useHP = true;
            //holyParticle.Play();
        }
    }

    public override void UpdateItem()
    {
        if(_useHP)
        {
            if (timer > healDuration)
            {
                ResetPotion();
                return;
            }

            timer += Time.deltaTime;
            float healAmount = healRate * Time.deltaTime;
            currentHealth += healAmount;
            currentHealth = Mathf.Clamp(currentHealth, 0f, maxHealth);

            int healedAmount = Mathf.RoundToInt(currentHealth) - Mathf.RoundToInt(lastHP);
            lastHP = currentHealth;
            _playerStatAct.Heal(healedAmount);
            UIManager.Instance.InGame.ChangeCurrentHP(_playerStatAct.PercentHP());
        }
    }

    public void ResetPotion()
    {
        Debug.Log(timer);
        currentHealth = 0f;
        lastHP = 0f;
        timer = 0f;
        _useHP = false;
    }
}

