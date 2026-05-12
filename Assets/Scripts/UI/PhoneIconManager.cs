using UnityEngine;
using UnityEngine.SceneManagement;

public class PhoneIconManager : MonoBehaviour
{
    public GameObject phoneIconObject;

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
        string nameLower = sceneName.ToLower();
        bool isMinigame = nameLower.Contains("minigame");
        bool isEnding = nameLower == "endingscene";

        bool isForbidden = isMinigame || isEnding;

        if (phoneIconObject != null)
        {
            phoneIconObject.SetActive(!isForbidden);
        }

        if (isForbidden && SmartphoneController.Instance != null && SmartphoneController.Instance.IsPhoneActive)
        {
            SmartphoneController.Instance.TurnOffPhone();
        }

    }

    void Update()
    {
        string nameLower = SceneManager.GetActiveScene().name.ToLower();
        bool isForbidden = nameLower.Contains("minigame") || nameLower == "endingscene";

        if (!isForbidden && Input.GetKeyDown(KeyCode.Tab))
        {
            if (SmartphoneController.Instance != null)
            {
                SmartphoneController.Instance.TogglePhone();
            }
        }
    }
}