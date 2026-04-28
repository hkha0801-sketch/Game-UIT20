using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class QuizController : MonoBehaviour
{
    [Header("UI - Main")]
    public TextMeshProUGUI questionText;
    public TextMeshProUGUI timerText;
    public Button[] answerButtons; // Yêu cầu đúng 4 nút

    [Header("UI - Game Over Panel")]
    public GameObject gameOverPanel;

    private QuizMinigameSO quizData;
    private int currentQuestionIndex = 0;
    private float currentTime;
    private bool isPlaying = false;

    void Start()
    {
        InitializeGame();
    }

    void Update()
    {
        if (!isPlaying) return;

        currentTime -= Time.deltaTime;
        timerText.text = "Time: " + Mathf.CeilToInt(currentTime).ToString();

        if (currentTime <= 0)
        {
            LoseGame();
        }
    }

    // Gán dữ liệu từ Manager và bắt đầu
    private void InitializeGame()
    {
        if (MinigameManager.Instance == null || !(MinigameManager.Instance.CurrentData is QuizMinigameSO))
        {
            Debug.LogError("Quiz Data is missing!");
            return;
        }

        quizData = MinigameManager.Instance.CurrentData as QuizMinigameSO;
        
        // Gán sự kiện click cho 4 nút đáp án
        for (int i = 0; i < answerButtons.Length; i++)
        {
            int index = i; 
            answerButtons[i].onClick.RemoveAllListeners();
            answerButtons[i].onClick.AddListener(() => OnAnswerSelected(index));
        }

        StartQuiz();
    }

    // Reset trạng thái và load câu đầu tiên
    private void StartQuiz()
    {
        currentQuestionIndex = 0;
        currentTime = quizData.TimeLimit;
        gameOverPanel.SetActive(false);
        isPlaying = true;

        LoadQuestion();
    }

    // Đổ dữ liệu câu hỏi hiện tại lên UI
    private void LoadQuestion()
    {
        QuizQuestion currentQuestion = quizData.Questions[currentQuestionIndex];
        questionText.text = currentQuestion.QuestionText;

        for (int i = 0; i < answerButtons.Length; i++)
        {
            if (i < currentQuestion.Answers.Length)
            {
                answerButtons[i].gameObject.SetActive(true);
                answerButtons[i].GetComponentInChildren<TextMeshProUGUI>().text = currentQuestion.Answers[i];
            }
            else
            {
                answerButtons[i].gameObject.SetActive(false);
            }
        }
    }

    // Xử lý khi người chơi bấm nút đáp án
    private void OnAnswerSelected(int selectedIndex)
    {
        if (!isPlaying) return;

        int correctIndex = quizData.Questions[currentQuestionIndex].CorrectAnswerIndex;

        if (selectedIndex == correctIndex)
        {
            currentQuestionIndex++;
            
            // Nếu đã trả lời hết câu hỏi
            if (currentQuestionIndex >= quizData.Questions.Count)
            {
                WinGame();
            }
            else
            {
                LoadQuestion();
            }
        }
        else
        {
            LoseGame();
        }
    }

    private void WinGame()
    {
        isPlaying = false;
        MinigameManager.Instance.CompleteMinigame(true);
    }

    private void LoseGame()
    {
        isPlaying = false;
        gameOverPanel.SetActive(true);
    }

    // Nút "Chơi Lại" trên GameOver Panel gọi hàm này
    public void RetryGame()
    {
        StartQuiz();
    }

    // Nút "Thoát" trên GameOver Panel gọi hàm này
    public void QuitGame()
    {
        MinigameManager.Instance.CompleteMinigame(false);
    }
}