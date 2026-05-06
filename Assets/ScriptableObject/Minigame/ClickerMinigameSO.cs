using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public struct ClickLevelConfig
{
    public float ForceThreshold;
    public float DecayRatePerSecond;
    public float ForceAddedPerClick;
    public int CharsRevealedPerClick; 
}[CreateAssetMenu(fileName = "ClickerGameSO", menuName = "UITGAME/Minigame/Clicker Game")]
public class ClickerMinigameSO : MinigameSO
{[Header("Game Content")]
    [TextArea(10, 20)] 
    public string TargetText;     
    public float TimeLimit = 30f;
    public List<ClickLevelConfig> LevelConfigs;
}