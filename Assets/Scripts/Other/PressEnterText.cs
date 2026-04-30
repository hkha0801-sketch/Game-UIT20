using UnityEngine;
using UnityEngine.SceneManagement;

public class PressEnter : MonoBehaviour
{
    public CanvasGroup textGroup;
    public BackGroundMove bgMove; // kéo background vào đây

    void Start()
    {
        // ẩn text lúc đầu
        if (textGroup != null)
            textGroup.alpha = 0;
    }

    void Update()
    {
        // chỉ chạy khi background xong
        if (bgMove != null && bgMove.IsFinished())
        {
            // nhấp nháy
            if (textGroup != null)
            {
                textGroup.alpha = Mathf.Abs(Mathf.Sin(Time.time * 2));
            }

            // bấm Enter
            if (Input.GetKeyDown(KeyCode.Return))
            {
                SceneManager.LoadScene("Map E");
            }
        }
    }
}