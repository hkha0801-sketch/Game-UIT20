using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class MemoryCard : MonoBehaviour
{
    public Image frontImage;
    public GameObject coverObj;
    public Button cardButton;[HideInInspector] public int pairID; 
    private MemoryGameController controller;

    public void SetupCard(int id, Sprite frontSprite, MemoryGameController ctrl)
    {
        pairID = id;
        frontImage.sprite = frontSprite;
        controller = ctrl;
        
        transform.rotation = Quaternion.Euler(Vector3.zero);
        transform.localScale = Vector3.one;

        coverObj.SetActive(true);
        cardButton.interactable = true;
        
        cardButton.onClick.RemoveAllListeners();
        cardButton.onClick.AddListener(OnCardClicked);
    }

    private void OnCardClicked()
    {
        controller.CardClicked(this);
    }

    public void Reveal()
    {
        cardButton.interactable = false;

        Sequence seq = DOTween.Sequence();
        seq.Append(transform.DORotate(new Vector3(0, 90, 0), 0.15f));
        seq.AppendCallback(() => coverObj.SetActive(false));
        seq.Append(transform.DORotate(new Vector3(0, 0, 0), 0.15f));
    }

    public void Cover()
    {
        Sequence seq = DOTween.Sequence();
        seq.Append(transform.DORotate(new Vector3(0, 90, 0), 0.15f));
        seq.AppendCallback(() => coverObj.SetActive(true));
        seq.Append(transform.DORotate(new Vector3(0, 0, 0), 0.15f)).OnComplete(() =>
        {
            cardButton.interactable = true;
        });
    }

    public void SetMatched()
    {
        transform.DOScale(Vector3.zero, 0.3f).SetEase(Ease.InBack).OnComplete(() =>
        {
            gameObject.SetActive(false);
        });
    }

    private void OnDestroy()
    {
        transform.DOKill();
    }
}