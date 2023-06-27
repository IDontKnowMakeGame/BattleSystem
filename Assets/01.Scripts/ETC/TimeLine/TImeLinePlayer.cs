using Core;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;

public class TImeLinePlayer : MonoBehaviour
{
    private PlayableDirector _playable;
    [SerializeField]
    private BloodController _obj;
    [SerializeField]
    private BloodController _obj2;

    private bool _isPlaying;

    private int tutorialPhase = 0;
    private int pageNum = 0;

    [SerializeField] 
    private List<PlayableAsset> _timelines = new List<PlayableAsset>();
    private void Awake()
    {
        _playable = GetComponent<PlayableDirector>();
    }
    private void Start()
    {
        Define.GetManager<EventManager>().StartListening(EventFlag.PlayTimeLine, PlayTimeLine);
    }
    public void StartTimeLine()
    {
        _isPlaying = true;
    }
    public void EndTimeLine()
    {
        _isPlaying = false;
    }
    
    private int count = 0;
    public void SpreadBlood()
    {
        if (count == 0)
        {
            _obj.BloodSpread(0.2f);
		}
        else if(count == 1)
        {
			_obj2.BloodSpread(0.5f);
		}
        count++;
	}

    public void EndSpreadBlood()
    {
        _obj.EndBlood();
        _obj2.EndBlood();
	}

    private float _timer = 0;
    public void Fade(float value)
    {
        ScreenEffect.Fade.Invoke(_timer, value);
    }

    public void FadeTime(float timer)
    {
        _timer = timer;
	}
    public void TutorialPhase(int num)
    {
        pageNum = num;
        UIManager.Instance.MoveAndInputStop();
        switch(num)
        {
            case 4:
                DataManager.UserData_.firstWeapon = Data.ItemID.OldGreatSword;
                break;
            case 5:
                DataManager.UserData_.firstWeapon = Data.ItemID.OldTwinSword;
                break;
            case 6:
                DataManager.UserData_.firstWeapon = Data.ItemID.OldSpear;
                break;
            case 7:
                DataManager.UserData_.firstWeapon = Data.ItemID.OldBow;
                break;
        }
    }
    public void ChangeWeapon()
    {
        Define.GetManager<EventManager>().TriggerEvent(EventFlag.WeaponEquip, new EventParam());
    }
    public void ShowExplanation()
    {
        UIManager.Instance.Explanation.Show(pageNum);
        Define.GetManager<EventManager>().TriggerEvent(EventFlag.TutorialBossRevive, new EventParam());
    }
    public void TutorialLine()
    {
        Debug.Log($"Phase : {tutorialPhase}");
        switch (tutorialPhase)
        {
            case 0:
                tutorialPhase++;
                PlayTimeLine("TutorialGreatSword");
                break;
            case 1:
                tutorialPhase++;
                PlayTimeLine("TutorialTwinSword");
                break;
            case 2:
                tutorialPhase++;
                PlayTimeLine("TutorialSpear");
                break;
            case 3:
                PlayTimeLine("TutorialBow");
                break;
        }
    }
    public void PlayTimeLine(string name)
    {
        _isPlaying = true;
        ChangeTimeLine(TimeLineNumber(name));
        _playable.Play();
    }
    private void PlayTimeLine(EventParam eventParam)
    {
        if (_isPlaying) return;

        ChangeTimeLine(TimeLineNumber(eventParam.stringParam));
        _playable.Play();
    }


    //±è´ëÇöÀÌ Â«
    private int TimeLineNumber(string lineName) => lineName switch
    {
        "Damaged" => 0,
		"StraightSword" => 1,
		"GreatSword" => 2,
		"TwinSword" => 3,
		"Spear" => 4,
		"Bow" => 5, 
        "CrazyGhost" => 6,
		"OldGreatSkill" => 7,
		"Execute" => 8,
		"KnightStatue" => 9,
        "TutorialGreatSword" => 10,
        "TutorialTwinSword" => 11,
        "TutorialSpear" => 12,
        "TutorialBow" => 13,
        _ => 0
	};

    private void ChangeTimeLine(int number)
    {
        if (number > _timelines.Count)
        {
            Debug.LogError("IndexOut {List number}");
            return;
        }
        _playable.playableAsset = _timelines[number];
    }
}
