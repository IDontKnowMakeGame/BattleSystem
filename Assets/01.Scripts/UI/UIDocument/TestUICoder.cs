using Core;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class TestUICoder : MonoBehaviour
{
    [SerializeField]
    private ItemStoreTableSO so;
    private void Awake()
    {
        
    }

    private void Start()
    {
        Define.GetManager<DataManager>().AddItemInInventory(ItemID.OldStraightSword);
        //Define.GetManager<DataManager>().AddFeahter(10000);
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.K))
        {
            Define.GetManager<UIManager>().ShowInventory();
        }
    }
}
