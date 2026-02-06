using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class Card : MonoBehaviour
{
    [SerializeField] Image iconImage;

    public Sprite hiddenIconSprite;
    public Sprite iconSprite;

    [HideInInspector] public bool isSelected;
    [HideInInspector] public CardsController controller;

    bool isAnimating;

    public void SetIconSprite(Sprite sprite)
    {
        iconSprite = sprite;
    }

    public void Show()
    {
        if (isAnimating) return;
        StartCoroutine(Flip(true));
    }

    public void Hide()
    {
        if (isAnimating) return;
        StartCoroutine(Flip(false));
    }

    public void HideInstant()
    {
        iconImage.sprite = hiddenIconSprite;
        isSelected = false;
    }

    IEnumerator Flip(bool show)
    {
        isAnimating = true;

        float t = 0f;
        float time = 0.15f;

        while (t < time)
        {
            t += Time.deltaTime;
            float x = Mathf.Lerp(1f, 0f, t / time);
            transform.localScale = new Vector3(x, 1f, 1f);
            yield return null;
        }

        iconImage.sprite = show ? iconSprite : hiddenIconSprite;
        isSelected = show;

        t = 0f;
        while (t < time)
        {
            t += Time.deltaTime;
            float x = Mathf.Lerp(0f, 1f, t / time);
            transform.localScale = new Vector3(x, 1f, 1f);
            yield return null;
        }

        transform.localScale = Vector3.one;
        isAnimating = false;
    }

    public void OnCardClick()
    {
        controller.SetSelected(this);
    }
}
