using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class MemoryGameController : MonoBehaviour
{
    [Header("UI - Main")]
    public TextMeshProUGUI timerText;
    public Transform gridParent;
    public MemoryCard cardPrefab;

    [Header("UI - Game Over Panel")]
    public GameObject gameOverPanel;

    private MemoryGameSO gameData;
    private float currentTime;
    private bool isPlaying = false;
    private int pairsMatched = 0;
    private int totalPairs;

    private MemoryCard firstCard;
    private MemoryCard secondCard;
    private bool isChecking = false;

    void Start()
    {
        InitializeGame();
    }

    void Update()
    {
        if (!isPlaying) return;

        currentTime -= Time.deltaTime;
        timerText.text = "Time: " + Mathf.CeilToInt(currentTime).ToString();

        if (currentTime <= 0)
        {
            LoseGame();
        }
    }

    private void InitializeGame()
    {
        if (MinigameManager.Instance == null || !(MinigameManager.Instance.CurrentData is MemoryGameSO))
        {
            Debug.LogError("Memory Data is missing!");
            return;
        }

        gameData = MinigameManager.Instance.CurrentData as MemoryGameSO;
        
        SetupGrid();
        StartMemoryGame(); // Gọi không tham số để khớp với các hàm Retry
    }

    private void StartMemoryGame()
    {
        StopAllCoroutines();
        isChecking = false;
        firstCard = null;
        secondCard = null;

        // Xóa bài cũ
        foreach (Transform child in gridParent)
        {
            Destroy(child.gameObject);
        }

        // TÍNH TOÁN SỐ CẶP AN TOÀN ĐỂ TRÁNH LỖI INDEX OUT OF RANGE
        int requiredPairs = (gameData.Rows * gameData.Columns) / 2;
        int availablePairs = gameData.CardPairs.Count;
        totalPairs = Mathf.Min(requiredPairs, availablePairs);

        SpawnAndShuffleCards();

        currentTime = gameData.TimeLimit;
        pairsMatched = 0;
        gameOverPanel.SetActive(false);
        isPlaying = true;
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
            cardA.SetupCard(i, pair.imageA, this);
            cards.Add(cardA);

            MemoryCard cardB = Instantiate(cardPrefab, gridParent);
            cardB.SetupCard(i, pair.imageB, this);
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
        if (isChecking || !isPlaying || card == firstCard) return;

        card.Reveal();

        if (firstCard == null)
        {
            firstCard = card;
        }
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
            yield return new WaitForSeconds(0.5f);
            firstCard.SetMatched();
            secondCard.SetMatched();
            pairsMatched++;

            if (pairsMatched >= totalPairs)
            {
                WinGame();
            }
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

    private void WinGame()
    {
        isPlaying = false;
        MinigameManager.Instance.CompleteMinigame(true);
    }

    private void LoseGame()
    {
        isPlaying = false;
        gameOverPanel.SetActive(true);
    }

    public void RetryGame()
    {
        StartMemoryGame();
    }

    public void QuitGame()
    {
        MinigameManager.Instance.CompleteMinigame(false);
    }
}