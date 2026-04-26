using UnityEngine;
using DialogueEditor;

public class NPCDialogue : MonoBehaviour
{
    public NPCConversation conversation;

    public void StartDialogue() { //called by NPCDetect
        ConversationManager.Instance.StartConversation(conversation);
        SyncMedalsToDialogue();
    }

    private void SyncMedalsToDialogue()
    {
        if (MedalManager.Instance == null) return;

        foreach (MedalSO medal in MedalManager.Instance.ownedMedals)
        {
            string flagName = "has_" + medal.MedalID;
            //Debug.Log(flagName);
            ConversationManager.Instance.SetBool(flagName, true);
        }
    }
}
