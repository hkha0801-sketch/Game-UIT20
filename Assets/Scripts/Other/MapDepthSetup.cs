using UnityEngine;

public class MapDepthSetup : MonoBehaviour
{
    [Header("Thông số Depth Map này")]
    public float mapBottomY;
    public float mapTopY;
    public float scaleAtBottom = 1.2f;
    public float scaleAtTop = 0.5f;

    void Start()
    {
        GameObject player = GameObject.FindGameObjectWithTag("Player");
        if (player != null)
        {
            DepthScaler scaler = player.GetComponent<DepthScaler>();
            if (scaler != null)
            {
                scaler.bottomY = mapBottomY;
                scaler.topY = mapTopY;
                scaler.scaleAtBottom = scaleAtBottom;
                scaler.scaleAtTop = scaleAtTop;
                
                scaler.ResetOverride(); 
            }
        }
    }

}