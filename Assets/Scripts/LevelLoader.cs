using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

[RequireComponent(typeof(Animator))]
public class LevelLoader : MonoBehaviour
{
    [SerializeField]
    LevelLoadEvent data;
    [SerializeField]
    Image loadingBar;
    [SerializeField]
    TMP_Text loadingProgressText;
    [SerializeField]
    TMP_Text loadingHintText;
    [Space]
    [Range(0,1)]
    [SerializeField]
    float actualLoadingEndsAt=0.5f;
    [SerializeField]
    int iterationsForLoadingSimulation=3;

    AsyncOperation loading;
    Animator loadingAnimator;
    int randomNumber;
    int levelToLoad=0;
    static LevelLoader Instance;

    readonly int fadeInHash = Animator.StringToHash("FadeIn");
    readonly int fadeOutHash = Animator.StringToHash("FadeOut");

    private void Awake()
    {
        if (Instance != null)
        {
            Destroy(gameObject);
            return;
        }
        else
        {
            Instance = this;
        }
        DontDestroyOnLoad(gameObject);
        loadingAnimator = GetComponent<Animator>();
    }

    private void OnEnable()
    {
        data.Register(this);
    }
    private void OnDisable()
    {
        data.DeRegister(this);
    }

    public void LoadLevel()
    {
        levelToLoad = data.LoadLevelNumber;
        StartCoroutine(Loading());
    }

    IEnumerator Loading()
    {
        SetLoadingProgress(0);
        loadingHintText.text = data.GetRandomTip();

        if (loadingAnimator!=null)
            loadingAnimator.SetTrigger(fadeInHash);

        yield return new WaitForSeconds(1f);
        loading= SceneManager.LoadSceneAsync(levelToLoad);

        while (loading.progress<1)
        {
            SetLoadingProgress(loading.progress*actualLoadingEndsAt);
            yield return null;
        }
        SetLoadingProgress(actualLoadingEndsAt);
        randomNumber = (int)(actualLoadingEndsAt * 100);
        yield return new WaitForSeconds(1f);
        for (int i = 0; i < iterationsForLoadingSimulation; i++)
        {
            randomNumber= Random.Range(randomNumber, 90);
            SetLoadingProgress(randomNumber/100f);
            yield return new WaitForSeconds(1f/iterationsForLoadingSimulation);
        }
        SetLoadingProgress(1);

        if (loadingAnimator!=null)
            loadingAnimator.SetTrigger(fadeOutHash);
    }

    void SetLoadingProgress(float progress)
    {
        progress=(float)decimal.Round((decimal)progress,2);
        Debug.Log(progress);
        loadingBar.fillAmount = progress;
        loadingProgressText.text = "Loading " + progress * 100 + "%";
    }

    public void OnEventRaised(LevelLoadEvent passedEvent)
    {
        data = passedEvent;
        LoadLevel();
    }
}
