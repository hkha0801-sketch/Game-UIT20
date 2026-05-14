using UnityEngine;
using UnityEngine.UI;

public class LoginUIController : MonoBehaviour
{
    [Header("UI Panels")]
    public GameObject deleteConfirmPanel;

    [Header("UI Buttons")]
    public Button continueButton; 

    private void Start()
    {
        if (deleteConfirmPanel != null) 
            deleteConfirmPanel.SetActive(false);

        RefreshContinueButton();
    }

    public void RefreshContinueButton()
    {
        if (continueButton != null && SaveManager.Instance != null)
        {
            continueButton.interactable = SaveManager.Instance.HasSaveData();
        }
    }

    public void Btn_ShowDeleteConfirm()
    {
        if (deleteConfirmPanel != null) 
            deleteConfirmPanel.SetActive(true);
    }

    public void Btn_CancelDelete()
    {
        if (deleteConfirmPanel != null) 
            deleteConfirmPanel.SetActive(false);
    }

    public void Btn_ConfirmDelete()
    {
        if (SaveManager.Instance != null)
        {
            SaveManager.Instance.DeleteSaveAndReset();
        }

        if (deleteConfirmPanel != null) 
            deleteConfirmPanel.SetActive(false);

        RefreshContinueButton();
    }
}