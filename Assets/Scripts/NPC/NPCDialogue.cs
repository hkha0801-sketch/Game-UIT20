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

        if (MinigameManager.Instance != null) 
            MinigameManager.Instance.SyncMinigameResultsToDialogue();
    }

    public void OnFirstMeetingSuccess()
    {
        if (currentData != null && NPCManager.Instance != null)
            NPCManager.Instance.MarkAsMet(currentData.NPCID);
    }

    public void GiveMedal(MedalSO medal)
    {
        if (MedalManager.Instance != null)
            MedalManager.Instance.AddMedal(medal);
    }

    public void CallMinigame(MinigameSO minigameData)
    {
        if (MinigameManager.Instance != null)
        {
            MinigameManager.Instance.StartMinigame(minigameData, currentData.NPCID);
        }
    }

    public void StartEndingSequence()
    {
        if (SmartphoneController.Instance != null)
        {
            SmartphoneController.Instance.TriggerEndingSequence();
        }
    }
}