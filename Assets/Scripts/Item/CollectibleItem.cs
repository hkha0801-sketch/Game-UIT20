using UnityEngine;
using DialogueEditor;

public class CollectibleItem : MonoBehaviour
{
    public MedalSO medalData;
    public NPCConversation itemConversation;

    public void StartItemDialogue()
    {
        if (itemConversation != null)
        {
            ConversationManager.Instance.StartConversation(itemConversation);
        }
        else
        {
            Collect();
        }
    }
    public void Collect()
    {
        if (MedalManager.Instance != null && medalData != null)
        {
            MedalManager.Instance.AddMedal(medalData);
        }
        gameObject.SetActive(false);
    }
}