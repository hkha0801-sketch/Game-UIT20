using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public struct PunchLevelConfig
{
    public float ForceThreshold;
    public float DecayRatePerSecond;
    public float ForceAddedPerClick;
    public Sprite LevelSprite;  
}

[CreateAssetMenu(fileName = "PunchGameSO", menuName = "UITGAME/Minigame/Punch Game")]
public class PunchMinigameSO : MinigameSO
{
    [Header("Game Settings")]
    public float TimeLimit = 30f;
    public List<PunchLevelConfig> LevelConfigs;
}