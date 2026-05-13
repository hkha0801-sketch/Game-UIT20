using UnityEngine;

public class PressEnter : MonoBehaviour
{
    public CanvasGroup textGroup;
    public BackGroundMove bgMove;
    public SoundFeedback pressSound;

    void Start()
    {
        if (textGroup != null)
            textGroup.alpha = 0;
    }

    void Update()
    {
        if (bgMove != null && bgMove.IsFinished())
        {
            if (textGroup != null)
            {
                textGroup.alpha = Mathf.Abs(Mathf.Sin(Time.time * 2));
            }

            if (Input.GetKeyDown(KeyCode.E))
            {
                if (pressSound != null) pressSound.PlaySound();

                if (SaveManager.Instance != null)
                {
                    SaveManager.Instance.ContinueSavedGame();
                }
                else
                {
                    SceneController.Instance.PlayGame();
                }
            }
        }
    }
}