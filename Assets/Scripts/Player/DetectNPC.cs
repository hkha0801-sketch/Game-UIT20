using UnityEngine;

public class DetectNPC : MonoBehaviour
{
    [SerializeField] private float detectDistance = 0.2f;
    [SerializeField] private PlayerMovement playerMovement;
    private Vector2 direction;
    private NPCController currentNPC;


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
            currentNPC.Interact();
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
            SpriteRenderer spriteRenderer;
            spriteRenderer = other.GetComponent<SpriteRenderer>();

            Color tmpColor = spriteRenderer.color;
            tmpColor.a = 1f; 
            spriteRenderer.color = tmpColor;
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
            SpriteRenderer spriteRenderer;
            spriteRenderer = other.GetComponent<SpriteRenderer>();

            Color tmpColor = spriteRenderer.color;
            tmpColor.a = 0f; 
            spriteRenderer.color = tmpColor;
        }
    }

     
}
