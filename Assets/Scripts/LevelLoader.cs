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
    [Range(0, 1)]
    [SerializeField]
    float actualLoadingEndsAt = 0.5f;
    [SerializeField]
    float delayAfterActualLoading = 1f;
    [SerializeField]
    int iterationsForLoadingSimulation = 3;

    bool isLoading=false;
    AsyncOperation loading;
    Animator loadingAnimator;
    int randomNumber;
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
        if (isLoading)
            return;
        isLoading=true;
        StartCoroutine(Loading(data.LoadLevelNumber));
    }

    IEnumerator Loading(int levelNumber)
    {
        SetLoadingProgress(0);
        loadingHintText.text = data.GetRandomTip();

        if (loadingAnimator != null)
            loadingAnimator.SetTrigger(fadeInHash);

        yield return new WaitForSeconds(1f);
        loading = SceneManager.LoadSceneAsync(levelNumber);

        while (loading.progress < 1)
        {
            SetLoadingProgress(loading.progress * actualLoadingEndsAt);
            yield return null;
        }
        SetLoadingProgress(actualLoadingEndsAt);
        randomNumber = (int)(actualLoadingEndsAt * 100);

        float delay = delayAfterActualLoading / iterationsForLoadingSimulation;

        for (int i = 0; i < iterationsForLoadingSimulation; i++)
        {
            randomNumber = Random.Range(randomNumber, 100);
            SetLoadingProgress(randomNumber / 100f);
            yield return new WaitForSeconds(delay);
        }
        SetLoadingProgress(1);
        isLoading = false;
        if (loadingAnimator != null)
            loadingAnimator.SetTrigger(fadeOutHash);
    }

    void SetLoadingProgress(float progress)
    {
        progress = (float)decimal.Round((decimal)progress, 2);
        loadingBar.fillAmount = progress;
        loadingProgressText.text = "Loading " + progress * 100 + "%";
    }

    public void OnEventRaised(LevelLoadEvent passedEvent)
    {
        data = passedEvent;
        LoadLevel();
    }
}
