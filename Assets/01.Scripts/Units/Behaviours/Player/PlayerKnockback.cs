using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Units.Behaviours.Unit;
using DG.Tweening;
using Core;
using Managements.Managers;
using Units.Base.Unit;

[System.Serializable]
public class PlayerKnockback : UnitBehaviour
{
    private Sequence _seq;

    public override void Update()
    {
        if(Input.GetKeyDown(KeyCode.N))
        {
            KnockBack(Vector3.back, 2);
        }
        base.Update();
    }

    public void KnockBack(Vector3 direction, int power)
    {
        Vector3 orginPos = ThisBase.Position;
        Vector3 targetPos = orginPos;
        int checkPower = 0;

        var map = Define.GetManager<MapManager>();

        for (int i = 1; i <= power; i++)
        {
            Vector3 checkPos = targetPos + (direction * i);
            if (InGame.GetUnit(checkPos) != null || 
                map.GetBlock(checkPos) == null || !map.GetBlock(checkPos).isWalkable)
            {
                break;
            }
            checkPower++;
        }

        targetPos = targetPos + (direction * checkPower);
        targetPos.y = 1;

        InGame.SetUnit(ThisBase, targetPos);
        ThisBase.AddState(BaseState.Knockback);

        _seq = DOTween.Sequence();
        _seq.Append(ThisBase.transform.DOLocalMove(targetPos, 0.3f).SetEase(Ease.OutBack)); 
        _seq.AppendCallback(() =>
        {
            ThisBase.Position = targetPos;
            InGame.SetUnit(null, orginPos);
            ThisBase.RemoveState(BaseState.Knockback);
        });
    }
}
