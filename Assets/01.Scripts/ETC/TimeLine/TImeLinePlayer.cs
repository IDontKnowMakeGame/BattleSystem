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

    //±è´ëÇöÀÌ Â«
    private int TimeLineNumber(string lineName) => lineName switch
    {
        "Damaged" => 0,
		"StraightSword" => 1,
		"GreatSword" => 2,
		"TwinSword" => 3,
		"Spear" => 4,
		"Bow" => 5, 
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
