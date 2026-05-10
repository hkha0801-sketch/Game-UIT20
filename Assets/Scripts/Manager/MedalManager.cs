using System.Collections.Generic;
using UnityEngine;
using DialogueEditor;

public class MedalManager : MonoBehaviour
{
    public static MedalManager Instance;

    public List<MedalSO> DatabaseMedals = new List<MedalSO>();
    public List<MedalSO> ownedMedals = new List<MedalSO>();

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
            ConversationManager.Instance.SetBool("has_" + newMedal.MedalID, true);

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
}