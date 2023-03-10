using Managements.Managers;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponSelectPanel : MonoBehaviour
{
    private Transform _panel;
    bool _isActive = false;
    private void Awake()
    {
        _panel = transform.Find("Panel").GetComponent<Transform>();
    }
    private void Start()
    {
        Managements.GameManagement.Instance.GetManager<EventManager>().StartListening(EventFlag.WeaponPanelConnecting, InterectionConecting);
        Managements.GameManagement.Instance.GetManager<EventManager>().StartListening(EventFlag.WeaponPanelDisConnecting, InterectionDisConecting);
        _panel.gameObject.SetActive(false);
    }
    private void InterectionConecting(EventParam eventParam)
    {
        InputManager.OnInteractionPress += ShowWeaponSelectPanel;
    }
    private void InterectionDisConecting(EventParam eventParam)
    {
        InputManager.OnInteractionPress -= ShowWeaponSelectPanel;
    }

    private void ShowWeaponSelectPanel()
    {
        if(!_isActive)
        {
            _isActive = true;
            _panel.gameObject.SetActive(true);
        }
    }

    public void ExitBtn()
    {
        if (_isActive)
        {
            _isActive = false;
            _panel.gameObject.SetActive(false);
        }
    }

    private void OnDisable()
    {
        var manager = Core.Define.GetManager<EventManager>();
        manager?.StopListening(EventFlag.WeaponPanelConnecting, InterectionConecting);
        manager?.StopListening(EventFlag.WeaponPanelDisConnecting, InterectionDisConecting);
    }
}
