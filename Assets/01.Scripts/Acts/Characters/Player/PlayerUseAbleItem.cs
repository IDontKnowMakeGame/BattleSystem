using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Acts.Base;
using Data;
using Managements.Managers;
using Core;

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


        public override void Start()
        {
            base.Start();

            _torchItem = new Torch();
            _shieldItem = new Shield();
            _hpPotion = new HPPotion();

            useAbleItems.Add(ItemID.Torch, _torchItem);
            useAbleItems.Add(ItemID.Shield, _shieldItem);
            useAbleItems.Add(ItemID.HPPotion, _hpPotion);

            InputManager<Weapon>.OnItemPress += CheckItem;

            _hpPotion.SettingItem();
            _shieldItem.SettingItem();
            _torchItem.SettingItem();
        }

        private void CheckItem(int itemKey)
        {
            List<ItemID> allItems = Define.GetManager<DataManager>().LoadUsableItemList();

            if (allItems.Count < itemKey)
                return;

            ItemID currentID = allItems[itemKey - 1];

            if(currentID != ItemID.None)
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
