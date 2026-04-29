using System.Collections.Generic;
using UnityEngine;[CreateAssetMenu(fileName = "MapMedalsSO", menuName = "UITGAME/Medal/Map Medals")]
public class MapMedalsSO : ScriptableObject
{
    public string MapName;
    public List<MedalSO> MedalsInThisMap;
}