using UnityEngine;
using TMPro;

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