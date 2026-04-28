using UnityEngine;

[CreateAssetMenu(fileName = "_MinigameSO", menuName = "UITGAME/Minigame/Base minigame", order = 0)]
public class MinigameSO : ScriptableObject
{
    [Header("Minigame Infor")]
    public string MinigameID;
    public string MinigameName;
    public string SceneName;
}