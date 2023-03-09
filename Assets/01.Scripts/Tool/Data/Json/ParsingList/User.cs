using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class User
{
    public int maxHp = 100;
    public int feather = 0;

    public string firstWeapon;
    public string secondWeapon;

    public string firstHelo = "";
    public string secondHelo = "";
    public string thirdHelo = "";

    public List<ItemInfo> equipUseableItem; //0~4 x
}
