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

        if (Input.GetKeyDown(KeyCode.E) && currentNPC != null)
        {
            if (ConversationManager.Instance != null && !ConversationManager.Instance.IsConversationActive)
            {
                currentNPC.Interact();
            }
        }

        if (Input.GetKeyDown(KeyCode.E) && mapChangePoint != null && currentNPC == null)
        {

            MapPanel panel = mapChangePoint.GetComponent<MapPanel>();
            string mapSceneName = panel.MapData.SceneName;
            string spawnID = panel.targetSpawnPointID;

            SceneController.Instance.ChangeScene(mapSceneName, spawnID);
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

        if (other.CompareTag("MapChangePoint"))
        {
            mapChangePoint = other.gameObject;

            CanvasGroup cg = other.GetComponent<CanvasGroup>();
            if (cg != null)
            {
                cg.alpha = 1f;
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

        if (other.CompareTag("MapChangePoint"))
        {
            mapChangePoint = null;

            CanvasGroup cg = other.GetComponent<CanvasGroup>();
            if (cg != null)
            {
                cg.alpha = 0f;
            }
        }
    }
}