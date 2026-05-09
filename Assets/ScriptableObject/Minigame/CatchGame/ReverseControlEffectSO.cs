using UnityEngine;[CreateAssetMenu(fileName = "ReverseEffect", menuName = "UITGAME/Effects/Reverse Control")]
public class ReverseControlEffectSO : CatchEffectSO
{
    public override void ApplyEffect(CatchPlayer player)
    {
        player.ApplyReverse(duration);
    }
}