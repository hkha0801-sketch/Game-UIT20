using UnityEngine;
using UnityEngine.UI;

public class MedalSlotUI : MonoBehaviour
{
    [SerializeField] private Image iconImage;
    [SerializeField] private Button slotButton;
    
    private MedalSO myMedal;
    private string myMapName;
    private MedalBookUI bookUI;

    public void Setup(MedalSO medal, string mapName, MedalBookUI ui, bool isOwned)
    {
        myMedal = medal;
        myMapName = mapName;
        bookUI = ui;

        iconImage.sprite = medal.MedalIcon;

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
        bookUI.ShowMedalDetails(myMedal, myMapName);
    }
}