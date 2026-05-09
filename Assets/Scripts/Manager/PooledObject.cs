using UnityEngine;

public class PooledObject : MonoBehaviour
{[HideInInspector] public GameObject originalPrefab;

    public void DestroyOrReturn()
    {
        if (ObjectPoolManager.Instance != null && originalPrefab != null)
        {
            ObjectPoolManager.Instance.ReturnObject(originalPrefab, gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }
}