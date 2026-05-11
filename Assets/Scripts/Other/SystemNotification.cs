using UnityEngine;
using TMPro;
using System.Collections;

public class SystemNotification : MonoBehaviour
{
    public static SystemNotification Instance;

    public GameObject NotificationPanel;
    public TextMeshProUGUI NotificationText;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else Destroy(gameObject);
    }

    private void Start()
    {
        if (NotificationPanel != null) NotificationPanel.SetActive(false);
    }

    public void ShowMessage(string message, float duration = 4f)
    {
        StopAllCoroutines();
        StartCoroutine(Routine(message, duration));
    }

    private IEnumerator Routine(string message, float duration)
    {
        if (NotificationPanel != null && NotificationText != null)
        {
            NotificationText.text = message;
            NotificationPanel.SetActive(true);

            yield return new WaitForSeconds(duration);

            NotificationPanel.SetActive(false);
        }
    }
}