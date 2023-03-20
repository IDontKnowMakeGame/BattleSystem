using Core;
using Data;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class UIManager : MonoBehaviour
{
    public static UIManager Instance;

    public UIDocument _document;

    public UIInGame InGame = new UIInGame();
    public UIInventory Inventory = new UIInventory();


    private void Awake()
    {
        if(Instance != null)
        {
            Debug.LogError("Mutiple UIManager");
        }
        Instance = this;

        _document = GetComponent<UIDocument>();



    }

    private void Start()
    {

        Init();
    }

    private void Init()
    {
        InGame.Init();
        Inventory.Init();
    }

    private void Update()
    {
        if(Input.GetKeyDown(KeyCode.A))
        {
            Inventory.ShowInventory();
        }
    }

}
