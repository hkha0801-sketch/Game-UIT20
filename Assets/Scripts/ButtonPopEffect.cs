using UnityEngine;
using UnityEngine.EventSystems;
using System.Collections;

public class ButtonPopEffect : MonoBehaviour,
    IPointerDownHandler
{
    private Vector3 originalScale;

    public float popScale = 1.2f;

    private void Start()
    {
        originalScale = transform.localScale;
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        StopAllCoroutines();
        StartCoroutine(PopAnimation());
    }

    IEnumerator PopAnimation()
    {
        transform.localScale =
            originalScale * popScale;

        yield return new WaitForSeconds(0.08f);

        transform.localScale = originalScale;
    }
}