using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public struct CardPair
{
    public Sprite imageA;
    public Sprite imageB;

}
[CreateAssetMenu(fileName = "MemoryGameSO", menuName = "UITGAME/Minigame/Memory Game")]
public class MemoryGameSO : MinigameSO
{
    [Header("Memory Settings")]
    public float TimeLimit = 60f;
    public int Rows = 3;
    public int Columns = 4;
    public List<CardPair> CardPairs;
}