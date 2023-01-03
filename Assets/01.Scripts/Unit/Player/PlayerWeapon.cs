using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Behaviour = Unit.Behaviour;

[Serializable]
public class PlayerWeapon : Behaviour
{
    [UnityEngine.SerializeField] private WeaponSO weapon;
    private List<Action> weaponSkills = new List<Action>();

    public override void Awake()
    {
        weaponSkills.Add(GreatSwordSkill);
        weaponSkills.Add(LongSwordSkill);
    }

    public void SetWeapon(WeaponSO weapon)
    {
        this.weapon = weapon;
    }

    public void UseSkill()
    {
        weaponSkills[weapon.idx]?.Invoke();
    }
    
    //Skills are added to the list in the order they are in the weaponSO
    private void GreatSwordSkill()
    {
        Debug.Log("great");
        //Do something
    }
    
    private void LongSwordSkill()
    {
        Debug.Log("long");
        //Do something
    }
}
