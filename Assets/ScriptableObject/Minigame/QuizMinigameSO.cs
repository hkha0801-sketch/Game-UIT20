using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public struct QuizQuestion
{
    [TextArea] public string QuestionText;
    public string[] Answers;
    public int CorrectAnswerIndex;
}

[CreateAssetMenu(fileName = "_QuizzMinigameSO", menuName = "UITGAME/Minigame/Quiz", order = 1)]
public class QuizMinigameSO : MinigameSO
{
    [Header("Quiz Infor")]
    public float TimeLimit = 60f;
    public List<QuizQuestion> Questions;
}