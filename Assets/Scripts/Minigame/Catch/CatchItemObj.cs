using UnityEngine;

public class CatchItemObj : MonoBehaviour
{
    private float fallSpeed;
    private int scoreValue;
    private CatchEffectSO effectData;
    private CatchGameController controller;

    public void SetupNormal(Sprite sprite, float speed, int score, CatchGameController ctrl)
    {
        var sr = GetComponent<SpriteRenderer>();
        if (sr != null && sprite != null)
        {
            sr.sprite = sprite;
            NormalizeScale(sr, 1f);
        }
        else if (sprite == null)
        {
            Debug.LogError($"Vật phẩm {gameObject.name} bị thiếu Sprite trong SO!");
        }
        
        fallSpeed = speed;
        scoreValue = score;
        effectData = null;
        controller = ctrl;
    }

    public void SetupEffect(Sprite sprite, float speed, CatchEffectSO effect, CatchGameController ctrl)
    {
        var sr = GetComponent<SpriteRenderer>();
        if (sr != null && sprite != null)
        {
            sr.sprite = sprite;
            NormalizeScale(sr, 1f);
        }
        else if (sprite == null)
        {
            Debug.LogError($"Vật phẩm hiệu ứng {gameObject.name} bị thiếu Sprite trong SO!");
        }

        fallSpeed = speed;
        scoreValue = 0;
        effectData = effect;
        controller = ctrl;
    }

    private void NormalizeScale(SpriteRenderer sr, float targetSize)
    {
        if (sr == null || sr.sprite == null) return; 

        float width = sr.sprite.bounds.size.x;
        float height = sr.sprite.bounds.size.y;

        float maxDim = Mathf.Max(width, height);
        if (maxDim <= 0) return;

        float scaleFactor = targetSize / maxDim;
        transform.localScale = new Vector3(scaleFactor, scaleFactor, 1f);
    }

    void Update()
    {
        transform.Translate(Vector3.down * fallSpeed * Time.deltaTime);

        if (transform.position.y < -10f) 
            GetComponent<PooledObject>().DestroyOrReturn();
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            CatchPlayer player = other.GetComponent<CatchPlayer>();
            if (player != null)
            {
                if (effectData != null)
                {
                    controller.ShowEffectText(effectData.effectDescription, effectData.textColor);
                    effectData.ApplyEffect(player);
                }
                else
                {
                    float finalScore = scoreValue * (scoreValue > 0 ? player.ScoreMultiplier : 1f);
                    controller.AddScore(Mathf.RoundToInt(finalScore), transform.position);
                }
            }
            GetComponent<PooledObject>().DestroyOrReturn();
        }
    }
}