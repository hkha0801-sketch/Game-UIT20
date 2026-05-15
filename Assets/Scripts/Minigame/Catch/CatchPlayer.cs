using System.Collections;
using UnityEngine;

public class CatchPlayer : MonoBehaviour
{
    public float baseSpeed = 8f;
    public float limitX = 8f;

    private float currentSpeedMultiplier = 1f;
    private float currentReverseMultiplier = 1f;
    
    public float ScoreMultiplier { get; private set; } = 1f;

    private Coroutine speedCoroutine;
    private Coroutine reverseCoroutine;
    private Coroutine scoreCoroutine;

    private SpriteRenderer spriteRenderer;

    void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    void Update()
    {
        float moveX = Input.GetAxis("Horizontal");
        
        float finalSpeed = baseSpeed * currentSpeedMultiplier;
        float actualMove = moveX * finalSpeed * currentReverseMultiplier * Time.deltaTime;

        transform.Translate(Vector3.right * actualMove);

        Vector3 pos = transform.position;
        pos.x = Mathf.Clamp(pos.x, -limitX, limitX);
        transform.position = pos;

        if (actualMove < 0)
        {
            spriteRenderer.flipX = true;
        }
        else if (actualMove > 0)
        {
            spriteRenderer.flipX = false;
        }
    }

    public void ApplySpeedMultiplier(float multiplier, float duration)
    {
        if (speedCoroutine != null) StopCoroutine(speedCoroutine);
        speedCoroutine = StartCoroutine(SpeedRoutine(multiplier, duration));
    }

    public void ApplyReverse(float duration)
    {
        if (reverseCoroutine != null) StopCoroutine(reverseCoroutine);
        reverseCoroutine = StartCoroutine(ReverseRoutine(duration));
    }

    public void ApplyScoreMultiplier(float multiplier, float duration)
    {
        if (scoreCoroutine != null) StopCoroutine(scoreCoroutine);
        scoreCoroutine = StartCoroutine(ScoreRoutine(multiplier, duration));
    }

    private IEnumerator SpeedRoutine(float multiplier, float duration)
    {
        currentSpeedMultiplier = multiplier;
        yield return new WaitForSeconds(duration);
        currentSpeedMultiplier = 1f;
    }

    private IEnumerator ReverseRoutine(float duration)
    {
        currentReverseMultiplier = -1f;
        yield return new WaitForSeconds(duration);
        currentReverseMultiplier = 1f;
    }

    private IEnumerator ScoreRoutine(float multiplier, float duration)
    {
        ScoreMultiplier = multiplier;
        yield return new WaitForSeconds(duration);
        ScoreMultiplier = 1f;
    }

    public void ResetAllEffects()
    {
        StopAllCoroutines();
        currentSpeedMultiplier = 1f;
        currentReverseMultiplier = 1f;
        ScoreMultiplier = 1f;
    }
}