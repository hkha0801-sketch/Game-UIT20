using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using DialogueEditor;
using System.Collections;

public class MinigameManager : MonoBehaviour
{
    public static MinigameManager Instance;

    private Dictionary<string, int> minigameResults = new Dictionary<string, int>();
    
    public MinigameSO CurrentData;
    private string mapSceneName;
    private Vector3 playerReturnPosition;
    private Vector2 playerReturnDirection;
    private string lastNPCID;
    private bool shouldAutoTrigger;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
            SceneManager.sceneLoaded += OnSceneLoaded;
        }
        else Destroy(gameObject);
    }

    private void OnDestroy()
    {
        if (Instance == this) SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    public void StartMinigame(MinigameSO data, string npcID)
    {
        CurrentData = data;
        lastNPCID = npcID;
        mapSceneName = SceneManager.GetActiveScene().name;
        
        GameObject player = GameObject.FindGameObjectWithTag("Player");
        if (player != null)
        {
            playerReturnPosition = player.transform.position;
            playerReturnDirection = player.GetComponent<PlayerMovement>().direction;
        }

        if (ConversationManager.Instance.IsConversationActive)
            ConversationManager.Instance.EndConversation();

        SceneManager.LoadScene(data.SceneName);
    }

    public void CompleteMinigame(bool isWin)
    {
        if (CurrentData == null) return;

        minigameResults[CurrentData.MinigameID] = isWin ? 1 : -1;
        shouldAutoTrigger = isWin; 

        if (SaveManager.Instance != null) SaveManager.Instance.SaveGame();

        SceneController.Instance.ChangeScene(mapSceneName);
    }

    public void SyncMinigameResultsToDialogue()
    {
        foreach (var item in minigameResults)
        {
            ConversationManager.Instance.SetInt("minigame_" + item.Key, item.Value);
        }
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        if (scene.name == mapSceneName && !string.IsNullOrEmpty(lastNPCID))
        {
            GameObject player = GameObject.FindGameObjectWithTag("Player");
            if (player != null)
            {
                player.transform.position = playerReturnPosition;
                player.GetComponent<PlayerMovement>().SetFacingDirection(playerReturnDirection);
            }

            if (shouldAutoTrigger)
            {
                shouldAutoTrigger = false;
                StartCoroutine(TriggerNPCLater());
            }
        }
    }

    private IEnumerator TriggerNPCLater()
    {
        yield return new WaitForSeconds(0.1f);
        NPCController[] npcs = FindObjectsByType<NPCController>(FindObjectsSortMode.None);
        foreach (NPCController npc in npcs)
        {
            if (npc.npcData.NPCID == lastNPCID)
            {
                npc.Interact();
                break;
            }
        }
    }

    public void GetMinigameDataForSave(out System.Collections.Generic.List<string> keys, out System.Collections.Generic.List<int> values)
    {
        keys = new System.Collections.Generic.List<string>();
        values = new System.Collections.Generic.List<int>();
        foreach (var kvp in minigameResults)
        {
            keys.Add(kvp.Key);
            values.Add(kvp.Value);
        }
    }

    public void LoadMinigameData(System.Collections.Generic.List<string> keys, System.Collections.Generic.List<int> values)
    {
        minigameResults.Clear();
        for (int i = 0; i < keys.Count; i++)
        {
            minigameResults.Add(keys[i], values[i]);
        }
    }

    public void ClearMinigameData()
    {
        minigameResults.Clear();
        
        CurrentData = null;
        mapSceneName = "";
        playerReturnPosition = Vector3.zero;
        playerReturnDirection = Vector2.up;
        lastNPCID = "";
        shouldAutoTrigger = false;
    }

    public string GetMapSceneName() => mapSceneName;
    public Vector3 GetPlayerReturnPosition() => playerReturnPosition;
    public Vector2 GetPlayerReturnDirection() => playerReturnDirection;
}