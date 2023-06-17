using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Core;
using Acts.Characters.Player;

public class CompassOfSpace : UseAbleItem
{
    private Transform targetBlock;

    public Transform TargetBlock
    {
        get => targetBlock;
        set => targetBlock = value;
    }

    private bool useItem = false;


    public override void SettingItem()
    {
        InGame.Player.GetAct<PlayerUseAbleItem>().Arrow.transform.parent.gameObject.SetActive(false);
    }

    public override bool UseItem()
    {
        if(useItem)
        {
            Reset();
        }
        else
        {
            InGame.Player.GetAct<PlayerUseAbleItem>().Arrow.transform.parent.gameObject.SetActive(true);
            useItem = true;
        }
        return true;
    }

    public override void UpdateItem()
    {
        if (useItem)
        {
            ArrowDir();
        }
    }

    private void ArrowDir()
    {
        Vector3 dir = (targetBlock.transform.position - InGame.Player.GetAct<PlayerUseAbleItem>().Arrow.transform.position).normalized;

        dir.y = 0;

        float targetAngle = (Mathf.Atan2(dir.x, dir.z) * Mathf.Rad2Deg) + 90f;

        Debug.Log(targetAngle);

        Quaternion targetRotation = Quaternion.Euler(0f, targetAngle, 0f);

        InGame.Player.GetAct<PlayerUseAbleItem>().Arrow.transform.localRotation = targetRotation;
    }

    private void Reset()
    {
        InGame.Player.GetAct<PlayerUseAbleItem>().Arrow.transform.parent.gameObject.SetActive(false);
        useItem = false;
    }
}
