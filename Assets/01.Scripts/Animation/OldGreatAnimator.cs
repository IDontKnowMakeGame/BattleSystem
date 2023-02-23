using System;
using UnityEngine;
using Core;

enum GreatRoot
{
    None,
    Idle,
    VerticalMove,
    UpMove,
    DownMove,
    VerticalCharge,
    UpCharge,
    DownCharge,
    VeticalAttack,
    UpAttack,
    DownAttack,
    WeaponChange
}

public class OldGreatAnimator : WeaponAnimator
{
    public override int AnimationCheck()
    {
        GreatRoot curRoot = GreatRoot.Idle;
        if (weaponChange)
        {
            curRoot = GreatRoot.WeaponChange;
        }
        else if (moving)
        {
            curRoot = MovingCheck();
        }
        else if(charge)
        {
            curRoot = ChargeCheck();
        }
        else if (attack)
        {
            curRoot = AttackCheck();
        }

        if (curRoot == GreatRoot.None)
            Debug.LogError("잘못된 방향을 지정하였습니다.");

        ResetParameter();
        return (int)curRoot - 1;
    }
    private GreatRoot MovingCheck()
    {
        if (setDir == Vector3.left)
        {
            playerSprite.localScale = new Vector3(-1, 1, 1);
            return GreatRoot.VerticalMove;
        }
        else if (setDir == Vector3.right)
        {
            playerSprite.localScale = new Vector3(1, 1, 1);
            return GreatRoot.VerticalMove;
        }
        else if (setDir == Vector3.forward)
        {
            playerSprite.localScale = new Vector3(1, 1, 1);
            return GreatRoot.UpMove;
        }
        else if (setDir == Vector3.back)
        {
            playerSprite.localScale = new Vector3(1, 1, 1);
            return GreatRoot.DownMove;
        }
        return GreatRoot.None;
    }

    private GreatRoot AttackCheck()
    {
        if (setDir == Vector3.left)
        {
            playerSprite.localScale = new Vector3(-1, 1, 1);
            return GreatRoot.VeticalAttack;
        }
        else if (setDir == Vector3.right)
        {
            playerSprite.localScale = new Vector3(1, 1, 1);
            return GreatRoot.VeticalAttack;
        }
        else if (setDir == Vector3.forward)
        {
            playerSprite.localScale = new Vector3(1, 1, 1);
            return GreatRoot.UpAttack;
        }
        else if (setDir == Vector3.back)
        {
            playerSprite.localScale = new Vector3(1, 1, 1);
            return GreatRoot.DownAttack;
        }
        return GreatRoot.None;
    }

    private GreatRoot ChargeCheck()
    {
        if (setDir == Vector3.left)
        {
            playerSprite.localScale = new Vector3(-1, 1, 1);
            return GreatRoot.VerticalCharge;
        }
        else if (setDir == Vector3.right)
        {
            playerSprite.localScale = new Vector3(1, 1, 1);
            return GreatRoot.VerticalCharge;
        }
        else if (setDir == Vector3.forward)
        {
            playerSprite.localScale = new Vector3(1, 1, 1);
            return GreatRoot.UpCharge;
        }
        else if (setDir == Vector3.back)
        {
            playerSprite.localScale = new Vector3(1, 1, 1);
            return GreatRoot.DownCharge;
        }
        return GreatRoot.None;
    }

}
