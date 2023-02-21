using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

enum LongRoot
{
    None,
    Idle,
    VerticalMove,
    UpMove,
    DownMove,
    VerticalAttack,
    UpAttack,
    DownAttack,
    VerticalSkill,
    UpSkill,
    DownSkill,
    ChangeWeapon
}

public class OldLongAnimator : WeaponAnimator
{
    public override int AnimationCheck()
    {
        LongRoot curRoot = LongRoot.Idle;
        if(moving)
        {
            curRoot =  MovingCheck();
        }
        else if(attack)
        {
            curRoot = AttackCheck();
        }
        else if(skill)
        {
            curRoot = SkillCheck();
        }

        if (curRoot == LongRoot.None)
            Debug.LogError("잘못된 방향을 지정하였습니다.");

        return (int)curRoot - 1;
    }
    private LongRoot MovingCheck()
    {
        if (setDir == Vector3.left || setDir == Vector3.right)
            return LongRoot.VerticalMove;
        else if (setDir == Vector3.forward)
            return LongRoot.UpMove;
        else if (setDir == Vector3.forward)
            return LongRoot.DownMove;
        return LongRoot.None;
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

    private LongRoot SkillCheck()
    {
        if (setDir == Vector3.left || setDir == Vector3.right)
            return LongRoot.VerticalSkill;
        else if (setDir == Vector3.forward)
            return LongRoot.UpSkill;
        else if (setDir == Vector3.forward)
            return LongRoot.DownSkill;
        return LongRoot.None;
    }
}
