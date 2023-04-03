using Core;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;

public class TImeLinePlayer : MonoBehaviour
{
    private PlayableDirector _playable;

    private bool _isPlaying;

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

    private void PlayTimeLine(EventParam eventParam)
    {
        if (_isPlaying) return;

        ChangeTimeLine(TimeLineNumber(eventParam.stringParam));
        _playable.Play();
    }

    private int TimeLineNumber(string lineName)
    {
        int result = 0;
        switch(lineName)
        {
            case "Damaged":
                result = 0;
                break;
            case "StraightSword":
                result = 1;
                break;
            case "GreatSword":
                result = 2;
                break;
            case "TwinSword":
                result = 3;
                break;
            case "Spear":
                result = 4;
                break;
            case "Bow":
                result = 5;
                break;
        }
        return result;
    }

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
