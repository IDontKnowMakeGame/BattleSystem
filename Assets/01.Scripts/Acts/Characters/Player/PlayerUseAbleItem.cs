using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Acts.Base;
using Data;
using Managements.Managers;

namespace Acts.Characters.Player
{
    [System.Serializable]
    public class PlayerUseAbleItem : Act
    {
        private Torch _torchItem;
        private Shield _shieldItem;
        [SerializeField]
        private HPPotion _hpPotion;

        public HPPotion HPPotion => _hpPotion;


        private Dictionary<ItemID, UseAbleItem> useAbleItems = new Dictionary<ItemID, UseAbleItem>();

        private Dictionary<int, ItemID> keyItems = new Dictionary<int, ItemID>();


        public override void Start()
        {
            base.Start();

            _torchItem = new Torch();
            _shieldItem = new Shield();
            _hpPotion = new HPPotion();

            useAbleItems.Add(ItemID.Torch, _torchItem);
            useAbleItems.Add(ItemID.Shield, _shieldItem);
            useAbleItems.Add(ItemID.HPPotion, _hpPotion);

            keyItems.Add(1, ItemID.HPPotion);
            keyItems.Add(2, ItemID.Shield);
            keyItems.Add(3, ItemID.Torch);

            InputManager<Weapon>.OnItemPress += ChckItem;

            _hpPotion.SettingItem();
            _shieldItem.SettingItem();
            _torchItem.SettingItem();
        }

        private void ChckItem(int itemKey)
        {
            ItemID currentID = ItemID.None;
            if(keyItems.TryGetValue(itemKey, out currentID))
            {
                useAbleItems[currentID].UseItem();
            }
        }

        public override void Update()
        {
            base.Update();

            _hpPotion.UpdateItem();

            /*            if(Input.GetKeyDown(KeyCode.Alpha1))
                        {
                            torchItem.UseItem();
                        }
                        if(Input.GetKeyDown(KeyCode.Alpha2))
                        {
                            shieldItem.UseItem();
                        }*/
        }
    }
}
