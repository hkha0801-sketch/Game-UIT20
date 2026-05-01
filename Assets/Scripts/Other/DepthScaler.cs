using UnityEngine;

public class DepthScaler : MonoBehaviour
{
    [HideInInspector] public float bottomY, topY;
    [HideInInspector] public float scaleAtBottom, scaleAtTop;
    
    private int overrideCount = 0; 

    public void RegisterOverride(bool isEntering)
    {
        if (isEntering) overrideCount++;
        else overrideCount--;

        overrideCount = Mathf.Max(0, overrideCount);
    }

      public void ResetOverride()
    {
        overrideCount = 0;
    }


    void LateUpdate()
    {
        // Chỉ scale theo Map nếu không có vùng nào đang đè (overrideCount == 0)
        if (overrideCount > 0 || Mathf.Approximately(topY, bottomY)) return;

        float t = Mathf.InverseLerp(bottomY, topY, transform.position.y);
        float currentScale = Mathf.Lerp(scaleAtBottom, scaleAtTop, t);
        
        float signX = Mathf.Sign(transform.localScale.x); 
        transform.localScale = new Vector3(currentScale * signX, currentScale, 1f);
    }
}