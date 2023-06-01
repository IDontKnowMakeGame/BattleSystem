using Core;
using Data;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Diagnostics;
using UnityEngine.UIElements;
using static UnityEngine.Rendering.DebugUI;

public class UIInGame : UIBase
{
    private VisualElement _hpSlider;
    private VisualElement _hpServeSlider;
    private VisualElement _angerSlider;
    private VisualElement _adrenalineSlider;
    private VisualElement _haloIcon;

    private VisualElement _firstWaepon;
    private VisualElement _firstWeaponHide;
    private VisualElement _secondWeapon;
    private VisualElement _secondWeaponHide;
    private VisualElement _itemList;
    private VisualElement _interactionBox;

    private VisualElement _abnormalStatusBox;
    private VisualElement _abnormalStatusSliderBar;
    private VisualElement _abnormalStatusIcon;
    private int _abnormalStatusMaxCount = 0;
    private int _abnormalStatusCount = 0;

    private VisualElement _roomInfoPanel;
    private Label _roomNameText;

    //private VisualElement _questInfoPanel;
    private VisualElement _questListPanel;
    private VisualTreeAsset _questPanelTemp;

    private VisualElement _crsitalPanel;

    private VisualElement _itemPanel;
    private Queue<Pair> _itemQueue = new Queue<Pair>();
    private class Pair
    {
        public ItemID id;
        public int cnt;
    }
    private float _itemTime = 0;
    private float _hideItemTime = 2.3f;
    private bool _showItemPanel = false;

    private bool flagCool = true;

    private int currentRoom = 0;
    private int openQuestNum = 0;

    #region HP
    private float _currentHpValue = 100;
    private float _targetHpValue = 100;
    private float _hpTimer = 0;
    private float _hpDuration = 0.5f;
    private bool _IsHpServe = false;
    #endregion

    #region WeaponCool
    private float firstWeaponTimer = 0;
    private float firstWeaponDuration = 10;
    private bool IsFirstWeaponCool = false;
    private float secondWeaponTimer = 0;
    private float secondWeaponDuration = 20;
    private bool IsSecondWeaponCool = false;
    #endregion

    #region FeatherEffect;
    private float _featherEffectTime = 3;
    private float _featherEffectDuration = 3f;
    private bool _IsFeatherEffect = false;
    #endregion

    private Dictionary<ItemID, VisualElement> _invenInItems = new Dictionary<ItemID, VisualElement>();
    private Dictionary<QuestName,VisualElement> _questLlistCard = new Dictionary<QuestName,VisualElement>();

    private Label _feather;
    private Label _addFeatherCnt;

    public override void Init()
    {
        _root = UIManager.Instance._document.rootVisualElement.Q<VisualElement>("UI_InGame");

        _hpSlider = _root.Q<VisualElement>("slider_hpbar");
        _hpServeSlider = _hpSlider.Q<VisualElement>("ServeFill");
        _angerSlider = _root.Q<VisualElement>("slider_angerbar");
        _adrenalineSlider = _root.Q<VisualElement>("slider_adrenaline");
        _haloIcon = _root.Q<VisualElement>("HaloIcon");

        _firstWaepon = _root.Q<VisualElement>("weaponbox_first");
        _firstWeaponHide = _firstWaepon.Q<VisualElement>("Hide");
        _secondWeapon = _root.Q<VisualElement>("weaponbox_second");
        _secondWeaponHide = _secondWeapon.Q<VisualElement>("Hide");
        _itemList = _root.Q<VisualElement>("area_item");
        _itemPanel = _root.Q<VisualElement>("ItemPanel");
        _interactionBox = _root.Q<VisualElement>("InteractionBox");

        _abnormalStatusBox = _root.Q<VisualElement>("abnormalStatusBox");
        _abnormalStatusSliderBar = _root.Q<VisualElement>("slider_AbnormalStatusBar");
        _abnormalStatusIcon = _root.Q<VisualElement>("IabnormalStatusIcon").Q<VisualElement>("Icon");

        _roomInfoPanel = _root.Q<VisualElement>("RoomInfoPanel");
        _roomNameText = _roomInfoPanel.Q<Label>("RoomNameText");

        _questListPanel = _root.Q<VisualElement>("QuestListPanel");
        _questPanelTemp = Define.GetManager<ResourceManager>().Load<VisualTreeAsset>("UIDoc/QuestPanelTemp");

        _crsitalPanel = _root.Q<VisualElement>("area_Cristal");

        _feather = _root.Q<Label>("featherCnt");
        _addFeatherCnt = _root.Q<Label>("AddFeatherCnt");

        _feather.text = Define.GetManager<DataManager>().GetFeather().ToString();

        ChangeFirstWeaponImage(DataManager.UserData_.firstWeapon);
        ChangeSecondWeaponImage(DataManager.UserData_.secondWeapon);
        ChangeItemPanelImage();
        InitQuestPanel();
        ChangeHalo();
        HideAbnormalStatusBox();

        CristalInfoInRoom(0);
    }
    public override void Start()
    {
        OpenQuestPanel();
    }
    public override void Update()
    {
        ChangeCurrentHP();
        FirstWeaponCoolTime();
        SecondWeaponCoolTime();
        AddFeatherEffect();
        GetItemUpdate();
        OpenQuestPanel();
        WriteFeatherValue();
    }
    public void ChangeWeaponPanel()
    {
        ChangeFirstWeaponImage(DataManager.UserData_.firstWeapon);
        ChangeSecondWeaponImage(DataManager.UserData_.secondWeapon);
        ChangeWeaponCoolTime();
    }
    public void ChanageMaxHP(int value)
    {
        VisualElement bar = _hpSlider.Q<VisualElement>("bar_hpbackgroud");
        bar.style.width = new Length(value, LengthUnit.Percent);
    }
    public void ChangeCurrentHP(int value)
    {
        VisualElement bar = _hpSlider.Q<VisualElement>("Fill");
        bar.style.width = new Length(value, LengthUnit.Percent);
        _currentHpValue = _targetHpValue;
        _targetHpValue = value;
        _hpTimer = 0;
        _IsHpServe = true;
    }
    public void ChangeCurrentHP()
    {
        if (_IsHpServe == false) return;

        _hpTimer += Time.deltaTime;
        float t = Mathf.Clamp01(_hpTimer / _hpDuration);
        float currentFov = Mathf.Lerp(_currentHpValue, _targetHpValue, t);
        _hpServeSlider.style.width = new Length(currentFov, LengthUnit.Percent);

        if (_hpTimer > _hpDuration)
            _IsHpServe = false;
    }

    public void ChangeAngerValue(int value)
    {
        VisualElement bar = _angerSlider.Q<VisualElement>("Fill");
        bar.style.width = new Length(value, LengthUnit.Percent);
    }
    public void FlagAngerBuff(bool value)
    {
        VisualElement back = _angerSlider.Q<VisualElement>("BackGround");
        if (value)
            back.AddToClassList("OnAnger");
        else
            back.RemoveFromClassList("OnAnger");
    }
    public void ChangeAdrenalineValue(int value)
    {
        VisualElement bar = _adrenalineSlider.Q<VisualElement>("Fill");
        bar.style.width = new Length(value, LengthUnit.Percent);
    }
    public void FlagAdrenalineBuff(bool value)
    {
        VisualElement back = _angerSlider.Q<VisualElement>("BackGround");
        if (value)
            back.AddToClassList("OnAdrenalin");
        else
            back.RemoveFromClassList("OnAdrenalin");
    }
    public void ChangeHalo()
    {
        ItemID id = DataManager.UserData_.firstHalo;
        ChangeHalo(id);
    }
    public void ChangeHalo(ItemID id)
    {
        _haloIcon.style.backgroundImage = new StyleBackground(Define.GetManager<ResourceManager>().Load<Sprite>($"Item/{(int)id}"));
    }

    public void FlagCoolTimePanel(float coolTime)
    {
        if(flagCool)
            FirstWeaponCoolTime(coolTime);
        else
            SecondWeaponCoolTime(coolTime);
    }
    public void ChangeFirstWeaponImage(ItemID itemID)
    {
        Sprite sprite = Define.GetManager<ResourceManager>().Load<Sprite>("Item/" + (int)itemID);
        _firstWaepon.style.backgroundImage = new StyleBackground(sprite);
    }
    public void FirstWeaponCoolTime(float coolTime)
    {
        firstWeaponDuration = coolTime;
        firstWeaponTimer = 0;
    }
    public void FirstWeaponCoolTime()
    {
        firstWeaponTimer += Time.deltaTime;
        float t = Mathf.Clamp01(firstWeaponTimer / firstWeaponDuration);
        float currentFov = Mathf.Lerp(100, 0, t);
        _firstWeaponHide.style.height = new Length(currentFov, LengthUnit.Percent);
    }
    public void ChangeSecondWeaponImage(ItemID itemID)
    {
        Sprite sprite = Define.GetManager<ResourceManager>().Load<Sprite>("Item/" + (int)itemID);
        _secondWeapon.style.backgroundImage = new StyleBackground(sprite);
    }
    public void SecondWeaponCoolTime(float coolTime)
    {
        secondWeaponDuration = coolTime;
        secondWeaponTimer = 0;
    }
    public void SecondWeaponCoolTime()
    {
        secondWeaponTimer += Time.deltaTime;
        float t = Mathf.Clamp01(secondWeaponTimer / secondWeaponDuration);
        float currentFov = Mathf.Lerp(100, 0, t);
        _secondWeaponHide.style.height = new Length(currentFov, LengthUnit.Percent);
    }

    public void InitQuestPanel()
    {
        List<QuestName> list = DataManager.PlayerOpenQuestData_.openQuestList;
        for(int i = 0; i < list.Count;i++)
            AddQuestPanel(list[i]);

        list = DataManager.PlayerOpenQuestData_.readyClearQuestList;
        for (int i = 0; i < list.Count; i++)
            AddQuestPanel(list[i],true);
    }
    public void AddQuestPanel(QuestName name,bool isClearReadQuest = false)
    {
        VisualElement panel = _questPanelTemp.Instantiate();
        panel.Q<Label>("QuestName").text = name.ToString();
        _questListPanel.Add(panel);
        _questLlistCard[name] = panel;

        if(isClearReadQuest)
            ClearQuestPanel(name);

        //'OpenQuestPanel();
    }
    public void ClearQuestPanel(QuestName name)
    {
        Debug.Log($"{_questLlistCard[name].name} : ¿Ã∞‘ ππ¡“?");
        _questLlistCard[name].Q<VisualElement>("ClearMark").style.display = DisplayStyle.Flex;
        
    }
    public void OpenQuestPanel()
    {
        if (openQuestNum >= _questLlistCard.Count) return;

        openQuestNum = 0;
        foreach (VisualElement panel in _questLlistCard.Values)
        {
            panel.Q<VisualElement>("TempCard").RemoveFromClassList("CloseQuest");
            openQuestNum++;
        }
    }
    public void CloseQuestPanel(QuestName name = QuestName.none)
    {
        _questLlistCard[name].AddToClassList("CloseQuest");
    }

    public void CristalInfoInRoom(int roomNum)
    {
        currentRoom = roomNum;
        _roomNameText.text = UIManager.Instance.MapNameData.firstMapName[roomNum];
        if (roomNum == 0)
        {
            _crsitalPanel.style.display = DisplayStyle.None;
            return;
        }
        _crsitalPanel.style.display = DisplayStyle.Flex;
        UpdateCristalText();
    }
    public void UpdateCristalText()
    {
        List<int> cristalInfo = UIFirstFloorMap.castMap[currentRoom];
        List<int> onCristalData = Define.GetManager<DataManager>().LoadOnCristalData(DataManager.MapData_.currentFloor);
        int allCiristalCnt = cristalInfo.Count;
        int onCristalCnt = cristalInfo.Intersect(onCristalData).Count();
        _crsitalPanel.Q<Label>("cristalCnt").text = string.Format("{0}/{1}", onCristalCnt, allCiristalCnt);
    }

    public void ShowInteraction()
    {
        _interactionBox.style.display = DisplayStyle.Flex;
    }
    public void HideInteraction()
    {
        _interactionBox.style.display = DisplayStyle.None;
    }

    public void ShowAbnormalStatusBox()
    {
        _abnormalStatusBox.style.display = DisplayStyle.Flex;
        _abnormalStatusMaxCount = 7;
    }
    public void HideAbnormalStatusBox()
    {
        _abnormalStatusBox.style.display = DisplayStyle.None;
        _abnormalStatusMaxCount = 7;
    }
    public void SetAbnormalStatus(int count)
    {
        _abnormalStatusCount = count;
        int sliderValue = (100/ _abnormalStatusMaxCount) * count;
        _abnormalStatusSliderBar.style.width = new Length(sliderValue, LengthUnit.Percent);
    }
    public void AddAbnormalStatus(int count = 1)
    {
        if(_abnormalStatusCount == 0)
            ShowAbnormalStatusBox();
        SetAbnormalStatus(_abnormalStatusCount + count);
    }

    public void GetItemUpdate()
    {
        _itemTime += Time.deltaTime;

        if (_itemTime > _hideItemTime)
            if (_showItemPanel)
                HideItemPanel();
            else
                AddShowItemPanel();
    }
    public void AddShowItemPanel()
    {
        if (_showItemPanel || _itemQueue.Count <= 0) return;

        _itemTime = 0;
        _showItemPanel = true;
        Pair pair = _itemQueue.Dequeue();
        _itemPanel.Q<VisualElement>("Image").style.backgroundImage = new StyleBackground(Define.GetManager<ResourceManager>().Load<Sprite>($"Item/{(int)pair.id}"));
        _itemPanel.Q<Label>("ItemName").text = pair.id.ToString();
        _itemPanel.Q<Label>("ItemCntText").text = string.Format("x{0}", pair.cnt);
        _itemPanel.RemoveFromClassList("HideItemPanel");
    }
    public void AddShowItemPanel(ItemID id, int cnt = 1)
    {
        _itemQueue.Enqueue(new Pair() { id = id, cnt = cnt });
        AddShowItemPanel();
    }
    public void HideItemPanel()
    {
        _itemTime = 0;
        _showItemPanel = false;
        _itemPanel.AddToClassList("HideItemPanel");
    }

    public void ChangeWeaponCoolTime()
    {
        VisualElement temp = _firstWeaponHide;
        _firstWeaponHide = _secondWeaponHide;
        _secondWeaponHide = temp;
        flagCool = !flagCool;
    }
    public void ChangeItemPanelImage()
    {
        List<ItemID> item = Define.GetManager<DataManager>().LoadUsableItemList();
        int index = 0;
        foreach(VisualElement card in _itemList.Children())
        {
            _invenInItems[item[index]] = card;
            card.style.backgroundImage = new StyleBackground(Define.GetManager<ResourceManager>().Load<Sprite>("Item/" + (int)item[index]));
            SetItemPanelCnt(item[index]);
            index++;
        }
    }
    public void SetItemPanelCnt(ItemID itemID)
    {
        SaveItemData saveData = Define.GetManager<DataManager>().LoadItemFromInventory(itemID);
        if (saveData == null) return;
        if (_invenInItems.ContainsKey(itemID) == false) return;

        Label cntText = _invenInItems[itemID].Q<Label>("ItemCntText");
        cntText.text = saveData.currentCnt.ToString();

    }
    public void UseItemCool(ItemID itemID,int cooltime)
    {
        List<ItemID> item = Define.GetManager<DataManager>().LoadUsableItemList();
        for(int i = 0; i < item.Count; i++)
        {
            if (item[i] == itemID)
            {
                VisualElement asd =  _itemList[i].Q<VisualElement>("Hide");
            }
        }
    }
    public void AddFeatherValue(int value)
    {
        _addFeatherCnt.text = string.Format("+{0}", value);
        _featherEffectTime = 0;
        _IsFeatherEffect = false;

        _feather.text = Define.GetManager<DataManager>().GetFeather().ToString();
    }
    public void AddFeatherEffect()
    {
        if (_IsFeatherEffect) return;

        _featherEffectTime += Time.deltaTime;
        float t = Mathf.Clamp01(_featherEffectTime / _featherEffectDuration);
        float currentFov = Mathf.Lerp(1, 0, t);
        
        _addFeatherCnt.style.opacity = new StyleFloat(currentFov);
       // Debug.Log($"oppacity : {currentFov} {_addFeatherCnt.style.opacity}");

        if (currentFov <= 0)
            _IsFeatherEffect = true;
    }
    public void WriteFeatherValue()
    {
        if(Define.GetManager<DataManager>()==null) return;

        _feather.text = Define.GetManager<DataManager>().GetFeather().ToString();
    }
}
