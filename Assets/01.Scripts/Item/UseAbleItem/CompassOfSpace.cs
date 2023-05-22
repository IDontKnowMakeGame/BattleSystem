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

    private float timer = 0f;
    private float maxTimer = 4f;
    private bool useItem = false;


    public override void SettingItem()
    {
        arrow = InGame.Player.transform.Find("Arrow");
        arrow.gameObject.SetActive(false);
    }

    public override bool UseItem()
    {
        if (useItem) return false;
        timer = 0f;
        arrow.gameObject.SetActive(true);
        useItem = true;
        return true;
    }

    public override void UpdateItem()
    {
        if (useItem)
        {
            ArrowDir();
            Timer();
        }
    }

    private void ArrowDir()
    {
        Vector3 dir = targetBlock.transform.position - arrow.position;
        dir.y = 0;

        Quaternion rot = Quaternion.LookRotation(dir.normalized);
        Quaternion targetRotation = Quaternion.Euler(-90f, rot.eulerAngles.y, rot.eulerAngles.z);

        Debug.Log(targetRotation.eulerAngles);

        arrow.localRotation = targetRotation;
    }

    private void Timer()
    {
        timer += Time.deltaTime;

        if (timer >= maxTimer)
        {
            arrow.gameObject.SetActive(false);
            useItem = false;
        }
    }
}
