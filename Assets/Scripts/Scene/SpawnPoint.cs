using UnityEngine;

public class SpawnPoint : MonoBehaviour
{
    public string pointID;
    public Vector2 facingDirection = Vector2.down;
    public bool gimozOn = false;

    private void OnDrawGizmos()
    {
        if (gimozOn)
        {
            Gizmos.color = Color.green;
            Gizmos.DrawWireSphere(transform.position, 0.5f);
            Gizmos.DrawLine(transform.position, transform.position + (Vector3)facingDirection);
        }
    }
}