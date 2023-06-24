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
        clickAnimator = clickText.GetComponent<Animator>();

        // ToolTip List Setting
        for (int i = 0; i < titleToolTipList.tooltipList.Count; i++)
        {
            toolTipIdx.Add(i);
        }

        ui.SetActive(false);

        if (Instnace != this)
        {
            Destroy(gameObject);
            return;
        }
        DontDestroyOnLoad(gameObject);
    }


    [SerializeField]
    private Image backgroundImg;

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
    private Animator clickAnimator;

    [SerializeField]
    private GameObject toolTipParent;
    [SerializeField]
    private GameObject progressBarParent;
    [SerializeField]
    private GameObject textvignette;
    [SerializeField]
    private GameObject clickText;

    public IEnumerator LoadScene(string sceneName, float timer = 0f)
    {
        if (IsVisbleLoading()) yield break;
        isLoading = true;

        ui.SetActive(true);

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
                    progressBarParent.SetActive(false);
                    //toolTipParent.SetActive(false);
                    //textvignette.SetActive(false);
                    yield break;
                }
            }
        }
        isLoading = false;
    }

    public bool IsVisbleLoading()
    {
        return (isLoading || nextScene);
    }

    private void Update()
    {
        if(Input.GetKeyDown(KeyCode.LeftArrow))
        {
            curIdx--;
            if(curIdx < 0)
            {
                curIdx = titleToolTipList.tooltipList.Count - 1;
            }
            titleTmp.text = titleToolTipList.tooltipList[toolTipIdx[curIdx]].Replace("\\r\\n", "\n");
            writeTmp.text = writeToolTipList.tooltipList[toolTipIdx[curIdx]].Replace("\\r\\n", "\n");
        }
        else if(Input.GetKeyDown(KeyCode.RightArrow))
        {
            curIdx = (++curIdx) % titleToolTipList.tooltipList.Count;
            titleTmp.text = titleToolTipList.tooltipList[toolTipIdx[curIdx]].Replace("\\r\\n", "\n");
            writeTmp.text = writeToolTipList.tooltipList[toolTipIdx[curIdx]].Replace("\\r\\n", "\n");
        }
        else if(nextScene && Input.anyKey && !Input.GetKey(KeyCode.LeftArrow) && !Input.GetKey(KeyCode.RightArrow))
        {
            StartCoroutine(Fade(false));
            nextScene = false;
        }

    }

    private void OnSceneLoaded(Scene arg0, LoadSceneMode arg1)
    {
        if (arg0.name == loadSceneName)
        {
            //StartCoroutine(Fade(false));
            clickText.SetActive(true);
            clickAnimator.SetTrigger("Click");
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
            Core.Define.GetManager<SoundManager>().Play("Sounds/BackGround/" + loadSceneName + "BackGround", Core.Define.Sound.Bgm);
        }
    }

    private void SetUI()
    {
        progressBarParent.SetActive(true);
        toolTipParent.SetActive(true);
        textvignette.SetActive(true);
        clickText.SetActive(false);

        // background 설정
        int rand = Random.Range(0, bgSprites.Count);
        backgroundImg.sprite = bgSprites[rand];

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
