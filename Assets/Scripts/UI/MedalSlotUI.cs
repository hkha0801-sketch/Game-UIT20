using UnityEngine;
using UnityEngine.UI;

public class MedalSlotUI : MonoBehaviour
{
    [Header("Data")]
    public MedalSO medalData; 

    [Header("UI References")]
    [SerializeField] private Image iconImage;
    [SerializeField] private Button slotButton;
    
    private MedalBookUI bookUI;

    public void RefreshSlot(MedalBookUI ui)
    {
        bookUI = ui;
        if (medalData == null) {
            iconImage.color = Color.black; 
            slotButton.interactable = false;
            return;
        }

        iconImage.sprite = medalData.MedalIcon;

        bool isOwned = false;
        if (MedalManager.Instance != null && MedalManager.Instance.ownedMedals != null)
        {
            isOwned = MedalManager.Instance.ownedMedals.Contains(medalData);
        }

        if (isOwned)
        {
            iconImage.color = Color.white; 
            slotButton.interactable = true;
        }
        else
        {
            iconImage.color = Color.black; 
            slotButton.interactable = false;
        }

        slotButton.onClick.RemoveAllListeners();
        slotButton.onClick.AddListener(OnSlotClicked);
    }

    private void OnSlotClicked()
    {
        bookUI.ShowMedalDetails(medalData);
    }
}