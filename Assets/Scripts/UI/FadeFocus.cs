using UnityEngine.EventSystems;
using System.Collections;
using UnityEngine;

public class FadeFocus : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{


    private CanvasGroup canvasGroup;

    private Coroutine brightenCoroutine;
    private Coroutine hideCoroutine;

    public float FadeAwayWaitSeconds = 1f;
    public float AlphaIncrement = 0.1f;

    private void Start()
    {
        canvasGroup = GetComponent<CanvasGroup>();
    }

    private IEnumerator Brighten(bool reverse = true)
    {
        if (reverse)
            while (canvasGroup.alpha < 1)
            {
                canvasGroup.alpha += AlphaIncrement;
                yield return new WaitForSeconds(FadeAwayWaitSeconds * Time.deltaTime);
            }
        else
            while (canvasGroup.alpha > 0)
            {
                canvasGroup.alpha -= AlphaIncrement;
                yield return new WaitForSeconds(FadeAwayWaitSeconds * Time.deltaTime);
            }
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        if (hideCoroutine != null) StopCoroutine(hideCoroutine);
        brightenCoroutine = StartCoroutine(Brighten(true));
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        if (brightenCoroutine != null) StopCoroutine(brightenCoroutine);
        hideCoroutine = StartCoroutine(Brighten(false));
    }
}
