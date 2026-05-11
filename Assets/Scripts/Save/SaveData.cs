using System.Collections.Generic;
using UnityEngine;[System.Serializable]
public class SaveData
{
    public List<string> OwnedMedalIDs = new List<string>();
    public List<string> MetNPCIDs = new List<string>();
    public List<string> MinigameIDs = new List<string>();
    public List<int> MinigameResults = new List<int>();
    
    public string SavedSceneName;
    
    public bool IsSavedInMinigame; 
    public Vector3 ReturnPosition;
    public Vector2 ReturnDirection;
    public int LastNotifiedMilestone; 
}