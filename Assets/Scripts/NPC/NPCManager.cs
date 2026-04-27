using System.Collections.Generic;
using UnityEngine;
using DialogueEditor;

public class NPCManager : MonoBehaviour
{
    public static NPCManager Instance;

    private HashSet<string> metNPCIDs = new HashSet<string>();

    private void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(gameObject);
    }

    public void MarkAsMet(string id)
    {
        if (!metNPCIDs.Contains(id))
        {
            metNPCIDs.Add(id);
            Debug.Log($"Đã lưu vào danh bạ: Đã gặp NPC [{id}]");
        }
    }

    public bool HasMet(string id)
    {
        return metNPCIDs.Contains(id);
    }

    public void SyncNPCState(string id)
    {
        bool met = HasMet(id);
        ConversationManager.Instance.SetBool("is_met", met);
    }
}