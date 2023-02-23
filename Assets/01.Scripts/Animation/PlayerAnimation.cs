using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Units.Behaviours.Unit;

enum WeaponAnimation
{
    LongSword,
    TwinSword,
    GreatSword
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
        weaponAnimators.Add(new OldGreatAnimator());
        base.Awake();
    }

    public override void Update()
    {
        if(!curWeaponAnimator.LastChange && IsFinished())
        {
            curWeaponAnimator.LastChange = true;
        }
        base.Update();
    }

    public void SetAnmation()
    {
        ChangeState(curWeaponAnimator.AnimationCheck());
    }
}
