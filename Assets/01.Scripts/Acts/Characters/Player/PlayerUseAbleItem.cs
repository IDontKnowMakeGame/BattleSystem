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
        private HPPotion _hpPotion;

        private FirstMap _fristMap;
        private SecondMap _secondMap;
        private ThirdMap _thirdMap;

        public HPPotion HPPotion => _hpPotion;
        public Shield Shield => _shieldItem;


        private Dictionary<ItemID, UseAbleItem> useAbleItems = new Dictionary<ItemID, UseAbleItem>();

        [SerializeField]
        private ParticleSystem holyParticle;

        public override void Start()
        {
            base.Start();

            _torchItem = new Torch();
            _shieldItem = new Shield();
            _hpPotion = new HPPotion();

            _fristMap = new FirstMap();
            _secondMap = new SecondMap();
            _thirdMap = new ThirdMap();

            _hpPotion.HolyParticle = holyParticle;

            useAbleItems.Add(ItemID.Torch, _torchItem);
            useAbleItems.Add(ItemID.Shield, _shieldItem);
            useAbleItems.Add(ItemID.HPPotion, _hpPotion);

            useAbleItems.Add(ItemID.FirstMap, _fristMap);
            useAbleItems.Add(ItemID.SecondMap, _secondMap);
            useAbleItems.Add(ItemID.ThirdMap, _thirdMap);

            InputManager<Weapon>.OnItemPress += CheckItem;

            _hpPotion.SettingItem();
            _shieldItem.SettingItem();
            _torchItem.SettingItem();

            _fristMap.SettingItem();
            _secondMap.SettingItem();
            _thirdMap.SettingItem();
        }

        private void CheckItem(int itemKey)
        {
            List<ItemID> allItems = Define.GetManager<DataManager>().LoadUsableItemList();

            if (allItems.Count < itemKey)
                return;

            ItemID currentID = allItems[itemKey - 1];

            if(currentID != ItemID.None)
            {

                SaveItemData currentData = Define.GetManager<DataManager>().LoadItemFromInventory(currentID);
                int cnt = currentData.currentCnt;

                if (cnt <= 0) return;

                useAbleItems[currentID].UseItem();
                currentData.currentCnt--;
                UIManager.Instance.InGame.SetItemPanelCnt(currentID);
                Define.GetManager<DataManager>().ChangeItemInfo(currentData);
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
