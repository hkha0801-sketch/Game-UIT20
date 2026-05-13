using UnityEngine;
using TMPro;
using DG.Tweening;


public abstract class MinigameController : MonoBehaviour
{
    [Header("Base UI - Text")]
    public TextMeshProUGUI timerText;
    public TextMeshProUGUI startPanelTitleText;
    public TextMeshProUGUI startPanelRulesText;

    [Header("Base UI - Panels")]
    public GameObject startPanel;
    public GameObject pausePanel;
    public GameObject gameOverPanel;
    public GameObject victoryPanel;

    [Header("Victory Panel Setup")]
    public UnityEngine.UI.Image stickerImage;
    public TextMeshProUGUI congratulationText;
    public TextMeshProUGUI clickToContinueText;
    public UnityEngine.UI.Button fullScreenButton;

    protected float currentTime;
    protected bool isPlaying = false;
    protected bool isPaused = false;
    protected MinigameSO baseGameData;

    protected virtual void Start()
    {
        OnInit(); 

        if (baseGameData != null)
        {
            if (startPanelTitleText != null) 
                startPanelTitleText.text = baseGameData.MinigameName;
            
            if (startPanelRulesText != null) 
                startPanelRulesText.text = baseGameData.TutorialText;
        }

        startPanel.SetActive(true);
        pausePanel.SetActive(false);
        gameOverPanel.SetActive(false);
        victoryPanel.SetActive(false);
    }

    protected virtual void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape) && !startPanel.activeSelf && !gameOverPanel.activeSelf)
        {
            Btn_TogglePause();
        }

        if (!isPlaying || isPaused) return;

        currentTime -= Time.deltaTime;
        if (timerText != null) timerText.text = "Time: " + Mathf.CeilToInt(currentTime);

        if (currentTime <= 0)
        {
            LoseGame();
            return;
        }

        OnUpdate();
    }

    public void Btn_StartGame()
    {
        startPanel.SetActive(false);
        currentTime = GetBaseTimeLimit();
        isPlaying = true;
        OnGameStart(); 
    }

    public void Btn_TogglePause()
    {
        isPaused = !isPaused;
        pausePanel.SetActive(isPaused);
        Time.timeScale = isPaused ? 0f : 1f; 
    }

    public void Btn_Resume()
    {
        if (isPaused) Btn_TogglePause();
    }

    public void Btn_Retry()
    {
        Time.timeScale = 1f;
        startPanel.SetActive(false);
        gameOverPanel.SetActive(false);
        pausePanel.SetActive(false);
        currentTime = GetBaseTimeLimit();
        isPlaying = true;
        isPaused = false;
        OnGameStart(); 
    }

    public void Btn_FinishMinigame()
    {
        Time.timeScale = 1f;
        MinigameManager.Instance.CompleteMinigame(true);
    }

    public void Btn_Quit()
    {
        Time.timeScale = 1f;
        MinigameManager.Instance.CompleteMinigame(false);
    }

    protected void WinGame()
    {
        isPlaying = false;
        victoryPanel.SetActive(true);

        if (fullScreenButton != null) fullScreenButton.interactable = false;
        if (clickToContinueText != null) clickToContinueText.gameObject.SetActive(false);

        if (baseGameData != null && baseGameData.RewardMedal != null)
        {
            if (stickerImage != null) stickerImage.sprite = baseGameData.RewardMedal.MedalIcon;
            if (congratulationText != null) congratulationText.text = "Chúc mừng bạn đã nhận được " + baseGameData.RewardMedal.MedalName + "!";
        }
        else
        {
            if (congratulationText != null) congratulationText.text = "Thử thách hoàn tất!";
        }

        if (stickerImage != null)
        {
            stickerImage.transform.localScale = Vector3.zero;
            
            Sequence winSeq = DOTween.Sequence();
            winSeq.Append(stickerImage.transform.DOScale(Vector3.one * 1.3f, 0.3f).SetEase(Ease.OutQuad));
            winSeq.Append(stickerImage.transform.DOScale(Vector3.one, 0.2f).SetEase(Ease.InQuad));
            winSeq.OnComplete(() =>
            {
                if (clickToContinueText != null) 
                {
                    clickToContinueText.text = "Nhấn vào màn hình để tiếp tục hành trình UIT";
                    clickToContinueText.gameObject.SetActive(true);
                }
                if (fullScreenButton != null) fullScreenButton.interactable = true;
            });
        }
        else
        {
            if (clickToContinueText != null) clickToContinueText.gameObject.SetActive(true);
            if (fullScreenButton != null) fullScreenButton.interactable = true;
        }
    }

    protected void LoseGame()
    {
        isPlaying = false;
        gameOverPanel.SetActive(true);
    }

    protected abstract void OnInit();
    protected abstract void OnGameStart();
    protected virtual void OnUpdate() { }
    protected abstract float GetBaseTimeLimit();
}