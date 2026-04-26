using System.Collections.Generic;
using UnityEngine;
using DialogueEditor;

public class MedalManager : MonoBehaviour
{
    public static MedalManager Instance;
    public List<MedalSO> ownedMedals = new List<MedalSO>();

    private void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(gameObject);
    }

    public void AddMedal(MedalSO newMedal)
    {
        if (!ownedMedals.Contains(newMedal))
        {
            ownedMedals.Add(newMedal);
            string flagName = "has_" + newMedal.MedalID;

            Debug.Log("add new medal: " + flagName);
            ConversationManager.Instance.SetBool(flagName, true);
        }
    }
}