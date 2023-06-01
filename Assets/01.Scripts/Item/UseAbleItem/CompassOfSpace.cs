using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Core;

public class CompassOfSpace : UseAbleItem
{
    private Transform arrow;

    private Transform targetBlock;

    public Transform TargetBlock
    {
        get => targetBlock;
        set => targetBlock = value;
    }

    private bool useItem = false;


    public override void SettingItem()
    {
        arrow = InGame.Player.transform.Find("Arrow");
        if(arrow != null)
            arrow.gameObject.SetActive(false);
    }

    public override bool UseItem()
    {
        if (useItem)
        {
            Debug.Log("나침반 리셋");
            Reset();
            return true;
        }
        Debug.Log("나침반 시작");
        arrow.gameObject.SetActive(true);
        useItem = true;
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
        Vector3 dir = targetBlock.transform.position - arrow.position;
        dir.y = 0;

        Quaternion rot = Quaternion.LookRotation(dir.normalized);
        Quaternion targetRotation = Quaternion.Euler(-90f, rot.eulerAngles.y, rot.eulerAngles.z);

        arrow.localRotation = targetRotation;
    }

    private void Reset()
    {
        arrow.gameObject.SetActive(false);
        useItem = false;
    }
}
