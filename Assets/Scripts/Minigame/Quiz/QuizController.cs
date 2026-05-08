using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class QuizController : MinigameController
{
    [Header("Quiz UI")]
    public TextMeshProUGUI questionText;
    public Button[] answerButtons;

    private QuizMinigameSO quizData;
    private int currentQuestionIndex = 0;

    protected override void OnInit()
    {
        if (MinigameManager.Instance == null) return;
        baseGameData = MinigameManager.Instance.CurrentData;
        quizData = baseGameData as QuizMinigameSO;

        for (int i = 0; i < answerButtons.Length; i++)
        {
            int index = i; 
            answerButtons[i].onClick.RemoveAllListeners();
            answerButtons[i].onClick.AddListener(() => OnAnswerSelected(index));
        }
    }

    protected override float GetBaseTimeLimit() => quizData.TimeLimit;

    protected override void OnGameStart()
    {
        currentQuestionIndex = 0;
        LoadQuestion();
    }

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

    private void OnAnswerSelected(int selectedIndex)
    {
        if (!isPlaying || isPaused) return;

        if (selectedIndex == quizData.Questions[currentQuestionIndex].CorrectAnswerIndex)
        {
            currentQuestionIndex++;
            if (currentQuestionIndex >= quizData.Questions.Count) WinGame();
            else LoadQuestion();
        }
        else LoseGame();
    }
}