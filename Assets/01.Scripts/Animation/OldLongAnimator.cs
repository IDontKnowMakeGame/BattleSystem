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
        if(weaponChange)
        {
            curRoot = LongRoot.ChangeWeapon;
        }
        else if(moving)
        {
            if (skill)
            {
                curRoot = SkillCheck();
                Debug.Log("Skill 사용");
            }
            else
                curRoot = MovingCheck();
        }
        else if(attack)
        {
            curRoot = AttackCheck();
        }

        if (curRoot == LongRoot.None)
            Debug.LogError("잘못된 방향을 지정하였습니다.");

        ResetParameter();
        return (int)curRoot - 1;
    }
    private LongRoot MovingCheck()
    {
        if (setDir == Vector3.left)
        {
            playerSprite.localScale = new Vector3(-1, 1, 1);
            return LongRoot.VerticalMove;
        }
        else if (setDir == Vector3.right)
        {
            playerSprite.localScale = new Vector3(1, 1, 1);
            return LongRoot.VerticalMove;
        }
        else if (setDir == Vector3.forward)
        {
            playerSprite.localScale = new Vector3(1, 1, 1);
            return LongRoot.UpMove;
        }
        else if (setDir == Vector3.back)
        {
            playerSprite.localScale = new Vector3(1, 1, 1);
            return LongRoot.DownMove;
        }
        return LongRoot.None;
    }
    private LongRoot AttackCheck()
    {
        if (setDir == Vector3.left)
        {
            playerSprite.localScale = new Vector3(-1, 1, 1);
            return LongRoot.VerticalAttack;
        }
        else if (setDir == Vector3.right)
        {
            playerSprite.localScale = new Vector3(1, 1, 1);
            return LongRoot.VerticalAttack;
        }
        else if (setDir == Vector3.forward)
        {
            playerSprite.localScale = new Vector3(1, 1, 1);
            return LongRoot.UpAttack;
        }
        else if (setDir == Vector3.back)
        {
            playerSprite.localScale = new Vector3(1, 1, 1);
            return LongRoot.DownAttack;
        }

        return LongRoot.None;
    }

    private LongRoot SkillCheck()
    {
        if (setDir == Vector3.left)
        {
            playerSprite.localScale = new Vector3(-1, 1, 1);
            return LongRoot.VerticalSkill;
        }
        else if (setDir == Vector3.right)
        {
            playerSprite.localScale = new Vector3(1, 1, 1);
            return LongRoot.VerticalSkill;
        }
        else if (setDir == Vector3.forward)
        {
            playerSprite.localScale = new Vector3(1, 1, 1);
            return LongRoot.UpSkill;
        }
        else if (setDir == Vector3.back)
        {
            playerSprite.localScale = new Vector3(1, 1, 1);
            return LongRoot.DownSkill;
        }
        return LongRoot.None;
    }
}
