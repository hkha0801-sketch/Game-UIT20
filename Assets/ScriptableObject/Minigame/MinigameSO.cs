using UnityEngine;

[CreateAssetMenu(fileName = "_MinigameSO", menuName = "UITGAME/Minigame/Base minigame", order = 0)]
public class MinigameSO : ScriptableObject
{
    [Header("Minigame Info")]
    public string MinigameID;
    public string MinigameName;
    public string SceneName;

    [TextArea(3, 10)] 
    public string TutorialText;

    [Header("Reward")]
    public MedalSO RewardMedal;
}