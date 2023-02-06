using Managements.Managers;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponSelectPanel : MonoBehaviour
{
    bool _isActive = false;
    private void Start()
    {
        Managements.GameManagement.Instance.GetManager<EventManager>().StartListening(EventFlag.WeaponPanelConnecting, InterectionConecting);
        Managements.GameManagement.Instance.GetManager<EventManager>().StartListening(EventFlag.WeaponPanelDisConnecting, InterectionDisConecting);
        gameObject.SetActive(false);
    }
    private void InterectionConecting(EventParam eventParam)
    {
        Managements.GameManagement.Instance.GetManager<InputManager>().AddInGameAction(InputTarget.ShowWeaponChangePanel, InputStatus.Press, ShowWeaponSelectPanel);
    }
    private void InterectionDisConecting(EventParam eventParam)
    {
        Managements.GameManagement.Instance.GetManager<InputManager>().RemoveInGameAction(InputTarget.ShowWeaponChangePanel, InputStatus.Press, ShowWeaponSelectPanel);
    }

    private void ShowWeaponSelectPanel()
    {
        if(!_isActive)
        {
            _isActive = true;
            gameObject.SetActive(true);
        }
    }

    public void ExitBtn()
    {
        if (_isActive)
        {
            _isActive = false;
            gameObject.SetActive(false);
        }
    }
}
