using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponSelectObject : MonoBehaviour
{

    private void OnTriggerEnter(Collider other)
    {
        if(other.CompareTag("Player"))
        {
            Debug.Log("InPlayer");
            EventParam eventParam = new EventParam();
            eventParam.stringParam = "E : Click";

            Managements.GameManagement.Instance.GetManager<EventManager>().TriggerEvent(EventFlag.ShowInterection, eventParam);
            Managements.GameManagement.Instance.GetManager<EventManager>().TriggerEvent(EventFlag.WeaponPanelConnecting, eventParam);
        }    
    }
    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            Debug.Log("OutPlayer");
            Managements.GameManagement.Instance.GetManager<EventManager>().TriggerEvent(EventFlag.HideInterection, new EventParam());
            Managements.GameManagement.Instance.GetManager<EventManager>().TriggerEvent(EventFlag.WeaponPanelDisConnecting, new EventParam());
        }
    }
}
