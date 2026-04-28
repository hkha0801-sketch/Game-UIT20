// using System.Collections.Generic;
// using UnityEngine;
// using UnityEngine.SceneManagement;
// using DialogueEditor;

// public class MinigameManager : MonoBehaviour
// {
//     public static MinigameManager Instance;

//     [Header("Kho lưu kết quả")]
//     private Dictionary<string, int> minigameResults = new Dictionary<string, int>();
    
//     [Header("Dữ liệu tạm thời (Đi & Về)")]
//     public MinigameSO CurrentData;
//     private string mapSceneName;
//     private Vector3 playerReturnPosition;

//     private void Awake()
//     {
//         if (Instance == null)
//         {
//             Instance = this;
//             DontDestroyOnLoad(gameObject);
//             SceneManager.sceneLoaded += OnSceneLoaded;
//         }
//         else Destroy(gameObject);
//     }

//     private void OnDestroy()
//     {
//         if (Instance == this) SceneManager.sceneLoaded -= OnSceneLoaded;
//     }

//     public void StartMinigame(MinigameSO data)
//     {
//         CurrentData = data;
//         mapSceneName = SceneManager.GetActiveScene().name;
        
//         GameObject player = GameObject.FindGameObjectWithTag("Player");
//         if (player != null) playerReturnPosition = player.transform.position;

//         if (ConversationManager.Instance.IsConversationActive)
//             ConversationManager.Instance.EndConversation();

//         SceneManager.LoadScene(data.SceneName);
//     }

//     public void CompleteMinigame(bool isWin)
//     {
//         if (CurrentData == null) return;

//         int result = isWin ? 1 : -1;
//         minigameResults[CurrentData.MinigameID] = result;
//         Debug.Log($"Kết quả game {CurrentData.MinigameID}: {(isWin ? "THẮNG" : "THUA")}");

//         SceneManager.LoadScene(mapSceneName);
//     }

//     public void SyncMinigameResultsToDialogue()
//     {
//         foreach (var item in minigameResults)
//         {
//             string flagName = "minigame_" + item.Key;
//             ConversationManager.Instance.SetInt(flagName, item.Value);
//         }
//     }

//     private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
//     {
//         if (scene.name == mapSceneName && CurrentData != null)
//         {
//             GameObject player = GameObject.FindGameObjectWithTag("Player");
//             if (player != null)
//             {
//                 player.transform.position = playerReturnPosition;
//             }
//             CurrentData = null;
//         }
//     }
// }

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

        SceneManager.LoadScene(mapSceneName);
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
}