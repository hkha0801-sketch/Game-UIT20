using UnityEngine;[CreateAssetMenu(fileName = "SpeedEffect", menuName = "UITGAME/Effects/Speed Change")]
public class SpeedChangeEffectSO : CatchEffectSO
{
    public float speedMultiplier = 1.5f;

    public override void ApplyEffect(CatchPlayer player)
    {
        player.ApplySpeedMultiplier(speedMultiplier, duration);
    }
}