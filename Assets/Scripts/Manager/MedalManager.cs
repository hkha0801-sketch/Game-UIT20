using System.Collections.Generic;
using UnityEngine;
using DialogueEditor;
using UnityEngine.SceneManagement;

public class MedalManager : MonoBehaviour
{
    public static MedalManager Instance;

    [System.Serializable]
    public struct MilestoneData
    {
        public int RequiredMedals; 
        public string MapName;  
    }

    public List<MedalSO> DatabaseMedals = new List<MedalSO>();
    public List<MedalSO> ownedMedals = new List<MedalSO>();


    [Header("Milestone Settings")]
    public List<MilestoneData> Milestones;

    [Header("Test Tools")]
    public MedalSO MedalToTest;

    [HideInInspector] public int lastNotifiedMilestone = 0;

    private void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(gameObject);
    }

    public void AddMedal(MedalSO newMedal)
    {
        if (!ownedMedals.Contains(newMedal))
        {
            ownedMedals.Add(newMedal);

            if (ConversationManager.Instance != null && ConversationManager.Instance.IsConversationActive)
            {
                ConversationManager.Instance.SetBool("has_" + newMedal.MedalID, true);
            }

            CheckAndNotifyMilestone();

            if (SaveManager.Instance != null) SaveManager.Instance.SaveGame();
        }
    }

    public void RemoveMedal(MedalSO medal)
    {
        if (ownedMedals.Contains(medal))
        {
            ownedMedals.Remove(medal);
            ConversationManager.Instance.SetBool("has_" + medal.MedalID, false);
            
            if (SaveManager.Instance != null) SaveManager.Instance.SaveGame();
        }
    }

    public void SyncMedalsToDialogue()
    {
        foreach (MedalSO medal in ownedMedals)
        {
            if (string.IsNullOrEmpty(medal.MedalID)) continue;
            ConversationManager.Instance.SetBool("has_" + medal.MedalID, true);
        }
    }

    public List<string> GetOwnedMedalIDs()
    {
        List<string> ids = new List<string>();
        foreach (var m in ownedMedals) ids.Add(m.MedalID);
        return ids;
    }

    public void LoadOwnedMedals(List<string> ids)
    {
        ownedMedals.Clear();
        foreach (string id in ids)
        {
            MedalSO foundMedal = DatabaseMedals.Find(x => x.MedalID == id);
            if (foundMedal != null) ownedMedals.Add(foundMedal);
        }
    }

    public void CheckAndNotifyMilestone()
    {
        string sceneName = SceneManager.GetActiveScene().name.ToLower();
        
        Debug.Log($"<color=white>Check Milestone:</color> Tổng sticker hiện có: {GetActualMedalCount()} | Mốc thông báo cuối: {lastNotifiedMilestone}");

        if (sceneName.Contains("minigame") || sceneName == "login") 
        {
            Debug.Log("<color=orange>MedalManager:</color> Đang ở scene cấm, không hiện thông báo.");
            return;
        }

        foreach (MilestoneData ms in Milestones)
        {
            if (GetActualMedalCount() >= ms.RequiredMedals && ms.RequiredMedals > lastNotifiedMilestone)
            {
                Debug.Log($"<color=green>KÍCH HOẠT MỐC:</color> {ms.RequiredMedals} mộc tại {ms.MapName}");
                
                lastNotifiedMilestone = ms.RequiredMedals;
                
                if (SystemNotification.Instance != null)
                {
                    string msg = $"Lan đã xuất hiện tại {ms.MapName}, hãy đi gặp cô ấy nhé!";
                    SystemNotification.Instance.ShowMessage(msg);
                }
                else
                {
                    Debug.LogError("LỖI: Không tìm thấy SystemNotification Instance trong Scene!");
                }
                
                if (SaveManager.Instance != null) SaveManager.Instance.SaveGame();
                break; 
            }
        }
    }

    [ContextMenu("Add Test Medal")]
    public void AddTestMedal()
    {
        if (MedalToTest != null)
        {
            AddMedal(MedalToTest);
        }
    }

    int GetActualMedalCount()
    {
        int count = 0;
        foreach(var m in ownedMedals) if(m.IsActualMedal) count++;
        return count;
    }

    public bool HasMedal(string id)
    {
        return ownedMedals.Exists(m => m.MedalID == id);
    }
}