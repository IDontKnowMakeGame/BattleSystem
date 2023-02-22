using System;
using UnityEngine;

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
        if (setDir == Vector3.left || setDir == Vector3.right)
            return GreatRoot.VerticalMove;
        else if (setDir == Vector3.forward)
            return GreatRoot.UpMove;
        else if (setDir == Vector3.back)
            return GreatRoot.DownMove;
        return GreatRoot.None;
    }

    private GreatRoot AttackCheck()
    {
        if (setDir == Vector3.left || setDir == Vector3.right)
            return GreatRoot.VeticalAttack;
        else if (setDir == Vector3.forward)
            return GreatRoot.UpAttack;
        else if (setDir == Vector3.back)
            return GreatRoot.DownAttack;
        return GreatRoot.None;
    }

    private GreatRoot ChargeCheck()
    {
        if (setDir == Vector3.left || setDir == Vector3.right)
            return GreatRoot.VerticalCharge;
        else if (setDir == Vector3.forward)
            return GreatRoot.UpCharge;
        else if (setDir == Vector3.back)
            return GreatRoot.DownCharge;
        return GreatRoot.None;
    }

}
