using UnityEngine;
using UnityEngine.UI;
using TMPro;
using DG.Tweening;

public class ClickerGameController : MonoBehaviour
{[Header("UI - Main")]
    public TextMeshProUGUI contentText;
    public TextMeshProUGUI timerText;
    public TextMeshProUGUI levelMultiplierText;
    public Image forceBarFill;
    public ScrollRect textScrollRect;

    [Header("UI - Progress")]
    public Image progressBarFill;
    public TextMeshProUGUI progressText;

    [Header("UI - Panels")]
    public GameObject gameOverPanel;

    private ClickerMinigameSO gameData;
    private ClickLevelConfig currentLevelConfig;
    private float currentTime;
    private float currentForce = 0f;
    private int charIndex = 0;
    private bool isPlaying = false;
    
    private Tweener forceTweener;
    private Tweener progressTweener;

    void Start()
    {
        InitializeGame();
    }

    void Update()
    {
        if (!isPlaying) return;

        currentTime -= Time.deltaTime;
        timerText.text = "Deadline: " + Mathf.CeilToInt(currentTime).ToString() + "s";

        if (currentTime <= 0)
        {
            LoseGame();
            return;
        }

        currentForce -= currentLevelConfig.DecayRatePerSecond * Time.deltaTime;
        currentForce = Mathf.Clamp01(currentForce);
        
        if (forceTweener == null || !forceTweener.IsActive())
        {
            forceBarFill.fillAmount = currentForce;
        }

        UpdateCurrentLevel();
    }

    private void InitializeGame()
    {
        if (MinigameManager.Instance == null || !(MinigameManager.Instance.CurrentData is ClickerMinigameSO))
        {
            return;
        }

        gameData = MinigameManager.Instance.CurrentData as ClickerMinigameSO;
        currentTime = gameData.TimeLimit;
        charIndex = 0;
        currentForce = 0f;
        
        contentText.text = "";
        forceBarFill.fillAmount = 0f;
        if (progressBarFill != null) progressBarFill.fillAmount = 0f;
        if (progressText != null) progressText.text = "0%";
        
        gameOverPanel.SetActive(false);
        isPlaying = true;

        UpdateCurrentLevel();
    }

    public void OnClickSpam()
    {
        if (!isPlaying) return;

        currentForce += currentLevelConfig.ForceAddedPerClick;
        currentForce = Mathf.Clamp01(currentForce);

        if (forceTweener != null) forceTweener.Kill();
        forceTweener = forceBarFill.DOFillAmount(currentForce, 0.05f).SetEase(Ease.OutCubic);
        
        forceBarFill.transform.DOKill(true);
        forceBarFill.transform.DOPunchScale(Vector3.one * 0.1f, 0.1f);

        UpdateCurrentLevel();

        charIndex += currentLevelConfig.CharsRevealedPerClick;
        charIndex = Mathf.Min(charIndex, gameData.TargetText.Length);
        contentText.text = gameData.TargetText.Substring(0, charIndex);

        UpdateProgressUI();

        Canvas.ForceUpdateCanvases();
        if (textScrollRect != null)
        {
            textScrollRect.verticalNormalizedPosition = 0f;
        }

        if (charIndex >= gameData.TargetText.Length)
        {
            WinGame();
        }
    }

    private void UpdateProgressUI()
    {
        if (progressBarFill == null) return;

        float progress = (float)charIndex / gameData.TargetText.Length;

        if (progressText != null)
        {
            progressText.text = Mathf.FloorToInt(progress * 100f).ToString() + "%";
        }

        if (progressTweener != null) progressTweener.Kill();
        progressTweener = progressBarFill.DOFillAmount(progress, 0.1f).SetEase(Ease.OutQuad);

        progressBarFill.transform.DOKill(true);
        progressBarFill.transform.DOPunchScale(Vector3.one * 0.05f, 0.1f);
    }

    private void UpdateCurrentLevel()
    {
        for (int i = gameData.LevelConfigs.Count - 1; i >= 0; i--)
        {
            if (currentForce >= gameData.LevelConfigs[i].ForceThreshold)
            {
                currentLevelConfig = gameData.LevelConfigs[i];
                levelMultiplierText.text = "x" + currentLevelConfig.CharsRevealedPerClick.ToString();
                break;
            }
        }
    }

    private void WinGame()
    {
        isPlaying = false;
        if (forceTweener != null) forceTweener.Kill();
        if (progressTweener != null) progressTweener.Kill();
        MinigameManager.Instance.CompleteMinigame(true);
    }

    private void LoseGame()
    {
        isPlaying = false;
        if (forceTweener != null) forceTweener.Kill();
        if (progressTweener != null) progressTweener.Kill();
        gameOverPanel.SetActive(true);
    }

    public void RetryGame()
    {
        InitializeGame();
    }

    public void QuitGame()
    {
        MinigameManager.Instance.CompleteMinigame(false);
    }
}