using UnityEngine;
using DialogueEditor;

public class NPCDialogue : MonoBehaviour
{
    public NPCConversation conversation;

    public void StartDialogue() {
        ConversationManager.Instance.StartConversation(conversation);
    }
}
