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
        LongRoot curRoot = LongRoot.Idle;
        if (moving || attack || skill)
            curRoot =  AttackCheck();

        if (curRoot == LongRoot.None)
            Debug.LogError("잘못된 방향을 지정하였습니다.");

        return (int)curRoot;
    }
    private LongRoot AttackCheck()
    {
        if (setDir == Vector3.left || setDir == Vector3.right)
            return LongRoot.VerticalAttack;
        else if (setDir == Vector3.forward)
            return LongRoot.UpAttack;
        else if (setDir == Vector3.forward)
            return LongRoot.DownAttack;
        return LongRoot.None;
    }
}
