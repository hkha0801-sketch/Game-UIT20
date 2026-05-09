using UnityEngine;[CreateAssetMenu(fileName = "ScoreEffect", menuName = "UITGAME/Effects/Score Multiplier")]
public class ScoreMultiplierEffectSO : CatchEffectSO
{
    public float scoreMultiplier = 2f;

    public override void ApplyEffect(CatchPlayer player)
    {
        player.ApplyScoreMultiplier(scoreMultiplier, duration);
    }
}