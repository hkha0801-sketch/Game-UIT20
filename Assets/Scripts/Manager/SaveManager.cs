using UnityEngine;
using UnityEngine.SceneManagement;

public class SaveManager : MonoBehaviour
{
    public static SaveManager Instance;
    private const string SAVE_KEY = "GameSaveData";
    private SaveData targetLoadData;

    private bool turnOnAutoSave = false;
    private float autoSaveTimer = 0f;
    private float autoSaveInterval = 60f;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
            SceneManager.sceneLoaded += OnSceneLoadedAfterSave;
            SceneManager.sceneLoaded += OnSceneLoadedGeneral; 
        }
        else Destroy(gameObject);
    }

    private void Update()
    {
        if (!turnOnAutoSave) return;

        autoSaveTimer += Time.deltaTime;
        if (autoSaveTimer >= autoSaveInterval)
        {
            SaveGame();
            autoSaveTimer = 0f;
        }
    }

    private void OnDestroy()
    {
        if (Instance == this) SceneManager.sceneLoaded -= OnSceneLoadedAfterSave;
    }

     private void OnSceneLoadedGeneral(Scene scene, LoadSceneMode mode)
    {
        if (scene.name.ToLower() == "login" || scene.name.ToLower().Contains("minigame")) return;
        turnOnAutoSave = true;
        SaveGame();
    }

    public void SaveGame()
    {
        if (SceneManager.GetActiveScene().name.ToLower() == "login") return;

        SaveData data = new SaveData();

        data.OwnedMedalIDs = MedalManager.Instance.GetOwnedMedalIDs();
        data.MetNPCIDs = NPCManager.Instance.GetMetNPCs();
        MinigameManager.Instance.GetMinigameDataForSave(out data.MinigameIDs, out data.MinigameResults);

        bool isInMinigame = SceneManager.GetActiveScene().name.ToLower().Contains("minigame");
        
        if (isInMinigame)
        {
            data.IsSavedInMinigame = true;
            data.SavedSceneName = MinigameManager.Instance.GetMapSceneName();
            data.ReturnPosition = MinigameManager.Instance.GetPlayerReturnPosition();
            data.ReturnDirection = MinigameManager.Instance.GetPlayerReturnDirection();
        }
        else
        {
            data.IsSavedInMinigame = false;
            data.SavedSceneName = SceneManager.GetActiveScene().name;
            GameObject player = GameObject.FindGameObjectWithTag("Player");
            if (player != null)
            {
                data.ReturnPosition = player.transform.position;
                data.ReturnDirection = player.GetComponent<PlayerMovement>().direction;
            }
        }

        string json = JsonUtility.ToJson(data);
        PlayerPrefs.SetString(SAVE_KEY, json);
        PlayerPrefs.Save();
    }

    public bool HasSaveData()
    {
        return PlayerPrefs.HasKey(SAVE_KEY);
    }

    public void LoadData()
    {
        if (!HasSaveData()) return;

        string json = PlayerPrefs.GetString(SAVE_KEY);
        SaveData data = JsonUtility.FromJson<SaveData>(json);

        if (MedalManager.Instance != null) MedalManager.Instance.LoadOwnedMedals(data.OwnedMedalIDs);
        if (NPCManager.Instance != null) NPCManager.Instance.LoadMetNPCs(data.MetNPCIDs);
        if (MinigameManager.Instance != null) MinigameManager.Instance.LoadMinigameData(data.MinigameIDs, data.MinigameResults);

        targetLoadData = data;
    }

    public void ContinueSavedGame()
    {
        if (targetLoadData != null && !string.IsNullOrEmpty(targetLoadData.SavedSceneName) && targetLoadData.SavedSceneName.ToLower() != "login")
        {
            SceneController.Instance.ChangeScene(targetLoadData.SavedSceneName);
        }
        else
        {
            SceneController.Instance.PlayGame();
        }
    }

    private void OnSceneLoadedAfterSave(Scene scene, LoadSceneMode mode)
    {
        turnOnAutoSave = (scene.name != "Login");

        if (targetLoadData != null && scene.name == targetLoadData.SavedSceneName)
        {
            GameObject player = GameObject.FindGameObjectWithTag("Player");
            if (player != null)
            {
                player.transform.position = targetLoadData.ReturnPosition;
                PlayerMovement pMove = player.GetComponent<PlayerMovement>();
                if (pMove != null) pMove.SetFacingDirection(targetLoadData.ReturnDirection);
            }
            targetLoadData = null;
        }
    }

    private void OnApplicationQuit()
    {
        SaveGame();
    }

    private void OnApplicationFocus(bool hasFocus)
    {
        if (!hasFocus) SaveGame();
    }

    [ContextMenu("Delete Save Data")]
    public void DeleteSave() { PlayerPrefs.DeleteKey(SAVE_KEY); Debug.Log("Save Deleted"); }
}