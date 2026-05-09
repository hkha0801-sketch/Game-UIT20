using UnityEngine;
using TMPro;
using DG.Tweening;

public class FloatingTextObj : MonoBehaviour
{
    public TextMeshPro textMesh;

    public void Setup(string text, Color color)
    {
        textMesh.text = text;
        textMesh.color = color;

        transform.DOMoveY(transform.position.y + 1.5f, 1f).SetEase(Ease.OutCubic);
        textMesh.DOFade(0f, 1f).SetEase(Ease.InExpo).OnComplete(() =>
        {
            GetComponent<PooledObject>().DestroyOrReturn();
        });
    }
}