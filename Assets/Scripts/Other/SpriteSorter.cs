using UnityEngine;

public class SpriteSorter : MonoBehaviour
{
    private SpriteRenderer spriteRenderer;

    [SerializeField] private int sortingPrecision = 100;

    void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    void LateUpdate()
    {

        float rawOrder = transform.position.y * -sortingPrecision;
        spriteRenderer.sortingOrder = Mathf.RoundToInt(rawOrder);
    }
}