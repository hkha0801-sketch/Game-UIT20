using UnityEngine;
using DialogueEditor;

public class NpcDialouge : MonoBehaviour
{
    public NPCConversation conversation;

    private void OnMouseDown() {
        ConversationManager.Instance.StartConversation(conversation);
    }
}
