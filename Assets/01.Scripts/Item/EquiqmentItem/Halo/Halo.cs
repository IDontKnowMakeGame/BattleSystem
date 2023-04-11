using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Halo : EquiqmentItem
{
    protected float percent = 0;

    protected bool ConditionCheck()
    {
        int random = Random.Range(1, 101);

        if (random <= percent)
        {
            return true;
        }
        return false;
    }

    protected virtual void Using(EventParam eventParam)
    {

    }
}