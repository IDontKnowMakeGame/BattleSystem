using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;

public class LoadingSceneController : MonoBehaviour
{
    private static LoadingSceneController instance;

    public static LoadingSceneController Instnace
    {
        get
        {
            if (instance == null)
            {
                var obj = FindObjectOfType<LoadingSceneController>();
                if (obj != null)
                {
                    instance = obj;
                }
                else
                {
                    instance = Create();
                }
            }
            return instance;
        }
    }

    private static LoadingSceneController Create()
    {
        return Instantiate(Resources.Load<LoadingSceneController>("LoadingUI"));
    }

    private void Awake()
    {
        bgAnimator = backgroundImg.GetComponent<Animator>();

        // ToolTip List Setting
        for (int i = 0; i < titleToolTipList.tooltipList.Count; i++)
        {
            toolTipIdx.Add(i);
        }

        if (Instnace != this)
        {
            Destroy(gameObject);
            return;
        }
        DontDestroyOnLoad(gameObject);
    }


    [SerializeField]
    private Image backgroundImg;

    private Animator bgAnimator;

    [SerializeField]
    private List<Sprite> bgSprites;

    [SerializeField]
    private CanvasGroup canvasGroup;

    [SerializeField]
    private Image progressBar;

    [SerializeField]
    private TextMeshProUGUI titleTmp;

    [SerializeField]
    private TextMeshProUGUI writeTmp;

    [SerializeField]
    private ToolTipListSO titleToolTipList;

    [SerializeField]
    private ToolTipListSO writeToolTipList;

    [SerializeField]
    private GameObject ui;

    private string loadSceneName;

    public bool isLoading = false;

    private bool nextScene = false;

    [SerializeField]
    private List<int> toolTipIdx = new List<int>();

    private int curIdx = 0;

    public IEnumerator LoadScene(string sceneName, float timer = 0f)
    {
        if (isLoading) yield break;
        isLoading = true;

        ui.SetActive(true);

        bgAnimator.SetBool("Complete", false);

        SetUI();

        yield return new WaitForSeconds(timer);
        SceneManager.sceneLoaded += OnSceneLoaded;
        loadSceneName = sceneName;

        StartCoroutine(LoadSceneProcess());
    }

    private IEnumerator LoadSceneProcess()
    {
        progressBar.fillAmount = 0f;
        yield return StartCoroutine(Fade(true));

        AsyncOperation op = SceneManager.LoadSceneAsync(loadSceneName);
        op.allowSceneActivation = false;

        float timer = 0f;
        while (!op.isDone)
        {
            yield return null;
            if (op.progress < 0.9f)
            {
                progressBar.fillAmount = op.progress;
            }
            else
            {
                timer += Time.unscaledDeltaTime;
                progressBar.fillAmount = Mathf.Lerp(0.9f, 1f, timer);
                if (progressBar.fillAmount >= 1f)
                {
                    nextScene = true;
                    op.allowSceneActivation = true;
                    isLoading = false;
                    bgAnimator.SetBool("Complete", true);
                    Debug.Log("접근해!!");
                    yield break;
                }
            }
        }

        isLoading = false;
    }

    private void Update()
    {
        if(nextScene && Input.GetMouseButtonDown(0))
        {
            StartCoroutine(Fade(false));
            nextScene = false;
        }

        if(Input.GetKeyDown(KeyCode.LeftArrow))
        {
            curIdx--;
            if(curIdx < 0)
            {
                curIdx = titleToolTipList.tooltipList.Count - 1;
            }
            Debug.Log(curIdx);
            titleTmp.text = titleToolTipList.tooltipList[toolTipIdx[curIdx]].Replace("\\r\\n", "\n");
            writeTmp.text = writeToolTipList.tooltipList[toolTipIdx[curIdx]].Replace("\\r\\n", "\n");
        }
        else if(Input.GetKeyDown(KeyCode.RightArrow))
        {
            curIdx = (++curIdx) % titleToolTipList.tooltipList.Count;
            titleTmp.text = titleToolTipList.tooltipList[toolTipIdx[curIdx]].Replace("\\r\\n", "\n");
            writeTmp.text = writeToolTipList.tooltipList[toolTipIdx[curIdx]].Replace("\\r\\n", "\n");
        }
    }

    private void OnSceneLoaded(Scene arg0, LoadSceneMode arg1)
    {
        if (arg0.name == loadSceneName)
        {
            //StartCoroutine(Fade(false));
            SceneManager.sceneLoaded -= OnSceneLoaded;
        }
    }

    private IEnumerator Fade(bool isFadeIn)
    {
        float timer = 0f;
        while (timer <= 1f)
        {
            yield return null;
            timer += Time.unscaledDeltaTime * 3f;
            canvasGroup.alpha = isFadeIn ? Mathf.Lerp(0f, 1f, timer) : Mathf.Lerp(1f, 0f, timer);
        }

        if (!isFadeIn)
        {
            ui.SetActive(false);
        }
    }

    private void SetUI()
    {
        // background 설정
        int rand = Random.Range(0, bgSprites.Count);
        bgAnimator.SetInteger("Cnt", rand);
        Debug.Log(bgSprites[rand].name + "맞음?");
        backgroundImg.sprite = bgSprites[rand];
        Debug.Log(rand + "입니당");

        // Tooltip 설정
        for (int i = 0; i < 100; i++)
        {
            int randA = Random.Range(0, titleToolTipList.tooltipList.Count);
            int randB = Random.Range(0, titleToolTipList.tooltipList.Count);

            int swap = toolTipIdx[randA];
            toolTipIdx[randA] = toolTipIdx[randB];
            toolTipIdx[randB] = swap;
        }

        curIdx = 0;

        titleTmp.text = titleToolTipList.tooltipList[toolTipIdx[curIdx]].Replace("\\r\\n", "\n");
        writeTmp.text = writeToolTipList.tooltipList[toolTipIdx[curIdx]].Replace("\\r\\n", "\n");
    }
}
