using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MemoryGameController : MinigameController
{
    [Header("Memory UI")]
    public Transform gridParent;
    public MemoryCard cardPrefab;

    private MemoryGameSO gameData;
    private int pairsMatched = 0;
    private int totalPairs;

    private MemoryCard firstCard;
    private MemoryCard secondCard;
    private bool isChecking = false;

    [Header("MemoryCard_SFX")]
    public SoundFeedback correctSound;
    public SoundFeedback selectSound;

    protected override void OnInit()
    {
        if (MinigameManager.Instance != null)
        {
            baseGameData = MinigameManager.Instance.CurrentData;
            gameData = baseGameData as MemoryGameSO;
        }
    }

    protected override float GetBaseTimeLimit() => gameData != null ? gameData.TimeLimit : 60f;

    protected override void OnGameStart()
    {
        StopAllCoroutines();
        isChecking = false;
        firstCard = null;
        secondCard = null;
        pairsMatched = 0;

        foreach (Transform child in gridParent)
        {
            Destroy(child.gameObject);
        }

        if (gameData != null)
        {
            int requiredPairs = (gameData.Rows * gameData.Columns) / 2;
            totalPairs = Mathf.Min(requiredPairs, gameData.CardPairs.Count);
            
            SetupGrid();
            SpawnAndShuffleCards();
        }
    }

    private void SetupGrid()
    {
        GridLayoutGroup grid = gridParent.GetComponent<GridLayoutGroup>();
        if (grid != null)
        {
            grid.constraint = GridLayoutGroup.Constraint.FixedColumnCount;
            grid.constraintCount = gameData.Columns;
        }
    }

    private void SpawnAndShuffleCards()
    {
        List<MemoryCard> cards = new List<MemoryCard>();

        for (int i = 0; i < totalPairs; i++)
        {
            CardPair pair = gameData.CardPairs[i];

            MemoryCard cardA = Instantiate(cardPrefab, gridParent);
            cardA.SetupCard(i, pair.imageA, gameData.CardFrontBgImage, gameData.CardBackImage, this);
            cards.Add(cardA);

            MemoryCard cardB = Instantiate(cardPrefab, gridParent);
            cardB.SetupCard(i, pair.imageB, gameData.CardFrontBgImage, gameData.CardBackImage, this);
            cards.Add(cardB);
        }

        for (int i = 0; i < cards.Count; i++)
        {
            int randomIndex = Random.Range(i, cards.Count);
            cards[randomIndex].transform.SetSiblingIndex(i);
        }
    }

    public void CardClicked(MemoryCard card)
    {
        if (isChecking || !isPlaying || isPaused || card == firstCard) return;

        if (selectSound != null) selectSound.PlaySound();

        card.Reveal();

        if (firstCard == null) firstCard = card;
        else if (secondCard == null)
        {
            secondCard = card;
            isChecking = true;
            StartCoroutine(CheckMatchRoutine());
        }
    }

    private IEnumerator CheckMatchRoutine()
    {
        if (firstCard.pairID == secondCard.pairID)
        {
            if (correctSound != null) correctSound.PlaySound();
            yield return new WaitForSeconds(0.5f);
            firstCard.SetMatched();
            secondCard.SetMatched();
            pairsMatched++;

            if (pairsMatched >= totalPairs) WinGame();
        }
        else
        {
            yield return new WaitForSeconds(1f);
            firstCard.Cover();
            secondCard.Cover();
        }

        firstCard = null;
        secondCard = null;
        isChecking = false;
    }
}