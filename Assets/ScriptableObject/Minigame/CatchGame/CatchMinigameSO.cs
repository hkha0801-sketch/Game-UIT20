using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public struct NormalCatchItem
{
    public Sprite itemSprite;
    public int score;
    public float fallSpeed;
    public int spawnWeight;
}

[System.Serializable]
public struct EffectCatchItem
{
    public Sprite itemSprite;
    public CatchEffectSO effectSO;
    public float fallSpeed;
    public int spawnWeight;

}[CreateAssetMenu(fileName = "CatchMinigameSO", menuName = "UITGAME/Minigame/Catch Game")]
public class CatchMinigameSO : MinigameSO
{
    public int targetScore = 100;
    public float timeLimit = 60f;
    public float spawnInterval = 1f;

    [Header("Spawn Amount")]
    [Min(1)] public int minItemsPerSpawn = 1;
    [Min(1)] public int maxItemsPerSpawn = 1;

    public List<NormalCatchItem> normalItems;
    public List<EffectCatchItem> effectItems;
}