using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class Icon : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    private Image img;

    private void Awake()
    {
        img = GetComponent<Image>();
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        Debug.Log("mouse enter");
        SetAlpha(1f);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        Debug.Log("mouse exit");
        SetAlpha(0.5f);
    }

    private void SetAlpha(float alpha)
    {
        if (img != null)
        {
            Color tmp = img.color;
            tmp.a = alpha;
            img.color = tmp;
        }
    }
}