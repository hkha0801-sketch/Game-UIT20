using UnityEngine;
using UnityEngine.UI;
using TMPro;
using DG.Tweening;

public class ClickerGameController : MinigameController
{
    [Header("Clicker UI")]
    public TextMeshProUGUI contentText;
    public TextMeshProUGUI levelMultiplierText;
    public Image forceBarFill;
    public ScrollRect textScrollRect;
    public Image progressBarFill;
    public TextMeshProUGUI progressText;

    private ClickerMinigameSO gameData;
    private ClickLevelConfig currentLevelConfig;
    private float currentForce = 0f;
    private int charIndex = 0;
    private Tweener forceTweener;
    private Tweener progressTweener;

    protected override void OnInit()
    {
        if (MinigameManager.Instance != null)
        {
            baseGameData = MinigameManager.Instance.CurrentData;
            gameData = baseGameData as ClickerMinigameSO;
        }
    }

    protected override float GetBaseTimeLimit() => gameData != null ? gameData.TimeLimit : 30f;

    protected override void OnGameStart()
    {
        charIndex = 0;
        currentForce = 0f;
        contentText.text = "";
        forceBarFill.fillAmount = 0f;
        if (progressBarFill != null) progressBarFill.fillAmount = 0f;
        if (progressText != null) progressText.text = "0%";
        
        UpdateCurrentLevel();
    }

    protected override void OnUpdate()
    {
        currentForce -= currentLevelConfig.DecayRatePerSecond * Time.deltaTime;
        currentForce = Mathf.Clamp01(currentForce);
        
        if (forceTweener == null || !forceTweener.IsActive())
        {
            forceBarFill.fillAmount = currentForce;
        }

        UpdateCurrentLevel();
    }

    public void OnClickSpam()
    {
        if (!isPlaying || isPaused) return;

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
        if (textScrollRect != null) textScrollRect.verticalNormalizedPosition = 0f;

        if (charIndex >= gameData.TargetText.Length)
        {
            if (forceTweener != null) forceTweener.Kill();
            if (progressTweener != null) progressTweener.Kill();
            WinGame();
        }
    }

    private void UpdateProgressUI()
    {
        if (progressBarFill == null) return;
        float progress = (float)charIndex / gameData.TargetText.Length;
        if (progressText != null) progressText.text = Mathf.FloorToInt(progress * 100f).ToString() + "%";

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
}