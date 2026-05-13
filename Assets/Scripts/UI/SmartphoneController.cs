using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class SmartphoneController : MonoBehaviour
{
    public static SmartphoneController Instance;

    private enum ConfirmType { None, QuitGame, GoToMenu, EndGame }

    [Header("Main UI")]
    public GameObject phoneContainer;
    public GameObject homeScreen;
    public GameObject medalAppPanel;

    [Header("Popup UI")]
    public GameObject confirmPopupPanel;
    public TextMeshProUGUI confirmText;

    private ConfirmType currentConfirmType = ConfirmType.None;

    [Header("Alls App")]
    public List<GameObject> objectsToHideWhenOpen;

    [Header("Music UI")]
    public TextMeshProUGUI musicNameText;
    public SoundFeedback clickSound;

    public bool IsPhoneActive => phoneContainer.activeSelf;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    void Start()
    {
        GoHome();
        homeScreen.SetActive(false);

        UpdateMusicName();
    }

    public void TurnOffPhone()
    {
        if (clickSound != null) clickSound.PlaySound();
        
        phoneContainer.SetActive(false);
        ToggleHiddenObjects(false);
    }

    public void GoHome()
    {
        if (clickSound != null) clickSound.PlaySound();

        ToggleHiddenObjects(false);
        homeScreen.SetActive(true);
    }

    public void OpenApp(GameObject appPanel)
    {
        if (clickSound != null) clickSound.PlaySound();

        homeScreen.SetActive(false);
        appPanel.SetActive(true);
    }

    public void ShowConfirmQuitGame()
    {
        if (clickSound != null) clickSound.PlaySound();

        currentConfirmType = ConfirmType.QuitGame;
        confirmText.text = "Bạn có chắc chắn muốn thoát game?";
        confirmPopupPanel.SetActive(true);
    }

    public void ShowConfirmGoToMenu()
    {
        if (clickSound != null) clickSound.PlaySound();

        currentConfirmType = ConfirmType.GoToMenu;
        confirmText.text = "Bạn có muốn quay lại Menu chính?";
        confirmPopupPanel.SetActive(true);
    }

    public void OnConfirmYes()
    {
        if (clickSound != null) clickSound.PlaySound();

        confirmPopupPanel.SetActive(false);

        if (currentConfirmType == ConfirmType.QuitGame)
        {
            SceneController.Instance.QuitGame();
        }
        else if (currentConfirmType == ConfirmType.GoToMenu)
        {
            TurnOffPhone(); 
            SceneController.Instance.ChangeScene("Login");
        }
        else if (currentConfirmType == ConfirmType.EndGame)
        {
            TurnOffPhone();
            SceneController.Instance.ChangeScene("EndingScene");
        }
    }

    public void OnConfirmNo()
    {
        if (clickSound != null) clickSound.PlaySound();

        currentConfirmType = ConfirmType.None;
        confirmPopupPanel.SetActive(false);
    }

    public void TogglePhone()
    {
        if (clickSound != null) clickSound.PlaySound();

        if (DialogueEditor.ConversationManager.Instance != null)
        {
            if (DialogueEditor.ConversationManager.Instance.IsConversationActive)
                return;
        }

        bool isActive = !phoneContainer.activeSelf;
        phoneContainer.SetActive(isActive);

        if (isActive)
        {
            GoHome();
            ToggleHiddenObjects(false);
        }
        else
        {
            ToggleHiddenObjects(true);
        }
    }

    private void ToggleHiddenObjects(bool isVisible)
    {
        foreach (GameObject obj in objectsToHideWhenOpen)
        {
            if (obj != null)
            {
                obj.SetActive(isVisible);
            }
        }
    }

    public void ChangeMusic()
    {
        if (AudioManager.Instance != null)
        {
            AudioManager.Instance.NextMusic();
            UpdateMusicName();
        }
    }

    private void UpdateMusicName()
    {
        if (musicNameText != null && AudioManager.Instance != null)
        {
            musicNameText.text =
                "Music: " + AudioManager.Instance.GetCurrentMusicName();
        }
    }

    public void IncreaseVolume()
    {
        if (AudioManager.Instance != null)
        {
            AudioListener.volume = Mathf.Clamp(AudioListener.volume + 0.1f, 0f, 1f);
        }
    }
        
    public void DecreaseVolume()
    {
        if (AudioManager.Instance != null)
        {
            AudioListener.volume = Mathf.Clamp(AudioListener.volume - 0.1f, 0f, 1f);
        }
    }

    public void ShowConfirmEndGame()
    {
        if (clickSound != null) clickSound.PlaySound();
        
        currentConfirmType = ConfirmType.EndGame;
        confirmText.text = "Bạn có muốn kết thúc chuyến hành trình ở đây không?";
        confirmPopupPanel.SetActive(true);
    }


    public void TriggerEndingSequence()
    {
        StartCoroutine(WaitUntilDialogueClosed());
    }

    private System.Collections.IEnumerator WaitUntilDialogueClosed()
    {
        yield return new WaitUntil(() => DialogueEditor.ConversationManager.Instance.IsConversationActive == false);
        
        yield return new WaitForSeconds(0.1f);

        if (!phoneContainer.activeSelf)
        {
            TogglePhone(); 
            OpenApp(medalAppPanel);
        }
    }
}