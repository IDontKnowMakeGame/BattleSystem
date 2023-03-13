using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class User
{
    public int maxHp = 100;
    public int feather = 0;

    public ItemID firstWeapon = ItemID.None;
    public ItemID secondWeapon = ItemID.None;

    public ItemID firstHalo = ItemID.None;
    public ItemID secondHalo = ItemID.None;
    public ItemID thirdHalo = ItemID.None;

    public List<ItemData> equipUseableItem; //0~4 x
}
