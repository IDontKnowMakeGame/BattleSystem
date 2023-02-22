using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

enum TwinRoot
{
    None,
    Idle,
    VerticalAttack,
    UpAttack,
    DownAttack,
    ChangeWeapon
}
public class OldTwinAnimator : WeaponAnimator
{
    public override int AnimationCheck()
    {
        TwinRoot curRoot = TwinRoot.Idle;
        if (weaponChange)
        {
            curRoot = TwinRoot.ChangeWeapon;
        }
        else if (moving || attack || skill)
            curRoot = AttackCheck();

        if (curRoot == TwinRoot.None)
            Debug.LogError("잘못된 방향을 지정하였습니다.");

        ResetParameter();
        return (int)curRoot - 1;
    }
    private TwinRoot AttackCheck()
    {
        if (setDir == Vector3.left || setDir == Vector3.right)
            return TwinRoot.VerticalAttack;
        else if (setDir == Vector3.forward)
            return TwinRoot.UpAttack;
        else if (setDir == Vector3.down)
            return TwinRoot.DownAttack;
        return TwinRoot.None;
    }
}
