using System.Collections.Generic;
using UnityEngine;
using DialogueEditor;
using System.Linq;

public class NPCManager : MonoBehaviour
{
    public static NPCManager Instance;

    private HashSet<string> metNPCIDs = new HashSet<string>();
    public string LastInteractedNPCID;

    private void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(gameObject);
    }

    public void MarkAsMet(string id)
    {
        if (!metNPCIDs.Contains(id)) metNPCIDs.Add(id);
        if (SaveManager.Instance != null) SaveManager.Instance.SaveGame();
    }

    public bool HasMet(string id)
    {
        return metNPCIDs.Contains(id);
    }

    public void SyncNPCState(string id)
    {
        ConversationManager.Instance.SetBool("is_met", HasMet(id));
    }

    public List<string> GetMetNPCs()
    {
        return metNPCIDs.ToList();
    }

    public void LoadMetNPCs(List<string> ids)
    {
        metNPCIDs = new HashSet<string>(ids);
    }

    public void ClearMetNPCs()
    {
        metNPCIDs.Clear();
    }
}