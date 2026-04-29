using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class MedalBookUI : MonoBehaviour
{
    [Header("Data")]
    public List<MapMedalsSO> AllMaps;
    private int currentMapIndex = 0;
    [Header("Left Panel - Grid")]
    public TextMeshProUGUI mapTitleText;
    public Transform gridParent;
    public GameObject medalSlotPrefab;

    [Header("Right Panel - Details")]
     public GameObject detailIconObject;
    public GameObject detailTextPanelObject;
    public Image detailIcon;
    public TextMeshProUGUI detailNameText;
    public TextMeshProUGUI detailLocationText;
    public TextMeshProUGUI detailDescText;

    private void OnEnable()
    {
        currentMapIndex = 0;
        HideMedalDetails();
        UpdateBookDisplay();
    }

    public void NextMap()
    {
        currentMapIndex++;
        if (currentMapIndex >= AllMaps.Count) currentMapIndex = 0;
        HideMedalDetails();
        UpdateBookDisplay();
    }

    public void PrevMap()
    {
        currentMapIndex--;
        if (currentMapIndex < 0) currentMapIndex = AllMaps.Count - 1;
        HideMedalDetails();
        UpdateBookDisplay();
    }

    private void UpdateBookDisplay()
    {
        MapMedalsSO currentMap = AllMaps[currentMapIndex];
        mapTitleText.text = currentMap.MapName;

        foreach (Transform child in gridParent)
        {
            Destroy(child.gameObject);
        }

        foreach (MedalSO medal in currentMap.MedalsInThisMap)
        {
            if (medal == null) continue; 

            GameObject slotObj = Instantiate(medalSlotPrefab, gridParent);
            MedalSlotUI slotUI = slotObj.GetComponent<MedalSlotUI>();
            
            bool isOwned = false;
            if (MedalManager.Instance != null && MedalManager.Instance.ownedMedals != null)
            {
                isOwned = MedalManager.Instance.ownedMedals.Contains(medal);
            }
            
            slotUI.Setup(medal, currentMap.MapName, this, isOwned);
        }
    }

    public void ShowMedalDetails(MedalSO medal, string locationName)
    {
        if (detailIconObject != null) detailIconObject.SetActive(true);
        if (detailTextPanelObject != null) detailTextPanelObject.SetActive(true);

        detailIcon.sprite = medal.MedalIcon;
        detailNameText.text = "<b>Tên mộc: </b>" + medal.MedalName;
        detailLocationText.text = "<b>Vị trí: </b>" + locationName;
        detailDescText.text = "<b>Mô tả: </b>" + medal.Description;
    }

    public void HideMedalDetails()
    {
        if (detailIconObject != null) detailIconObject.SetActive(false);
        if (detailTextPanelObject != null) detailTextPanelObject.SetActive(false);
    }

    public void CloseBook()
    {
        gameObject.SetActive(false);
    }

    public void OpenBook()
    {
        gameObject.SetActive(true); 
    }
}