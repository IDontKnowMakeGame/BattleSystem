using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponSelectObject : InterectionObject
{
    protected override void TriggerEnter()
    {
        EventParam eventParam = new EventParam();
        eventParam.stringParam = "E : Click";

        Managements.GameManagement.Instance.GetManager<EventManager>().TriggerEvent(EventFlag.ShowInterection, eventParam);
        Managements.GameManagement.Instance.GetManager<EventManager>().TriggerEvent(EventFlag.WeaponPanelConnecting, eventParam);
    }
    protected override void TriggerEixt()
    {
        Managements.GameManagement.Instance.GetManager<EventManager>().TriggerEvent(EventFlag.HideInterection, new EventParam());
        Managements.GameManagement.Instance.GetManager<EventManager>().TriggerEvent(EventFlag.WeaponPanelDisConnecting, new EventParam());
    }

}
