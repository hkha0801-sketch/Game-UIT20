// using UnityEngine;
// using TMPro;
// using UnityEngine.UI;
// using System.Collections;
// using DG.Tweening;

// public class QuizController : MinigameController
// {
//     [Header("Quiz UI - Main")]
//     public TextMeshProUGUI questionText;
//     public Button[] answerButtons;

//     [Header("Quiz UI - Health & Effects")]
//     public Image[] heartImages; 
//     public Image damageVignette; 
//     public Color correctColor = Color.green;
//     public Color wrongColor = Color.red;
//     public Color defaultColor = Color.white;

//     private QuizMinigameSO quizData;
//     private int currentQuestionIndex = 0;
//     private int currentLives;
//     private bool isAnswering = false;

//     protected override void OnInit()
//     {
//         if (MinigameManager.Instance == null) return;
//         baseGameData = MinigameManager.Instance.CurrentData;
//         quizData = baseGameData as QuizMinigameSO;

//         for (int i = 0; i < answerButtons.Length; i++)
//         {
//             int index = i; 
//             answerButtons[i].onClick.RemoveAllListeners();
//             answerButtons[i].onClick.AddListener(() => OnAnswerSelected(index));
//         }

//         if (damageVignette != null)
//         {
//             Color c = damageVignette.color;
//             c.a = 0f;
//             damageVignette.color = c;
//             damageVignette.gameObject.SetActive(true);
//         }
//     }

//     protected override float GetBaseTimeLimit() => quizData.TimeLimit;

//     protected override void OnGameStart()
//     {
//         currentQuestionIndex = 0;
//         currentLives = quizData != null ? quizData.MaxLives : 3;
//         isAnswering = false;

//         ResetHeartsUI();
//         LoadQuestion();
//     }

//     private void ResetHeartsUI()
//     {
//         for (int i = 0; i < heartImages.Length; i++)
//         {
//             heartImages[i].gameObject.SetActive(i < currentLives);
//             heartImages[i].transform.localScale = Vector3.one;
//             heartImages[i].transform.DOKill();
//         }
//     }

//     private void LoadQuestion()
//     {
//         QuizQuestion currentQuestion = quizData.Questions[currentQuestionIndex];
//         questionText.text = currentQuestion.QuestionText;

//         for (int i = 0; i < answerButtons.Length; i++)
//         {
//             Image btnImage = answerButtons[i].GetComponent<Image>();
//             if (btnImage != null) btnImage.color = defaultColor;

//             if (i < currentQuestion.Answers.Length)
//             {
//                 answerButtons[i].gameObject.SetActive(true);
//                 answerButtons[i].GetComponentInChildren<TextMeshProUGUI>().text = currentQuestion.Answers[i];
//             }
//             else
//             {
//                 answerButtons[i].gameObject.SetActive(false);
//             }
//         }
//     }

//     private void OnAnswerSelected(int selectedIndex)
//     {
//         if (!isPlaying || isPaused || isAnswering) return;

//         isAnswering = true;
//         bool isCorrect = (selectedIndex == quizData.Questions[currentQuestionIndex].CorrectAnswerIndex);

//         Image btnImage = answerButtons[selectedIndex].GetComponent<Image>();
//         if (btnImage != null)
//         {
//             btnImage.color = isCorrect ? correctColor : wrongColor;
//         }

//         if (!isCorrect)
//         {
//             currentLives--;
//             TriggerDamageEffect();
//         }

//         StartCoroutine(WaitAndLoadNext(isCorrect));
//     }

//     private IEnumerator WaitAndLoadNext(bool isCorrect)
//     {
//         yield return new WaitForSeconds(1f);

//         currentQuestionIndex++;

//         if (currentLives <= 0)
//         {
//             LoseGame();
//         }
//         else if (currentQuestionIndex >= quizData.Questions.Count)
//         {
//             WinGame();
//         }
//         else
//         {
//             LoadQuestion();
//         }

//         isAnswering = false;
//     }

//     private void TriggerDamageEffect()
//     {
//         if (damageVignette != null)
//         {
//             damageVignette.DOKill();
//             damageVignette.color = new Color(wrongColor.r, wrongColor.g, wrongColor.b, 0.5f);
//             damageVignette.DOFade(0f, 0.5f).SetEase(Ease.OutQuad);
//         }

//         if (currentLives >= 0 && currentLives < heartImages.Length)
//         {
//             Image heartToLose = heartImages[currentLives];
            
//             heartToLose.transform.DOKill();
//             Sequence seq = DOTween.Sequence();
            
//             seq.Append(heartToLose.transform.DOShakePosition(0.5f, strength: new Vector3(15f, 0f, 0f), vibrato: 15));
//             seq.Append(heartToLose.transform.DOScale(Vector3.zero, 0.3f).SetEase(Ease.InBack));
//             seq.OnComplete(() => heartToLose.gameObject.SetActive(false));
//         }
//     }
// }

using UnityEngine;
using TMPro;
using UnityEngine.UI;
using System.Collections;
using DG.Tweening;

public class QuizController : MinigameController
{
    [Header(" Quiz UI - Main")]
    public TextMeshProUGUI questionText;
    public Button[] answerButtons;

    [Header("Quiz UI - Health & Effects")]
    public Image[] heartImages; 
    public Image damageVignette; 
    public Color correctColor = Color.green;
    public Color wrongColor = Color.red;
    public Color defaultColor = Color.white;

    private QuizMinigameSO quizData;
    private int currentQuestionIndex = 0;
    private int currentLives;
    private bool isAnswering = false;

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

        if (damageVignette != null)
        {
            Color c = damageVignette.color;
            c.a = 0f;
            damageVignette.color = c;
            damageVignette.gameObject.SetActive(true);
        }
    }

    protected override float GetBaseTimeLimit() => quizData.TimeLimit;

    protected override void OnGameStart()
    {
        currentQuestionIndex = 0;
        currentLives = quizData != null ? quizData.MaxLives : 3;
        isAnswering = false;

        ResetHeartsUI();
        LoadQuestion();
    }

    private void ResetHeartsUI()
    {
        for (int i = 0; i < heartImages.Length; i++)
        {
            heartImages[i].gameObject.SetActive(i < currentLives);
            heartImages[i].transform.localScale = Vector3.one;
            heartImages[i].transform.DOKill();
        }
    }

    private void LoadQuestion()
    {
        QuizQuestion currentQuestion = quizData.Questions[currentQuestionIndex];
        questionText.text = currentQuestion.QuestionText;

        for (int i = 0; i < answerButtons.Length; i++)
        {
            answerButtons[i].transform.localScale = Vector3.one;
            answerButtons[i].transform.DOKill();

            Image btnImage = answerButtons[i].GetComponent<Image>();
            if (btnImage != null) btnImage.color = defaultColor;

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
        if (!isPlaying || isPaused || isAnswering) return;

        isAnswering = true;
        bool isCorrect = (selectedIndex == quizData.Questions[currentQuestionIndex].CorrectAnswerIndex);

        GameObject btnObj = answerButtons[selectedIndex].gameObject;
        btnObj.transform.DOKill(true);
        btnObj.transform.DOPunchScale(new Vector3(0.1f, 0.1f, 0f), 0.2f, 10, 1f);

        Image btnImage = answerButtons[selectedIndex].GetComponent<Image>();
        if (btnImage != null)
        {
            btnImage.color = isCorrect ? correctColor : wrongColor;
        }

        if (!isCorrect)
        {
            currentLives--;
            TriggerDamageEffect();
        }

        StartCoroutine(WaitAndLoadNext(isCorrect));
    }

    private IEnumerator WaitAndLoadNext(bool isCorrect)
    {
        yield return new WaitForSeconds(1f);

        currentQuestionIndex++;

        if (currentLives <= 0)
        {
            LoseGame();
        }
        else if (currentQuestionIndex >= quizData.Questions.Count)
        {
            WinGame();
        }
        else
        {
            LoadQuestion();
        }

        isAnswering = false;
    }

    private void TriggerDamageEffect()
    {
        if (damageVignette != null)
        {
            damageVignette.DOKill();
            damageVignette.color = new Color(wrongColor.r, wrongColor.g, wrongColor.b, 0.8f);
            Sequence vSeq = DOTween.Sequence();
            vSeq.AppendInterval(0.1f);
            vSeq.Append(damageVignette.DOFade(0f, 1.2f).SetEase(Ease.InSine));
        }

        if (currentLives >= 0 && currentLives < heartImages.Length)
        {
            Image heartToLose = heartImages[currentLives];
            
            heartToLose.transform.DOKill();
            Sequence seq = DOTween.Sequence();
            
            seq.Append(heartToLose.transform.DOShakePosition(0.5f, strength: new Vector3(15f, 0f, 0f), vibrato: 15));
            seq.Append(heartToLose.transform.DOScale(Vector3.zero, 0.3f).SetEase(Ease.InBack));
            seq.OnComplete(() => heartToLose.gameObject.SetActive(false));
        }
    }
}