using UnityEngine;

public class SpriteSorter : MonoBehaviour
{
    private SpriteRenderer spriteRenderer;

    [Header("Setting")]
    [SerializeField] private int sortingPrecision = 100;
    [SerializeField] private int sortingOffset = 0;
    [SerializeField] private Transform customSortTarget;

    void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    void LateUpdate()
    {
        // Lấy tọa độ Y để tính toán (Ưu tiên lấy theo target nếu có)
        float targetY = (customSortTarget != null) ? customSortTarget.position.y : transform.position.y;

        float rawOrder = targetY * -sortingPrecision;
        
        // Công thức: (Y * -100) + Offset
        spriteRenderer.sortingOrder = Mathf.RoundToInt(rawOrder) + sortingOffset;
    }
}