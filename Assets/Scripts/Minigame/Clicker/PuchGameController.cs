using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using System.Collections;

public class PunchGameController : MinigameController
{
    [Header("UI")]
    public Image mainDisplayImage; 
    public Image actionDisplayImage;
    public Image forceBarFill;

    private PunchMinigameSO gameData;
    private PunchLevelConfig currentLevelConfig;
    
    private float currentForce = 0f;
    private Tweener forceTweener;
    private bool isWaitingForEnd = false;
    private Coroutine punchRoutine;

    protected override void OnInit()
    {
        if (MinigameManager.Instance != null)
        {
            baseGameData = MinigameManager.Instance.CurrentData;
            gameData = baseGameData as PunchMinigameSO;
        }
    }

    protected override float GetBaseTimeLimit() => gameData != null ? gameData.TimeLimit : 30f;

    protected override void OnGameStart()
    {
        currentForce = 0f;
        isWaitingForEnd = false;
        forceBarFill.fillAmount = 0f;

        if (actionDisplayImage != null && gameData != null && gameData.ActionStanceSprite != null)
        {
            actionDisplayImage.sprite = gameData.ActionStanceSprite;
        }

        UpdateCurrentLevel();
    }

    protected override void OnUpdate()
    {
        if (isWaitingForEnd) return;

        currentForce -= currentLevelConfig.DecayRatePerSecond * Time.deltaTime;
        currentForce = Mathf.Clamp01(currentForce);
        
        if (forceTweener == null || !forceTweener.IsActive())
        {
            forceBarFill.fillAmount = currentForce;
        }

        UpdateCurrentLevel();
    }

    public void OnClickPunch()
    {
        if (!isPlaying || isPaused || isWaitingForEnd) return;

        currentForce += currentLevelConfig.ForceAddedPerClick;
        currentForce = Mathf.Clamp01(currentForce);

        if (forceTweener != null) forceTweener.Kill();
        forceTweener = forceBarFill.DOFillAmount(currentForce, 0.05f).SetEase(Ease.OutCubic);

        PlayActionAnimation();

        UpdateCurrentLevel();

        if (currentForce >= 1f)
        {
            StartCoroutine(WinSequenceRoutine());
        }
    }

    private void PlayActionAnimation()
    {
        if (actionDisplayImage != null && gameData != null && gameData.ActionPunchSprite != null)
        {
            actionDisplayImage.sprite = gameData.ActionPunchSprite;

            if (punchRoutine != null) StopCoroutine(punchRoutine);
            punchRoutine = StartCoroutine(ResetStanceRoutine());
        }
    }

    private IEnumerator ResetStanceRoutine()
    {
        yield return new WaitForSeconds(gameData.ActionPunchDuration);
        if (actionDisplayImage != null && gameData.ActionStanceSprite != null)
        {
            actionDisplayImage.sprite = gameData.ActionStanceSprite;
        }
    }

    private void UpdateCurrentLevel()
    {
        for (int i = gameData.LevelConfigs.Count - 1; i >= 0; i--)
        {
            if (currentForce >= gameData.LevelConfigs[i].ForceThreshold)
            {
                currentLevelConfig = gameData.LevelConfigs[i];
                if (mainDisplayImage != null && currentLevelConfig.LevelSprite != null)
                {
                    mainDisplayImage.sprite = currentLevelConfig.LevelSprite;
                }
                break;
            }
        }
    }

    private IEnumerator WinSequenceRoutine()
    {
        isWaitingForEnd = true;

        if (forceTweener != null) forceTweener.Kill();
        forceBarFill.fillAmount = 1f;

        yield return new WaitForSeconds(2f);
        WinGame();
    }

    public new void Btn_Retry()
    {
        base.Btn_Retry();
    }

    public new void Btn_Quit()
    {
        base.Btn_Quit();
    }
}