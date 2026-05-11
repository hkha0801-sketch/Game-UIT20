using UnityEngine;
using DialogueEditor;

[CreateAssetMenu(fileName = "NPCSO", menuName = "UITGAME/NPC/New NPC", order = 0)]
public class NPCSO : ScriptableObject {

    [Header("NPC Info")]
    public string NPCID;
    public string NPCName;

}