using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class MedalBookUI : MonoBehaviour
{[Header("Medals List")]
    public List<MedalSlotUI> allMedalSlots; 

    [Header("Info Panel")]
    public GameObject detailIconObject;
    public GameObject detailTextPanelObject;
    public UnityEngine.UI.Image detailIcon;
    public TextMeshProUGUI detailNameText;
    public TextMeshProUGUI detailDescText;

    private void OnEnable()
    {
        HideMedalDetails();
        UpdateAllSlots(); 
    }

    public void UpdateAllSlots()
    {
        foreach (MedalSlotUI slot in allMedalSlots)
        {
            if (slot != null)
            {
                slot.RefreshSlot(this);
            }
        }
    }

    public void ShowMedalDetails(MedalSO medal)
    {
        if (detailIconObject != null) detailIconObject.SetActive(true);
        if (detailTextPanelObject != null) detailTextPanelObject.SetActive(true);

        detailIcon.sprite = medal.MedalIcon;
        detailNameText.text = medal.MedalName;
        detailDescText.text = "<b>Mô tả: </b>" + medal.Description;
    }

    public void HideMedalDetails()
    {
        if (detailIconObject != null) detailIconObject.SetActive(false);
        if (detailTextPanelObject != null) detailTextPanelObject.SetActive(false);
    }
}