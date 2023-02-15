using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class TextInterectionPanelUI : MonoBehaviour
{
    private TextMeshProUGUI _text;
    private Transform _panel;

    private void Awake()
    {
        _text = transform.Find("BackGround/Text").GetComponent<TextMeshProUGUI>();
        _panel = transform.Find("BackGround").GetComponent<Transform>();
    }
    private void Start()
    {
        Managements.GameManagement.Instance.GetManager<EventManager>().StartListening(EventFlag.ShowInterection, ShowTextPanel);
        Managements.GameManagement.Instance.GetManager<EventManager>().StartListening(EventFlag.HideInterection, HideTextPanel);
        _panel.gameObject.SetActive(false);
    }

    private void ShowTextPanel(EventParam param)
    {
        _text.text = param.stringParam;
        _panel.gameObject.SetActive(true);
    }

    private void HideTextPanel(EventParam param)
    {
        _panel.gameObject.SetActive(false);
    }

    private void OnDisable()
    {
        var manager = Core.Define.GetManager<EventManager>();
        manager?.StopListening(EventFlag.ShowInterection, ShowTextPanel);
        manager?.StopListening(EventFlag.HideInterection, HideTextPanel);
    }
}
