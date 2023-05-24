using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class TitleController : MonoBehaviour
{
    private bool enableLoad = false;
    private bool press = false;
    [SerializeField]
    private CanvasGroup group;


    private Animator animator;

    private readonly int hashSkip = Animator.StringToHash("Skip");

    Sequence _sequence;

    private void Start()
    {
        animator = GetComponent<Animator>();
        SetResolution();
    }

    void Update()
    {
        if(Input.GetKeyDown(KeyCode.Escape))
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
                LoadingSceneController.Instnace.StartCoroutine(LoadingSceneController.Instnace.LoadScene("Lobby"));
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
}
