using UnityEngine;
using DialogueEditor;

[CreateAssetMenu(fileName = "NPCSO", menuName = "UITGAME/NPC/New NPC", order = 0)]
public class NPCSO : ScriptableObject {

    [Header("NPC Infor")]
    public string NPCID;
    public string NPCName;

}