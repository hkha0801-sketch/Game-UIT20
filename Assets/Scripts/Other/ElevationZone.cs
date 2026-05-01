using UnityEngine;

public class ElevationZone : MonoBehaviour
{
    public enum ZoneType { UpperArea, TransitionStairs }
    public ZoneType zoneType;
    public bool isRadial = false;

    [Header("Speed Settings")]
    public float speedMultiplier = 0.7f;

    [Header("Upper Area Setting")]
    public float fixedScale = 1.3f; 

    [Header("Stairs Setting")]
    public Transform bottomPoint; 
    public Transform topPoint;    
    public float scaleBottom = 1f;
    public float scaleTop = 1.3f;

    private Transform player;
    private DepthScaler playerScaler;
    private PlayerMovement playerMovement;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            player = other.transform;
            playerScaler = player.GetComponent<DepthScaler>();
            playerMovement = player.GetComponent<PlayerMovement>();
            
            if (playerScaler != null) 
                playerScaler.RegisterOverride(true);
            
            if (playerMovement != null) 
                playerMovement.speedMultiplier = speedMultiplier;
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            if (playerScaler != null) 
                playerScaler.RegisterOverride(false);

            if (playerMovement != null) 
                playerMovement.speedMultiplier = 1f;
            
            player = null;
            playerMovement = null;
        }
    }

    private void LateUpdate()
    {
        if (player == null) return;

        float targetScale = 1f;
        if (zoneType == ZoneType.UpperArea)
        {
            targetScale = fixedScale;
        }
        else if (zoneType == ZoneType.TransitionStairs && bottomPoint != null && topPoint != null)
        {
            if (isRadial)
            {
                float distToCenter = Vector2.Distance(player.position, topPoint.position);
                float maxDist = Vector2.Distance(bottomPoint.position, topPoint.position);
                float t = 1f - Mathf.InverseLerp(0f, maxDist, distToCenter);
                targetScale = Mathf.Lerp(scaleBottom, scaleTop, Mathf.Clamp01(t));
            }
            else
            {
                Vector2 lineDir = topPoint.position - bottomPoint.position;
                Vector2 playerDir = (Vector2)player.position - (Vector2)bottomPoint.position;
                float t = Vector2.Dot(playerDir, lineDir.normalized) / lineDir.magnitude;
                targetScale = Mathf.Lerp(scaleBottom, scaleTop, Mathf.Clamp01(t));
            }
        }

        float signX = Mathf.Sign(player.localScale.x);
        player.localScale = new Vector3(targetScale * signX, targetScale, 1f);
    }
}