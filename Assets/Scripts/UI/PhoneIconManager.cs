using UnityEngine;
using UnityEngine.SceneManagement;

public class PhoneIconManager : MonoBehaviour
{
    public GameObject phoneIconObject;

    private bool isMinigameScene = false;

    private void OnEnable()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    private void OnDisable()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    private void Start()
    {
        CheckScene(SceneManager.GetActiveScene().name);
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        CheckScene(scene.name);
    }

    private void CheckScene(string sceneName)
    {
        isMinigameScene = sceneName.ToLower().Contains("minigame");

        if (phoneIconObject != null)
        {
            phoneIconObject.SetActive(!isMinigameScene);
        }

        if (isMinigameScene && SmartphoneController.Instance != null && SmartphoneController.Instance.IsPhoneActive)
        {
            SmartphoneController.Instance.TurnOffPhone();
        }

    }

    void Update()
    {
        if (!isMinigameScene && Input.GetKeyDown(KeyCode.Tab))
        {
            if (SmartphoneController.Instance != null)
            {
                SmartphoneController.Instance.TogglePhone();
            }
        }
    }
}