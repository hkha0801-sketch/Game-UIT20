using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneController : MonoBehaviour
{
    public static SceneController Instance;

    private string targetSpawnPointID;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
            SceneManager.sceneLoaded += OnSceneLoaded;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void ChangeScene(string sceneName, string spawnPointID = "")
    {
        targetSpawnPointID = spawnPointID;
        SceneTransition.Instance.PlayTransition(() => {SceneManager.LoadScene(sceneName);});
    }

    public void PlayGame()
    {
        SceneTransition.Instance.PlayTransition(() => {SceneManager.LoadScene("Map B");});
    }

    public void QuitGame()
    {
        Debug.Log("Quit game");
        SceneTransition.Instance.PlayTransition(() => {Application.Quit();});
    } 

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        if (string.IsNullOrEmpty(targetSpawnPointID)) return;

        GameObject player = GameObject.FindGameObjectWithTag("Player");
        if (player == null) return;

        SpawnPoint[] allPoints = Object.FindObjectsByType<SpawnPoint>(FindObjectsSortMode.None);

        foreach (SpawnPoint sp in allPoints)
        {
            if (sp.pointID == targetSpawnPointID)
            {
                // 1. Dịch chuyển vị trí
                player.transform.position = sp.transform.position;

                // 2. Cập nhật hướng quay mặt
                PlayerMovement movement = player.GetComponent<PlayerMovement>();
                if (movement != null)
                {
                    movement.SetFacingDirection(sp.facingDirection);
                }
                break;
            }
        }
        targetSpawnPointID = "";
    }

}