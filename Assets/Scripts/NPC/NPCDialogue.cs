using UnityEngine;
using DialogueEditor;

public class NPCDialogue : MonoBehaviour
{
    private NPCSO currentData;
    public NPCConversation conversation;

    public void StartDialogue(NPCSO data)
    {
        currentData = data;

        ConversationManager.Instance.StartConversation(conversation);

        if (NPCManager.Instance != null)
            NPCManager.Instance.SyncNPCState(data.NPCID);

        if (MedalManager.Instance != null)
            MedalManager.Instance.SyncMedalsToDialogue();
    }

    public void OnFirstMeetingSuccess()
    {
        if (currentData != null && NPCManager.Instance != null)
        {
            NPCManager.Instance.MarkAsMet(currentData.NPCID);
        }
    }
}