using UnityEngine;

public class DetectNPC : MonoBehaviour
{
    [SerializeField] private float detectDistance = 0.2f;
    [SerializeField] private PlayerMovement playerMovement;
    private Vector2 direction;
    private NPCDialogue currentNPC;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        direction = playerMovement.direction;

        if (direction != Vector2.zero)
        {
            transform.localPosition = direction * detectDistance;

            float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
            transform.rotation = Quaternion.Euler(0f, 0f, angle - 90f);
        }

        if (Input.GetKeyDown(KeyCode.E) && currentNPC != null)
        {
            currentNPC.StartDialogue();
        }

    }

    private void OnTriggerEnter2D(Collider2D other) 
    {
        if (other.CompareTag("NPC"))
        {
            currentNPC = other.GetComponent<NPCDialogue>();
        }
    }

    // Bước ra xa NPC -> Xóa khỏi bộ nhớ (Chỉ chạy 1 lần)
    private void OnTriggerExit2D(Collider2D other) 
    {
        if (other.CompareTag("NPC"))
        {
            currentNPC = null;
        }
    }
}
