using UnityEngine;
using DialogueEditor;

public class DetectNPC : MonoBehaviour
{
    [Header("Settings")]
    [SerializeField] private float detectDistance = 0.2f;
    [SerializeField] private PlayerMovement playerMovement;

    [Header("Y Offsets (Bù trừ độ lệch trục Y)")]
    [SerializeField] private float offsetY_Horizontal = 0f; // Dùng chung cho hướng Trái/Phải
    [SerializeField] private float offsetY_Up = 0f;         // Dùng cho hướng Lên
    [SerializeField] private float offsetY_Down = 0f;       // Dùng cho hướng Xuống

    private Vector2 direction;
    private NPCController currentNPC;

    void Update()
    {
        direction = playerMovement.direction;

        if (direction != Vector2.zero)
        {
            // 1. Tính toán vị trí gốc (Có thêm .normalized để khoảng cách chéo không bị xa hơn)
            Vector2 basePosition = direction.normalized * detectDistance;

            // 2. Xác định độ lệch Y dựa trên hướng đang quay
            float currentOffsetY = 0f;

            // Kiểm tra xem đang ưu tiên nhìn hướng nào (Trái/Phải hay Lên/Xuống)
            if (Mathf.Abs(direction.x) > 0.1f) 
            {
                // Đang đi sang Trái hoặc Phải
                currentOffsetY = offsetY_Horizontal;
            }
            else if (direction.y > 0.1f) 
            {
                // Đang đi Lên
                currentOffsetY = offsetY_Up;
            }
            else if (direction.y < -0.1f) 
            {
                // Đang đi Xuống
                currentOffsetY = offsetY_Down;
            }

            // 3. Áp dụng độ lệch vào trục Y của khung detect
            transform.localPosition = new Vector3(basePosition.x, basePosition.y + currentOffsetY, 0f);

            // 4. Xoay khung theo hướng
            float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
            transform.rotation = Quaternion.Euler(0f, 0f, angle - 90f);
        }

        // Tương tác với NPC (Đã thêm check an toàn của Dialogue Editor)
        if (Input.GetKeyDown(KeyCode.E) && currentNPC != null)
        {
            // Chỉ tương tác nếu hộp thoại CHƯA mở
            if (ConversationManager.Instance != null && !ConversationManager.Instance.IsConversationActive)
            {
                currentNPC.Interact();
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D other) 
    {
        if (other.CompareTag("NPC"))
        {
            currentNPC = other.GetComponent<NPCController>();
        }

        if (other.CompareTag("NPCUI"))
        {
            SpriteRenderer spriteRenderer = other.GetComponent<SpriteRenderer>();
            if (spriteRenderer != null)
            {
                Color tmpColor = spriteRenderer.color;
                tmpColor.a = 1f; 
                spriteRenderer.color = tmpColor;
            }
        }
    }

    private void OnTriggerExit2D(Collider2D other) 
    {
        if (other.CompareTag("NPC"))
        {
            currentNPC = null;
        }

        if (other.CompareTag("NPCUI"))
        {
            SpriteRenderer spriteRenderer = other.GetComponent<SpriteRenderer>();
            if (spriteRenderer != null)
            {
                Color tmpColor = spriteRenderer.color;
                tmpColor.a = 0f; 
                spriteRenderer.color = tmpColor;
            }
        }
    }
}