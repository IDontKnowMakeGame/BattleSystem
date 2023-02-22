using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Units.Behaviours.Unit;

enum WeaponAnimation
{
    LongSword,
    TwinSowrd
}
[System.Serializable]
public class PlayerAnimation : UnitAnimation
{
    private List<WeaponAnimator> weaponAnimators = new List<WeaponAnimator>();
    public List<WeaponAnimator> WeaponAnimators => weaponAnimators;
    private WeaponAnimator curWeaponAnimator;
    public WeaponAnimator CurWeaponAnimator
    {
        get => curWeaponAnimator;
        set
        {
            curWeaponAnimator = value;
            curWeaponAnimator.Init();
        }
    }

    public override void Awake()
    {
        weaponAnimators.Add(new OldLongAnimator());
        weaponAnimators.Add(new OldTwinAnimator());
        base.Awake();
    }

    public override void Update()
    {
        base.Update();
    }

    public void SetAnmation()
    {
        ChangeState(curWeaponAnimator.AnimationCheck());
    }
}
