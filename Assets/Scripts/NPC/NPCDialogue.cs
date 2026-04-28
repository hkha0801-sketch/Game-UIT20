// using UnityEngine;
// using DialogueEditor;

// public class NPCDialogue : MonoBehaviour
// {
//     private NPCSO currentData;
//     public NPCConversation conversation;

//     public void StartDialogue(NPCSO data)
//     {
//         currentData = data;

//         ConversationManager.Instance.StartConversation(conversation);

//         if (NPCManager.Instance != null)
//             NPCManager.Instance.SyncNPCState(data.NPCID);

//         if (MedalManager.Instance != null)
//             MedalManager.Instance.SyncMedalsToDialogue();

//         if (MinigameManager.Instance != null) 
//             MinigameManager.Instance.SyncMinigameResultsToDialogue();
//     }

//     public void OnFirstMeetingSuccess()
//     {
//         if (currentData != null && NPCManager.Instance != null)
//         {
//             NPCManager.Instance.MarkAsMet(currentData.NPCID);
//         }
//     }

//     public void GiveMedal(MedalSO medal)
//     {
//         if (MedalManager.Instance != null)
//         {
//             MedalManager.Instance.AddMedal(medal);
//         }
//         else
//         {
//             Debug.LogError("Không tìm thấy MedalManager Instance!");
//         }
//     }

//     public void CallMinigame(MinigameSO minigameData)
//     {
//         if (MinigameManager.Instance != null)
//         {
//             // Gọi Manager để chuyển Scene và bắt đầu trò chơi
//             MinigameManager.Instance.StartMinigame(minigameData);
//         }
//         else
//         {
//             Debug.LogError("NPCDialogue: Không tìm thấy MinigameManager!");
//         }
//     }
// }

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
            // Truyền thêm NPCID để quay về tự động kích hoạt thoại
            MinigameManager.Instance.StartMinigame(minigameData, currentData.NPCID);
        }
    }
}