using UnityEngine;
using DialogueEditor;

public class PlayerInteraction : MonoBehaviour
{
    [Header("Settings")]
    [SerializeField] private float detectDistance = 0.2f;
    [SerializeField] private PlayerMovement playerMovement;

    [Header("Y Offsets")]
    [SerializeField] private float offsetY_Horizontal = 0f;
    [SerializeField] private float offsetY_Up = 0f; 
    [SerializeField] private float offsetY_Down = 0f;

    private Vector2 direction;
    private NPCController currentNPC;
    private GameObject mapChangePoint;
    private CollectibleItem currentItem;

    void Update()
    {
        direction = playerMovement.direction;

        if (direction != Vector2.zero)
        {
            Vector2 basePosition = direction.normalized * detectDistance;

            float currentOffsetY = 0f;

            if (Mathf.Abs(direction.x) > 0.1f) 
            {
                currentOffsetY = offsetY_Horizontal;
            }
            else if (direction.y > 0.1f) 
            {
                currentOffsetY = offsetY_Up;
            }
            else if (direction.y < -0.1f) 
            {
                currentOffsetY = offsetY_Down;
            }

            transform.localPosition = new Vector3(basePosition.x, basePosition.y + currentOffsetY, 0f);

            float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
            transform.rotation = Quaternion.Euler(0f, 0f, angle - 90f);
        }

        if (Input.GetKeyDown(KeyCode.E))
        {
            if (ConversationManager.Instance != null && ConversationManager.Instance.IsConversationActive)
                return;

            if (currentItem != null)
            {
                currentItem.StartItemDialogue();
                return; 
            }

            if (currentNPC != null)
            {
                currentNPC.Interact();
                return;
            }

            if (mapChangePoint != null)
            {
                MapPanel panel = mapChangePoint.GetComponent<MapPanel>();
                if (panel != null && panel.MapData != null)
                {
                    SceneController.Instance.ChangeScene(panel.MapData.SceneName, panel.targetSpawnPointID);
                    return;
                }
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D other) 
    {

        if (other.CompareTag("NPCUI"))
        {
            SpriteRenderer spriteRenderer = other.GetComponent<SpriteRenderer>();
            if (spriteRenderer != null)
            {
                Color tmpColor = spriteRenderer.color;
                tmpColor.a = 1f; 
                spriteRenderer.color = tmpColor;
            }

            currentNPC = other.GetComponentInParent<NPCController>();
        }

        if (other.CompareTag("MapChangePoint"))
        {
            mapChangePoint = other.gameObject;

            CanvasGroup cg = other.GetComponent<CanvasGroup>();
            if (cg != null)
            {
                cg.alpha = 1f;
            }
        }

        if (other.CompareTag("Collectible"))
        {
            currentItem = other.GetComponent<CollectibleItem>();
        }
    }

    private void OnTriggerExit2D(Collider2D other) 
    {
        if (other.CompareTag("NPCUI"))
        {
            SpriteRenderer spriteRenderer = other.GetComponent<SpriteRenderer>();
            if (spriteRenderer != null)
            {
                Color tmpColor = spriteRenderer.color;
                tmpColor.a = 0f; 
                spriteRenderer.color = tmpColor;
            }

            currentNPC = null;
        }

        if (other.CompareTag("MapChangePoint"))
        {
            mapChangePoint = null;

            CanvasGroup cg = other.GetComponent<CanvasGroup>();
            if (cg != null)
            {
                cg.alpha = 0f;
            }
        }

         if (other.CompareTag("Collectible"))
        {
            currentItem = null;
        }
    }
}