using UnityEngine;
using TMPro;
using DG.Tweening;

public class CatchGameController : MinigameController
{
    [Header("Catch Game UI")]
    public TextMeshProUGUI scoreText;
    public TextMeshProUGUI targetScoreText;
    public TextMeshProUGUI effectNotifyText; 

    [Header("Spawning & Prefabs")]
    public CatchItemObj itemPrefab;
    public FloatingTextObj floatingTextPrefab; 
    public Transform spawnPointMin;
    public Transform spawnPointMax;
    public CatchPlayer player;

    private CatchMinigameSO gameData;
    private int currentScore;
    private float spawnTimer;
    private Sequence effectTextSeq;

    [Header("Catch_SFX")]
    public SoundFeedback correctSound;
    public SoundFeedback wrongSound;
    public SoundFeedback effectSound;

    protected override void OnInit()
    {
        if (MinigameManager.Instance != null)
        {
            baseGameData = MinigameManager.Instance.CurrentData;
            gameData = baseGameData as CatchMinigameSO;
        }
        effectNotifyText.gameObject.SetActive(false);
    }

    protected override float GetBaseTimeLimit() => gameData != null ? gameData.timeLimit : 60f;

    protected override void OnGameStart()
    {
        currentScore = 0;
        UpdateScoreUI();
        targetScoreText.text = "Target: " + gameData.targetScore;
        spawnTimer = gameData.spawnInterval;
        player.ResetAllEffects();
        
        if (effectTextSeq != null) effectTextSeq.Kill();
        effectNotifyText.gameObject.SetActive(false);
    }

    protected override void OnUpdate()
    {
        spawnTimer -= Time.deltaTime;
        if (spawnTimer <= 0)
        {
            int spawnCount = Random.Range(gameData.minItemsPerSpawn, gameData.maxItemsPerSpawn + 1);

            for (int i = 0; i < spawnCount; i++)
            {
                SpawnRandomItem();
            }

            spawnTimer = gameData.spawnInterval;
        }
    }

    private void SpawnRandomItem()
    {
        if (gameData == null)
        {
            Debug.LogError("Lỗi: gameData (SO) bị null. Có thể bạn đã kéo nhầm loại SO vào NPC!");
            return;
        }

        if (spawnPointMin == null || spawnPointMax == null)
        {
            Debug.LogError("Lỗi: Chưa kéo SpawnPointMin/Max vào GameController!");
            return;
        }

        if (itemPrefab == null)
        {
            Debug.LogError("Lỗi: Chưa kéo ItemPrefab vào GameController!");
            return;
        }

        int totalWeight = 0;
        foreach (var item in gameData.normalItems) totalWeight += item.spawnWeight;
        foreach (var item in gameData.effectItems) totalWeight += item.spawnWeight;

        if (totalWeight <= 0)
        {
            Debug.LogWarning("Cảnh báo: Tổng trọng số (Weight) bằng 0. Hãy kiểm tra các mốc Weight trong file SO.");
            return;
        }

        int randomValue = Random.Range(0, totalWeight);
        int currentWeight = 0;

        float spawnX = Random.Range(spawnPointMin.position.x, spawnPointMax.position.x);
        Vector3 spawnPos = new Vector3(spawnX, spawnPointMin.position.y, 0);

        CatchItemObj spawnedItem = Instantiate(itemPrefab, spawnPos, Quaternion.identity);

        foreach (var item in gameData.normalItems)
        {
            currentWeight += item.spawnWeight;
            if (randomValue < currentWeight)
            {
                spawnedItem.SetupNormal(item.itemSprite, item.fallSpeed, item.score, this);
                return;
            }
        }

        foreach (var item in gameData.effectItems)
        {
            currentWeight += item.spawnWeight;
            if (randomValue < currentWeight)
            {
                spawnedItem.SetupEffect(item.itemSprite, item.fallSpeed, item.effectSO, this);
                return;
            }
        }
    }

    public void AddScore(int amount, Vector3 catchPos)
    {
        if (!isPlaying || isPaused) return;

        if (amount > 0)
        {
            if (correctSound != null) correctSound.PlaySound();
        }
        else
        {
            if (wrongSound != null) wrongSound.PlaySound();
        }

        currentScore += amount;
        UpdateScoreUI();

        GameObject obj = ObjectPoolManager.Instance.Spawn(floatingTextPrefab.gameObject, catchPos, Quaternion.identity);
        FloatingTextObj floater = obj.GetComponent<FloatingTextObj>();

        string prefix = amount > 0 ? "+" : "";
        Color color = amount > 0 ? Color.green : Color.red;
        floater.Setup(prefix + amount.ToString(), color);

        if (currentScore >= gameData.targetScore) WinGame();
    }

    public void ShowEffectText(string desc, Color color)
    {
        
        if (effectSound != null) effectSound.PlaySound();

        if (effectTextSeq != null && effectTextSeq.IsActive()) effectTextSeq.Kill();

        effectNotifyText.gameObject.SetActive(true);
        effectNotifyText.text = desc;
        effectNotifyText.color = color;
        effectNotifyText.transform.localScale = Vector3.zero;

        effectTextSeq = DOTween.Sequence();
        effectTextSeq.Append(effectNotifyText.transform.DOScale(Vector3.one, 0.3f).SetEase(Ease.OutBack));
        effectTextSeq.AppendInterval(2.5f); 
        effectTextSeq.Append(effectNotifyText.transform.DOScale(Vector3.zero, 0.2f).SetEase(Ease.InBack));
        effectTextSeq.OnComplete(() => effectNotifyText.gameObject.SetActive(false));
    }

    private void UpdateScoreUI()
    {
        scoreText.text = "Score: " + currentScore;
    }
}