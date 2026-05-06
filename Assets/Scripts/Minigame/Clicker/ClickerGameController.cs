using UnityEngine;
using UnityEngine.UI;
using TMPro;
using DG.Tweening;

public class ClickerGameController : MonoBehaviour
{
    [Header("UI - Main")]
    public TextMeshProUGUI contentText;
    public TextMeshProUGUI timerText;
    public TextMeshProUGUI levelMultiplierText;
    public Image forceBarFill;
    public ScrollRect textScrollRect;

    [Header("UI - Panels")]
    public GameObject gameOverPanel;

    private ClickerMinigameSO gameData;
    private ClickLevelConfig currentLevelConfig;
    private float currentTime;
    private float currentForce = 0f;
    private int charIndex = 0;  
    private bool isPlaying = false;
    private Tweener fillTweener;

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
        
        if (fillTweener == null || !fillTweener.IsActive())
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
        gameOverPanel.SetActive(false);
        isPlaying = true;

        UpdateCurrentLevel();
    }

    public void OnClickSpam()
    {
        if (!isPlaying) return;

        currentForce += currentLevelConfig.ForceAddedPerClick;
        currentForce = Mathf.Clamp01(currentForce);

        if (fillTweener != null) fillTweener.Kill();
        fillTweener = forceBarFill.DOFillAmount(currentForce, 0.05f).SetEase(Ease.OutCubic);
        forceBarFill.transform.DOPunchScale(Vector3.one * 0.1f, 0.1f);

        UpdateCurrentLevel();

        charIndex += currentLevelConfig.CharsRevealedPerClick;
        charIndex = Mathf.Min(charIndex, gameData.TargetText.Length);
        contentText.text = gameData.TargetText.Substring(0, charIndex);

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
        if (fillTweener != null) fillTweener.Kill();
        MinigameManager.Instance.CompleteMinigame(true);
    }

    private void LoseGame()
    {
        isPlaying = false;
        if (fillTweener != null) fillTweener.Kill();
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