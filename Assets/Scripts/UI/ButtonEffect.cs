using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class ButtonEffect : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{

    private RectTransform rectTransform;
    private Button button;
    private TextMeshProUGUI text;

    private Coroutine expandCoroutine;
    private Coroutine shrinkCoroutine;

    public int ExpansionDurationInFrames = 60;
    private int currentFrameCount = 0;
    public float ExpansionRate = 1f;
    public float Transparency = 0.70f;

    public Color DefaultImageColor = Color.black;
    public Color DefaultTextColor = Color.green;

    public Color ClickedImageColor = Color.green;
    public Color ClickedTextColor = Color.black;

    void Start()
    {
        rectTransform = GetComponent<RectTransform>();
        button = GetComponent<Button>();
        text = button.GetComponentInChildren<TextMeshProUGUI>();
        ToDefaultColors();
    }

    private Color GetTransparentColor(Color color)
    {
        color.a = Transparency;
        return color;
    }

    private void ToDefaultColors()
    {
        button.image.color = GetTransparentColor(DefaultImageColor);
        text.color = GetTransparentColor(DefaultTextColor);
    }

    private void ToClickedColors()
    {
        button.image.color = GetTransparentColor(ClickedImageColor);
        text.color = GetTransparentColor(ClickedTextColor);
    }

    private IEnumerator StartExpanding()
    {
        ToClickedColors();
        if (shrinkCoroutine != null)
        {
            StopCoroutine(shrinkCoroutine);
            shrinkCoroutine = null;
        }
        while (currentFrameCount < ExpansionDurationInFrames)
        {
            rectTransform.sizeDelta = new Vector2(rectTransform.rect.width + ExpansionRate, rectTransform.rect.height + ExpansionRate);
            text.fontSize += 0.5f;
            currentFrameCount++;
            yield return new WaitForEndOfFrame();
        }
    }

    private IEnumerator StartShrinking()
    {
        ToDefaultColors();
        if (expandCoroutine != null)
        {
            StopCoroutine(expandCoroutine);
            expandCoroutine = null;
        }
        while (currentFrameCount > 0)
        {
            rectTransform.sizeDelta = new Vector2(rectTransform.rect.width - ExpansionRate, rectTransform.rect.height - ExpansionRate);
            text.fontSize -= 0.5f;
            currentFrameCount--;
            yield return new WaitForEndOfFrame();
        }
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        expandCoroutine = StartCoroutine(StartExpanding());
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        shrinkCoroutine = StartCoroutine(StartShrinking());
    }
}
