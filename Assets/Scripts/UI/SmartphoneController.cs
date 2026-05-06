using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

[System.Serializable]
public struct ThemeColorPair
{
    public Color PhoneBorderColor;
    public Color ButtonBorderColor;
}

public class SmartphoneController : MonoBehaviour
{
    private enum ConfirmType { None, QuitGame, GoToMenu }[Header("UI - Cấu trúc điện thoại")]
    public GameObject phoneContainer;
    public GameObject homeScreen;[Header("UI - Các Ứng Dụng (Apps)")]
    public GameObject medalAppPanel;
    public GameObject audioAppPanel;

    [Header("UI - Xác nhận (Popup)")]
    public GameObject confirmPopupPanel;
    public TextMeshProUGUI confirmText;
    private ConfirmType currentConfirmType = ConfirmType.None;[Header("Theme - Đổi màu viền")]
    public Image phoneBorderImage;
    public List<Image> buttonBorderImages;
    public List<ThemeColorPair> themeList;
    private int currentThemeIndex = 0;

    public bool IsPhoneActive => phoneContainer.activeSelf;

    void Start()
    {
        if (themeList.Count > 0)
        {
            ApplyTheme(themeList[0]);
        }
        GoHome();
    }

    public void TurnOffPhone()
    {
        phoneContainer.SetActive(false);
    }

    public void GoHome()
    {
        medalAppPanel.SetActive(false);
        audioAppPanel.SetActive(false);
        confirmPopupPanel.SetActive(false);
        
        homeScreen.SetActive(true);
    }

    public void OpenApp(GameObject appPanel)
    {
        homeScreen.SetActive(false);
        appPanel.SetActive(true);
    }

    public void CycleTheme()
    {
        if (themeList.Count == 0) return;

        currentThemeIndex++;
        if (currentThemeIndex >= themeList.Count)
        {
            currentThemeIndex = 0;
        }

        ApplyTheme(themeList[currentThemeIndex]);
    }

    private void ApplyTheme(ThemeColorPair theme)
    {
        if (phoneBorderImage != null)
            phoneBorderImage.color = theme.PhoneBorderColor;

        foreach (Image btnBorder in buttonBorderImages)
        {
            if (btnBorder != null)
                btnBorder.color = theme.ButtonBorderColor;
        }
    }

    public void ShowConfirmQuitGame()
    {
        currentConfirmType = ConfirmType.QuitGame;
        confirmText.text = "Bạn có chắc chắn muốn thoát game?";
        confirmPopupPanel.SetActive(true);
    }

    public void ShowConfirmGoToMenu()
    {
        currentConfirmType = ConfirmType.GoToMenu;
        confirmText.text = "Bạn có muốn quay lại Menu chính?";
        confirmPopupPanel.SetActive(true);
    }

    public void OnConfirmYes()
    {
        confirmPopupPanel.SetActive(false);

        if (currentConfirmType == ConfirmType.QuitGame)
        {
            SceneController.Instance.QuitGame();
        }
        else if (currentConfirmType == ConfirmType.GoToMenu)
        {
            SceneController.Instance.ChangeScene("Login");
        }
    }

    public void OnConfirmNo()
    {
        currentConfirmType = ConfirmType.None;
        confirmPopupPanel.SetActive(false);
    }

    public void TogglePhone()
    {

        if (DialogueEditor.ConversationManager.Instance.IsConversationActive) return;

        bool isActive = !phoneContainer.activeSelf;
        phoneContainer.SetActive(isActive);

        if (isActive)
        {
            GoHome();
        }
    }


}