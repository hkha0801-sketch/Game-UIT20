using UnityEngine;

public abstract class CatchEffectSO : ScriptableObject
{
    public string effectDescription; 
    public Color textColor = Color.yellow; 
    public float duration;
    public abstract void ApplyEffect(CatchPlayer player);
}
