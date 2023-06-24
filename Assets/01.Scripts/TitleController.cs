using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using Tool.Data.Json;

public class IntroData
{
    public bool isClearTutorial = false;
}
public class TitleController : MonoBehaviour
{
    private bool enableLoad = false;
    private bool press = false;
    [SerializeField]
    private CanvasGroup group;

    private IntroData introData;

    private Animator animator;

    private readonly int hashSkip = Animator.StringToHash("Skip");

    Sequence _sequence;

    private void Start()
    {
        introData = JsonManager.LoadJsonFile<IntroData>(Application.streamingAssetsPath + "/SAVE/Tutorial", "IntroData");
        animator = GetComponent<Animator>();
        SetResolution();
    }

    void Update()
    {
        if(Input.GetKeyDown(KeyCode.Escape) && GetNormalizedTime(animator, "NoneSkip") <= 0.7f)
        {
            animator.SetTrigger(hashSkip);
        }

        else if (enableLoad && Input.anyKeyDown)
        {
            if (press) return;
            press = true;
            _sequence = DOTween.Sequence();
            _sequence.Append(group.DOFade(0f, 0.2f));
            _sequence.Append(group.DOFade(0.3f, 0.1f));
            _sequence.Append(group.DOFade(0f, 0.5f));
            _sequence.AppendCallback(() =>
            {
                if(introData.isClearTutorial)
                    LoadingSceneController.Instnace.StartCoroutine(LoadingSceneController.Instnace.LoadScene("Lobby"));
                else
                    LoadingSceneController.Instnace.StartCoroutine(LoadingSceneController.Instnace.LoadScene(Floor.Tutorial.ToString()));
                _sequence.Kill();
            });
        }
    }

    public void SetEnableLoad()
    {
        enableLoad = true;
    }

    public void SetResolution()
    {
        int setWidth = 1920;
        int setHeight = 1080;

        Screen.SetResolution(setWidth, setHeight, true);
    }

    private float GetNormalizedTime(Animator animator, string tag)
    {
        AnimatorStateInfo currentInfo = animator.GetCurrentAnimatorStateInfo(0);
        AnimatorStateInfo nextInfo = animator.GetNextAnimatorStateInfo(0);

        if (animator.IsInTransition(0) && nextInfo.IsTag(tag))
        {
            return nextInfo.normalizedTime;
        }
        else if (!animator.IsInTransition(0) && currentInfo.IsTag(tag))
        {
            return currentInfo.normalizedTime;
        }
        else
        {
            return 0f;
        }
    }
}
