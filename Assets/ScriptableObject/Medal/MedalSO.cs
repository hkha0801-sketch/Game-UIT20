using UnityEngine;

[CreateAssetMenu(fileName = "MedalSO", menuName = "UITGAME/Medal/New medal", order = 0)]
public class MedalSO : ScriptableObject {
    public string MedalID;
    public string MedalName;
    public Sprite MedalIcon;
}