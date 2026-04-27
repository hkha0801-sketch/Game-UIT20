using UnityEngine;

public class NPCController : MonoBehaviour
{
    public NPCSO npcData;
    public NPCDialogue npcDialogue;

    void Awake()
    {
        if (npcDialogue == null)
        {
            npcDialogue = GetComponent<NPCDialogue>();
        }
    }

    public void Interact()
    {
        if (npcData == null)
        {
            Debug.LogError("NPC này chưa được gắn thẻ NPCSO");
            return;
        }

        npcDialogue.StartDialogue(npcData);
    }
}
