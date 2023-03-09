using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class User
{
    public int maxHp = 100;
    public int feather = 0;

    public ItemID firstWeapon = ItemID.None;
    public ItemID secondWeapon = ItemID.None;

    public ItemID firstHelo = ItemID.None;
    public ItemID secondHelo = ItemID.None;
    public ItemID thirdHelo = ItemID.None;

    public List<ItemInfo> equipUseableItem; //0~4 x
}
